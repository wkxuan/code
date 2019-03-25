using System;
using System.Collections.Generic;
using z.Extensions;
using System.Data;
using z.POS.Entities.Pos;
using System.Linq;
using z.ServiceHelper;
using System.Diagnostics;
using z.ERP.API.PosServiceAPI;

namespace z.POS.Services
{


    public class PosService : ServiceBase
    {

        internal PosService()
        {

        }
        #region 属性
        PosWebServiceSoap _posapi;

        PosWebServiceSoap PosAPI
        {
            get
            {
                if (_posapi == null)
                {
                    _posapi = WCF.CreateWCFServiceByURL<PosWebServiceSoap>(ConfigExtension.GetConfig("PosServiceUrl"));
                }
                return _posapi;
            }
        }
        #endregion
        public LoginConfigInfo GetConfig()
        {
            /* string sql = " select S.BRANCHID,P.SHOPID,P.CODE SHOPCODE,P.NAME SHOPNAME,G.PID,G.KEY"
                        + " from STATION S, POSO2OWFTCFG G, SHOP P"
                        + " where S.STATIONBH = G.POSNO(+)"
                        + " AND S.SHOPID = P.SHOPID(+)"
                        + $" AND S.STATIONBH = '{employee.PlatformId}'";  */

            string sql = "select b.fdbh BRANCHID,a.shopid,'' shopcode,'' shopname,'' pid,'' key  from RYXX a,SKT b"
                       + $" where person_id={employee.Id} and b.sktno='{employee.PlatformId}'";

            LoginConfigInfo lgi = DbHelper.ExecuteOneObject<LoginConfigInfo>(sql);
            return lgi;  

        }

        /// <summary>
        /// 最大交易号
        /// </summary>
        /// <returns></returns>
        public long GetLastDealid()
        {
            string sql = $"select nvl(max(jlbh),0) from xsjl where sktno = '{employee.PlatformId}'";

            long lastDealid = long.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());

            sql = $"select nvl(max(jlbh),0) from sktxsjl where sktno = '{employee.PlatformId}'";
            long lastDealid_his = long.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());

