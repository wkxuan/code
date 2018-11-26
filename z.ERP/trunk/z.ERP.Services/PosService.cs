using System;
using System.Collections.Generic;
using z.ERP.Entities.Service.Pos;
using z.Extensions;
using System.Data;
using System.Linq;
using z.ERP.API.PosServiceAPI;
using z.ServiceHelper;

namespace z.ERP.Services
{
    public class PosService : ServiceBase
    {
        internal PosService()
        {

        }

        public void a()
        {

        }

        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            string sql = "select a.goodsid,a.goodsdm goodscode,a.name,a.type,nvl(a.price,0) price,nvl(a.member_price,0) member_price,b.shopid";
            sql += "        from GOODS a,GOODS_SHOP b where a.goodsid=b.goodsid";

            if (filter.shopid.HasValue)
                sql += $"  and b.shopid = {filter.shopid}";
            if (filter.goodscode.IsNotEmpty())
                sql += $"  and (goodsdm = '{filter.goodscode}' or barcode = '{filter.goodscode}')";

            List<FindGoodsResult> goodsList = DbHelper.ExecuteObject<FindGoodsResult>(sql);

            if (goodsList.Count <= 0)
                throw new Exception("商品不存在或不属于此店铺!");

            return goodsList;
        }

        /// <summary>
        /// 最大交易号
        /// </summary>
        /// <returns></returns>
        public long GetLastDealid()
        {
            string sql = $"select nvl(max(dealid),0) from sale where posno = '{employee.PlatformId}'";

            long lastDealid = long.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());