            return (lastDealid > lastDealid_his) ? lastDealid : lastDealid_his;
        }

       public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            string sql = "select a.sp_id goodsid,a.spcode goodscode,a.name,0 type,nvl(a.lsdj,0) price,nvl(a.hylsdj,0) member_price,b.deptid shopid";
            sql += "        from SPXX a,GTSP b where a.sp_id=b.sp_id";

            if (filter.shopid.HasValue)
                sql += $"  and a.shopid = {filter.shopid}";
            if (filter.goodscode.IsNotEmpty())
                sql += $"  and (a.spcode = '{filter.goodscode}' or a.barcode = '{filter.goodscode}')";

            List<FindGoodsResult> goodsList = DbHelper.ExecuteObject<FindGoodsResult>(sql);

            if (goodsList.Count <= 0)
                throw new Exception("商品不存在或不属于此店铺!");

            return goodsList;
        }  

        public UserYYYResult GetClerkShop(PersonInfo req)    //暂不使用
        {
            string sql = "select a.userid,a.username,a.user_type,a.void_flag,a.shopid,b.code shopcode,b.name shopname";
            sql += $" from sysuser a,shop b where a.shopid=b.shopid(+) and a.usercode='{req.usercode}'";

            DataTable dt = DbHelper.ExecuteTable(sql);

            if (!dt.IsNotNull())
            {
                throw new Exception("营业员不存在!");

            }
            if (dt.Rows[0][2].ToString() != "2")
            {
                throw new Exception("类型不是营业员!");
            }
            if (dt.Rows[0][3].ToString() != "2")
            {
                throw new Exception("营业员已停用!");
            }

            return new UserYYYResult()
            {
                clerkId = dt.Rows[0][0].ToString(),
                clerkName = dt.Rows[0][1].ToString(),
                shopId = dt.Rows[0][4].ToString(),
                shopCode = dt.Rows[0][5].ToString(),
                shopName = dt.Rows[0][6].ToString()
            };

        }

        public List<FKFSResult> GetPayList()
        {   //decode(b.type,0,1,7,4,20,6,21,5,0)  -- 8.0
            //decode(b.type,0,1,7,4,13,6,14,5,0)  -- 10.0
            //decode(b.type,0,1,7,4,13,6,14,5,20,6,21,5,1)  --8.0 10.0
            string sql = $"select b.code payid,b.name,decode(b.type,0,1,7,4,13,6,14,5,20,6,21,5,1) type,2 fk,b.bj_jf jf,b.zlclfs zlfs,b.xssx flag";
            sql += $"        from skt_skfs a,skfs b";
            sql += $"       where a.skfsid = b.code";
            sql += $"         and a.sktno = '{employee.PlatformId}'";
            sql += $"       order by b.xssx";

            List<FKFSResult> payList = DbHelper.ExecuteObject<FKFSResult>(sql);
            if (payList.Count <= 0)
                throw new Exception("该款台没有配置支付方式!");

            return payList;
        }


        public SaleRequest GetDeal(GetDealFilter filter)
        {
            string strTable = "";
            string posNo;

            if (String.IsNullOrEmpty(filter.posno))
                posNo = employee.PlatformId;
            else
                posNo = filter.posno;

            string sql = $"select count(1) from xsjl where sktno='{posNo}' and jlbh={filter.dealid}";
            int saleCount = int.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());
            if (saleCount == 0)
            {
                strTable = "skt";
            }

            string sqlSale = "select sktno posno,jlbh dealid,jysj sale_time,jzrq account_date,sky cashierid,xsje sale_amount,zlje change_amount,";
            sqlSale += " nvl(vipid,-1) member_cardid,nvl(xfjlid,-1) crm_recordid,";
            sqlSale += $" sktno_old posno_old,nvl(jlbh_old,-1) dealid_old from {strTable}xsjl";
            sqlSale += $" where sktno='{posNo}' and jlbh={filter.dealid}";

            string sqlGoods = "select tckt_inx sheetid,inx,deptid shopid,sp_id goodsid,barcode goodscode,lsdj price,xssl quantity,";
            sqlGoods += $" xsje sale_amount,zkje discount_amount,yhje coupon_amount from {strTable}xsjlc";
            sqlGoods += $" where sktno='{posNo}' and jlbh={filter.dealid}";

            string sqlPay = $"select skfs payid,skje amount,remarks from {strTable}xsjlm";
            sqlPay += $" where sktno='{posNo}' and jlbh={filter.dealid}";

            string sqlClerk = $"select tckt_inx sheetid,yyy clerkid from {strTable}xsjlt";
            sqlClerk += $" where sktno='{posNo}' and jlbh={filter.dealid}";

            DataTable saleDt = DbHelper.ExecuteTable(sqlSale);
            //   List<SaleRequest> saleList = DbHelper.ExecuteObject<SaleRequest>(sqlSale);

            if (!saleDt.IsNotNull())
                throw new Exception("销售记录不存在!");

            //   saleList[0].goodslist = DbHelper.ExecuteObject<GoodsResult>(sqlGoods);
            //   saleList[0].paylist = DbHelper.ExecuteObject<PayResult>(sqlPay);
            //   saleList[0].clerklist = DbHelper.ExecuteObject<ClerkResult>(sqlClerk);

            return new SaleRequest()
            {
                posno = saleDt.Rows[0][0].ToString(),
                dealid = saleDt.Rows[0][1].ToString().ToInt(),
                sale_time = saleDt.Rows[0][2].ToString().ToDateTime(),
                account_date = saleDt.Rows[0][3].ToString().ToDateTime(),
                cashierid = saleDt.Rows[0][4].ToString().ToInt(),
                sale_amount = saleDt.Rows[0][5].ToString().ToDecimal(),
                change_amount = saleDt.Rows[0][6].ToString().ToDecimal(),
                member_cardid = saleDt.Rows[0][7].ToString(),
                crm_recordid = saleDt.Rows[0][8].ToString().ToInt(),
                posno_old = saleDt.Rows[0][9].ToString(),
                dealid_old = saleDt.Rows[0][10].ToString().ToInt(),
                goodslist = DbHelper.ExecuteObject<GoodsResult>(sqlGoods),
                paylist = DbHelper.ExecuteObject<PayResult>(sqlPay),
                clerklist = DbHelper.ExecuteObject<ClerkResult>(sqlClerk)
            };
        }

        public void Sale(SaleRequest request)
        {
            string posNo;
            if (String.IsNullOrEmpty(request.posno))
                posNo = employee.PlatformId;
            else
                posNo = request.posno;

            int dealId = request.dealid;


            decimal sumGoodsAmount = request.goodslist.Sum(a => a.sale_amount);
            decimal sumPayAmount = request.paylist.Sum(a => a.amount);

            if (sumGoodsAmount != sumPayAmount)
            {
                throw new Exception("商品列表中金额合计与支付列表中的金额合计不相等!");
            }

            string sql = $"select xsje from xsjl where sktno='{posNo}' and jlbh={dealId} ";

            DataTable Dt = DbHelper.ExecuteTable(sql);


            if (Dt.IsNotNull())
            {
                decimal saleAmount = Dt.Rows[0][0].ToString().ToDecimal();

                if (Math.Abs(saleAmount - sumGoodsAmount) >= 0.01m)
                {
                    throw new Exception("小票号" + dealId.ToString() + "已存在!");
                }


            }
            else
            {
                int goodsCount = request.goodslist.Count;
                int payCount = request.paylist.Count;
                int clerkCount = request.clerklist.Count;

                string[] sqlarr = new string[1 + goodsCount + payCount + clerkCount];  // + goodsCount * payCount

                sqlarr[0] = "insert into xsjl(sktno,jlbh,jysj,jzrq,sky,xsje,";
                sqlarr[0] += "zlje,vipid,xfjlid,sktno_old,jlbh_old)";
                sqlarr[0] += $"values('{posNo}',{request.dealid},"; //to_date('{request.sale_time}','yyyy-mm-dd HH24:MI:SS'),";
                sqlarr[0] += "sysdate,trunc(sysdate),";
                //    if (request.account_date.ToString().IsEmpty())
                //        sqlarr[0] += $"to_date('{request.sale_time.Date}','yyyy-mm-dd'),";
                //    else
                //        sqlarr[0] += $"to_date('{request.account_date.Date}','yyyy-mm-dd'),";
                sqlarr[0] += $"{request.cashierid},{sumGoodsAmount},{request.change_amount},";

                if (request.member_cardid.IsEmpty())
                    sqlarr[0] += $"-1,";
                else
                    sqlarr[0] += $"{request.member_cardid},";
                if (request.crm_recordid.ToString().IsEmpty())
                    sqlarr[0] += $"-1,";
                else
                    sqlarr[0] += $"{ request.crm_recordid},";

                if (!String.IsNullOrEmpty(request.posno_old))
                    sqlarr[0] += $"'{request.posno_old}',";
                else
                    sqlarr[0] += "null,";

                if (request.dealid_old.HasValue && request.dealid_old > 0)
                    sqlarr[0] += $"{request.dealid_old})";
                else
                    sqlarr[0] += "null)";


                int j = 0;

                for (int i = 1; i <= goodsCount; i++)
                {
                    sqlarr[i] = "insert into xsjlc(sktno,jlbh,tckt_inx,inx,deptid,sp_id,barcode,";
                    sqlarr[i] += "lsdj,xssl,xsje,zkje,yhje)";
                    sqlarr[i] += $"values('{posNo}',{request.dealid},{request.goodslist[j].sheetid},";
                    sqlarr[i] += $"{request.goodslist[j].inx},{request.goodslist[j].shopid},{request.goodslist[j].goodsid},";
                    sqlarr[i] += $"'{request.goodslist[j].goodscode}',{request.goodslist[j].price},{request.goodslist[j].quantity},";
                    sqlarr[i] += $"{request.goodslist[j].sale_amount},{request.goodslist[j].discount_amount},";
                    sqlarr[i] += $"{request.goodslist[j].coupon_amount})";
                    j++;
                }

                j = 0;

                for (int i = 1 + goodsCount; i <= goodsCount + payCount; i++)
                {

                    sqlarr[i] = "insert into xsjlm(sktno,jlbh,skfs,skje,remarks)";
                    sqlarr[i] += $"values('{posNo}',{request.dealid},{request.paylist[j].payid},{request.paylist[j].amount},";
                    if (request.paylist[j].remarks.IsEmpty())
                        sqlarr[i] += "null)";
                    else
                        sqlarr[i] += $"'{request.paylist[j].remarks}')";
                    j++;
                }

                j = 0;
                for (int i = 1 + goodsCount + payCount; i <= goodsCount + payCount + clerkCount; i++)
                {
                    sqlarr[i] = "insert into xsjlt(sktno,jlbh,tckt_inx,yyy,deptid)";
                    sqlarr[i] += $"values('{posNo}',{request.dealid},{request.clerklist[j].sheetid},{request.clerklist[j].clerkid},{request.goodslist[0].shopid})";
                    j++;
                }

                /*  decimal goodsPayAmount = 0;
                  decimal payAmount = 0;
                  j = 1 + goodsCount + payCount + clerkCount;
                  int inx = 0;
                  for (int m = 0; m < request.paylist.Count(); m++)
                  {
                      payAmount = request.paylist[m].amount;
                      inx = 0;
                      for (int n = 0; n < request.goodslist.Count(); n++)
                      {
                          goodsPayAmount = Math.Round(request.paylist[m].amount * request.goodslist[n].sale_amount / sumGoodsAmount, 2);
                          payAmount = payAmount - goodsPayAmount;

                          if (n == request.paylist.Count() - 1 && payAmount != 0)  //尾差放到最后一行
                              goodsPayAmount = goodsPayAmount + payAmount;


                          sqlarr[j] = "insert into sale_goods_pay(posno,dealid,goodsid,payid,amount,inx)";
                          sqlarr[j] += $"values('{posNo}',{request.dealid},{request.goodslist[n].goodsid},{request.paylist[m].payid},{goodsPayAmount},{inx})";
                          j++;
                          inx++;
                      }
                  } */



                int insertCount = 0;
                try
                {
                    using (var Tran = DbHelper.BeginTransaction())
                    {
                        insertCount = DbHelper.ExecuteNonQuery(sqlarr);
                        Tran.Commit();
                    }
                }
                catch (Exception e)
                {

                    throw new Exception("提交数据库时发生异常:" + e +":"+ String.Join(";", sqlarr));
                }

            }

            /*   if (insertCount != 1 + goodsCount + payCount + clerkCount + goodsCount * payCount)
               {
                   throw new Exception("写入数据不完整!");
               } */
        }

        public SaleSummaryResult GetSaleSummary(SaleSummaryFilter filter)
        {
            string sql = $"select s.sktno posno,s.jlbh dealid,decode(sign(s.xsje),0,0,1,0,-1,1) returnflag,"
                   + "       to_char(s.jysj,'yyyy-mm-dd HH24:MI:SS') sale_time,p.skfs payid,y.name payname,decode(y.type,0,1,7,4,13,6,14,5,20,6,21,5,1) paytype, p.skje amount"
                   + "  from xsjl s, xsjlm p,skfs y"
                   + " where s.sktno = p.sktno"
                   + "   and s.jlbh = p.jlbh"
                   + "   and p.skfs = y.code";
            if (String.IsNullOrEmpty(filter.posno))
                sql += $" and s.sktno = '{employee.PlatformId}'";
            else
                sql += $" and s.sktno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sql += $" and trunc(s.jysj) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sql += $" and trunc(s.jysj) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.jysj) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sql += $" and trunc(s.jysj) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.jysj) <= trunc(sysdate)";
            }

            sql += " union all ";

            sql += $"select s.sktno posno,s.jlbh dealid,decode(sign(s.xsje),0,0,1,0,-1,1) returnflag,"
                  + "       to_char(s.jysj,'yyyy-mm-dd HH24:MI:SS') sale_time,p.skfs payid,y.name payname,decode(y.type,0,1,7,4,13,6,14,5,20,6,21,5,1) paytype, p.skje amount"
                  + "  from sktxsjl s, sktxsjlm p,skfs y"
                  + " where s.sktno = p.sktno"
                  + "   and s.jlbh = p.jlbh"
                  + "   and p.skfs = y.code";
            if (String.IsNullOrEmpty(filter.posno))
                sql += $" and s.sktno = '{employee.PlatformId}'";
            else
                sql += $" and s.sktno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sql += $" and trunc(s.jysj) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sql += $" and trunc(s.jysj) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.jysj) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sql += $" and trunc(s.jysj) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.jysj) <= trunc(sysdate)";
            }

            List<PayDetailResult> detaillist = DbHelper.ExecuteObject<PayDetailResult>(sql);

            if (detaillist.Count <= 0)
                throw new Exception("无销售记录");

            string sqlsum = "select payid,payname,sum(returnflag * amount) amountreturn,sum(amount) amountsum from(";



            sqlsum += $"select s.sktno posno,s.jlbh dealid,decode(sign(s.xsje),0,0,1,0,-1,1) returnflag,"
                          + "       p.skfs payid,y.name payname, p.skje amount"
                          + "  from xsjl s, xsjlm p,skfs y"
                          + " where s.sktno = p.sktno"
                          + "   and s.jlbh = p.jlbh"
                          + "   and p.skfs = y.code";
            if (String.IsNullOrEmpty(filter.posno))
                sqlsum += $" and s.sktno = '{employee.PlatformId}'";
            else
                sqlsum += $" and s.sktno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sqlsum += $" and trunc(s.jysj) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sqlsum += $" and trunc(s.jysj) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.jysj) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sqlsum += $" and trunc(s.jysj) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.jysj) <= trunc(sysdate)";
            }

            sqlsum += " union all ";

            sqlsum += $"select s.sktno posno,s.jlbh dealid,decode(sign(s.xsje),0,0,1,0,-1,1) returnflag,"
                  + "       p.skfs payid,y.name payname, p.skje amount"
                  + "  from sktxsjl s, sktxsjlm p,skfs y"
                  + " where s.sktno = p.sktno"
                  + "   and s.jlbh = p.jlbh"
                  + "   and p.skfs = y.code";
            if (String.IsNullOrEmpty(filter.posno))
                sqlsum += $" and s.sktno = '{employee.PlatformId}'";
            else
                sqlsum += $" and s.sktno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sqlsum += $" and trunc(s.jysj) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sqlsum += $" and trunc(s.jysj) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.jysj) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sqlsum += $" and trunc(s.jysj) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.jysj) <= trunc(sysdate)";
            }

            sqlsum += ") group by payid,payname";

            List<PaySumResult> sumlist = DbHelper.ExecuteObject<PaySumResult>(sqlsum);
            decimal salesum = detaillist.Sum(a => a.amount);
            decimal salereturn = sumlist.Sum(a => a.amountreturn);

            return new SaleSummaryResult()
            {
                saleamountsum = salesum,
                saleamountreturn = salereturn,
                paysumlist = sumlist,
                paydetaillist = detaillist
            };
        }

    }
}