            if (lastDealid == 0)
            {
                sql = $"select nvl(max(dealid),0) from his_sale where posno = '{employee.PlatformId}'";
                lastDealid = long.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());
            }

            return lastDealid;
        }

        public UserYYYResult GetClerkShop(string usercode)
        {
            string sql = "select a.userid,a.username,a.user_type,a.void_flag,a.shopid,b.code shopcode,b.name shopname";
            sql += $" from sysuser a,shop b where a.shopid=b.shopid(+) and a.usercode='{usercode}'";

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
                throw new Exception("营业员状态不正确!");
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
        {
            string sql = $"select b.payid,b.name,b.type,b.fk,b.jf,b.zlfs,b.flag ";
            sql += $"        from station_pay a,pay b";
            sql += $"       where a.payid = b.payid";
            sql += $"         and void_flag = 1 and a.stationbh = '{employee.PlatformId}'";
            sql += $"       order by b.flag";

            List<FKFSResult> payList = DbHelper.ExecuteObject<FKFSResult>(sql);
            if (payList.Count <= 0)
                throw new Exception("该款台没有配置支付方式!");

            return payList;
        }

        public SaleRequest GetDeal(GetDealFilter filter)
        {
            string strTable = "";
            string sql = $"select count(1) from sale where posno='{filter.posno}' and dealid={filter.dealid}";
            int saleCount = int.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());
            if (saleCount == 0)
            {
                strTable = "his_";
            }

            string sqlSale = "select posno,dealid,sale_time,account_date,cashierid,sale_amount,change_amount,";
            sqlSale += " nvl(member_cardid,-1) member_cardid,nvl(crm_recordid,-1) crm_recordid,";
            sqlSale += $" posno_old,nvl(dealid_old,-1) dealid_old from {strTable}sale";
            sqlSale += $" where posno='{filter.posno}' and dealid={filter.dealid}";

            string sqlGoods = "select sheetid,inx,shopid,goodsid,goodscode,price,quantity,";
            sqlGoods += $" sale_amount,discount_amount,coupon_amount from {strTable}sale_goods";
            sqlGoods += $" where posno='{filter.posno}' and dealid={filter.dealid}";

            string sqlPay = $"select payid,amount from {strTable}sale_pay";
            sqlPay += $" where posno='{filter.posno}' and dealid={filter.dealid}";

            string sqlClerk = $"select sheetid,clerkid from {strTable}sale_clerk";
            sqlClerk += $" where posno='{filter.posno}' and dealid={filter.dealid}";

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
            string posNo = request.posno;
            decimal sumGoodsAmount = request.goodslist.Sum(a => a.sale_amount);
            decimal sumPayAmount = request.paylist.Sum(a => a.amount);

            if (sumGoodsAmount != sumPayAmount)
            {
                throw new Exception("商品列表中金额合计与支付列表中的金额合计不相等!");
            }

            int goodsCount = request.goodslist.Count;
            int payCount = request.paylist.Count;
            int clerkCount = request.clerklist.Count;

            string[] sqlarr = new string[1 + goodsCount + payCount + clerkCount + goodsCount * payCount];

            sqlarr[0] = "insert into sale(posno,dealid,sale_time,account_date,cashierid,sale_amount,";
            sqlarr[0] += "change_amount,member_cardid,crm_recordid,posno_old,dealid_old)";
            sqlarr[0] += $"values('{posNo}',{request.dealid},to_date('{request.sale_time}','yyyy-mm-dd HH24:MI:SS'),";
            if (request.account_date.ToString().IsEmpty())
                sqlarr[0] += $"to_date('{request.sale_time.Date}','yyyy-mm-dd HH24:MI:SS'),";
            else
                sqlarr[0] += $"to_date('{request.account_date.Date}','yyyy-mm-dd HH24:MI:SS'),";
            sqlarr[0] += $"{request.cashierid},{sumGoodsAmount},{request.change_amount},";

            if (request.member_cardid.IsEmpty())
                sqlarr[0] += $"-1,";
            else
                sqlarr[0] += $"{request.member_cardid},";
            if (request.crm_recordid.ToString().IsEmpty())
                sqlarr[0] += $"-1,";
            else
                sqlarr[0] += $"{ request.crm_recordid},";

            if (request.posno.IsNotEmpty() || request.posno == "")
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
                sqlarr[i] = "insert into sale_goods(posno,dealid,sheetid,inx,shopid,goodsid,goodscode,";
                sqlarr[i] += "price,quantity,sale_amount,discount_amount,coupon_amount)";
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

                sqlarr[i] = "insert into sale_pay(posno,dealid,payid,amount,remarks)";
                sqlarr[i] += $"values('{posNo}',{request.dealid},{request.paylist[j].payid},{request.paylist[j].amount},{request.paylist[j].remarks})";
                j++;
            }

            j = 0;
            for (int i = 1 + goodsCount + payCount; i <= goodsCount + payCount + clerkCount; i++)
            {
                sqlarr[i] = "insert into sale_clerk(posno,dealid,sheetid,clerkid)";
                sqlarr[i] += $"values('{posNo}',{request.dealid},{request.clerklist[j].sheetid},{request.clerklist[j].clerkid})";
                j++;
            }

            string sqlGoodsPay = "insert into sale_goods_pay(posno,dealid,goodsid,payid,amount)";
            sqlGoodsPay += $"values()";

            decimal goodsPayAmount = 0;
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
            }


            int insertCount = 0;
            try
            {
                insertCount = DbHelper.ExecuteNonQuery(sqlarr);
            }
            catch (Exception e)
            {

                throw new Exception("提交数据库时发生异常:" + e);
            }

            if (insertCount != 1 + goodsCount + payCount + clerkCount + goodsCount * payCount)
            {
                throw new Exception("写入数据不完整!");
            }
        }

        public SaleSummaryResult GetSaleSummary(SaleSummaryFilter filter)
        {
            string sql = $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                   + "       s.sale_time,p.payid,y.name payname, p.amount"
                   + "  from sale s, sale_pay p,pay y"
                   + " where s.posno = p.posno"
                   + "   and s.dealid = p.dealid"
                   + "   and p.payid = y.payid";
            if (filter.posno.IsEmpty())
                sql += $" and s.posno = '{employee.PlatformId}'";
            else
                sql += $" and s.posno = '{filter.posno}'";

            if (!filter.saledate_begin.HasValue && !filter.saledate_end.HasValue)
                sql += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (filter.saledate_begin.HasValue)
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date('{filter.saledate_begin}','yyyy-mm-dd')";
                else
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date(trunc(sysdate),'yyyy-mm-dd')";
                if (filter.saledate_end.HasValue)
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date('{filter.saledate_end}','yyyy-mm-dd')";
                else
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date(trunc(sysdate),'yyyy-mm-dd')";
            }

            sql += " union all ";

            sql += $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                  + "       s.sale_time,p.payid,y.name payname, p.amount"
                  + "  from his_sale s, his_sale_pay p,pay y"
                  + " where s.posno = p.posno"
                  + "   and s.dealid = p.dealid"
                  + "   and p.payid = y.payid";
            if (filter.posno.IsEmpty())
                sql += $" and s.posno = '{employee.PlatformId}'";
            else
                sql += $" and s.posno = '{filter.posno}'";

            if (!filter.saledate_begin.HasValue && !filter.saledate_end.HasValue)
                sql += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (filter.saledate_begin.HasValue)
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date('{filter.saledate_begin}','yyyy-mm-dd')";
                else
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date(trunc(sysdate),'yyyy-mm-dd')";
                if (filter.saledate_end.HasValue)
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date('{filter.saledate_end}','yyyy-mm-dd')";
                else
                    sql += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date(trunc(sysdate),'yyyy-mm-dd')";
            }

            List<PayDetailResult> detaillist = DbHelper.ExecuteObject<PayDetailResult>(sql);

            if (detaillist.Count <= 0)
                throw new Exception("无销售记录");

            string sqlsum = "select payid,payname,sum(returnflag * amount) amountreturn,sum(amount) amountsum from(";



            sqlsum += $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                          + "       p.payid,y.name payname, p.amount"
                          + "  from sale s, sale_pay p,pay y"
                          + " where s.posno = p.posno"
                          + "   and s.dealid = p.dealid"
                          + "   and p.payid = y.payid";
            if (filter.posno.IsEmpty())
                sqlsum += $" and s.posno = '{employee.PlatformId}'";
            else
                sqlsum += $" and s.posno = '{filter.posno}'";

            if (!filter.saledate_begin.HasValue && !filter.saledate_end.HasValue)
                sqlsum += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (filter.saledate_begin.HasValue)
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date('{filter.saledate_begin}','yyyy-mm-dd')";
                else
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date(trunc(sysdate),'yyyy-mm-dd')";
                if (filter.saledate_end.HasValue)
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date('{filter.saledate_end}','yyyy-mm-dd')";
                else
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date(trunc(sysdate),'yyyy-mm-dd')";
            }

            sqlsum += " union all ";

            sqlsum += $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                  + "       p.payid,y.name payname, p.amount"
                  + "  from his_sale s, his_sale_pay p,pay y"
                  + " where s.posno = p.posno"
                  + "   and s.dealid = p.dealid"
                  + "   and p.payid = y.payid";
            if (filter.posno.IsEmpty())
                sqlsum += $" and s.posno = '{employee.PlatformId}'";
            else
                sqlsum += $" and s.posno = '{filter.posno}'";

            if (!filter.saledate_begin.HasValue && !filter.saledate_end.HasValue)
                sqlsum += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (filter.saledate_begin.HasValue)
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date('{filter.saledate_begin}','yyyy-mm-dd')";
                else
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') >= to_date(trunc(sysdate),'yyyy-mm-dd')";
                if (filter.saledate_end.HasValue)
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date('{filter.saledate_end}','yyyy-mm-dd')";
                else
                    sqlsum += $" and to_date(trunc(s.sale_time),'yyyy-mm-dd') <= to_date(trunc(sysdate),'yyyy-mm-dd')";
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

        #region  PosService

        #region 属性
        IPOSService _posapi;

        IPOSService PosAPI
        {
            get
            {
                if (_posapi == null)
                {
                    _posapi = WCF.CreateWCFServiceByURL<IPOSService>(GetConfig("2001"));
                }
                return _posapi;
            }
        }
        #endregion

        public VipCard GetVipCard(GetVipCardRequest request)
        {
            GetVipCardResponse res = PosAPI.GetVipCard(request);
            if (res.GetVipCardResult)
            {
                return res.vipCard;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

        #endregion
    }
}
