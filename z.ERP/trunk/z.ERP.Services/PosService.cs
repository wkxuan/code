using System;
using System.Collections.Generic;
using z.ERP.Entities.Service.Pos;
using z.Extensions;
using System.Data;
using System.Linq;
using z.ERP.API.PosServiceAPI;
using z.ServiceHelper;
using System.Diagnostics;
using z.Encryption;
using z.ERP.Entities;

namespace z.ERP.Services
{


    public class PosService : ServiceBase
    {

        protected const string LoginSalt = "z.SSO.LoginSalt.1";

        internal PosService()
        {

        }

        public void a()
        {

        }


        public LoginConfigInfo GetConfig()
        {
            string sql = " select S.BRANCHID,B.CRMSTORECODE,P.SHOPID,P.CODE SHOPCODE,P.NAME SHOPNAME,UPPER(trim(S.NETWORK_NODE_ADDRESS)) MACADDRESS"
                       + " from BRANCH B,STATION S,SHOP P,SYSUSER R"
                       + " where B.ID= S.BRANCHID and S.SHOPID=R.SHOPID"
                       + "  AND R.USER_TYPE in (1,2) "
                       + " AND S.SHOPID = P.SHOPID(+)"
                       + $" AND S.STATIONBH = '{employee.PlatformId}'"
                       + $" AND R.USERID = {employee.Id}";


            string sqlUMS = " select ip,ip_bak,port,cfx_mchtid,cfx_termid,cfxmpay_mchtname,cfxmpay_mchtid,cfxmpay_termid"
                         + $"   from POSUMSCONFIG where POSNO = '{employee.PlatformId}'";

            string sqlWFT = " select url,pid,encryption,key,key_pub"
                         + $"   from POSO2OWFTCFG where POSNO = '{employee.PlatformId}'";



            LoginConfigInfo lgi = DbHelper.ExecuteOneObject<LoginConfigInfo>(sql);



            if (lgi!=null)
            {
                TicketInfo ticket = new TicketInfo();
                ticket.tickethead = ConfigExtension.GetConfig("TicketHead");
                ticket.tickettail = ConfigExtension.GetConfig("TicketTail");
                ticket.printCount = ConfigExtension.GetConfig("PrintCount");
                lgi.ticketInfo = ticket;
                lgi.posWFTConfig = DbHelper.ExecuteOneObject<PosWFTConfig>(sqlWFT);
                lgi.posUMSConfig = DbHelper.ExecuteOneObject<PosUMSConfig>(sqlUMS);
            }


            return lgi;

        }

        public void BindAddress(Address ads)
        {
            if (ads.address.IsEmpty())
            {
                throw new Exception("MAC地址为空！");
            }

            string address = ads.address;

            string sql = $"update STATION set NETWORK_NODE_ADDRESS=UPPER(trim('{address}')) where STATIONBH = '{employee.PlatformId}' and trim(NETWORK_NODE_ADDRESS) is null";

            try
            {
                int icount = DbHelper.ExecuteNonQuery(sql);

                if (icount == 0)
                {
                    throw new Exception("该终端已绑定MAC地址,不能重复绑定！");
                }
            }
            catch(Exception e)
            {
                throw new Exception("绑定MAC地址出错:" + e.ToString());
            }
         }

        public void ChangePassword(PasswordInfo passw)
        {
            if (string.IsNullOrEmpty(passw.oldPassword))
                throw new Exception("原密码不能为空");

            if (string.IsNullOrEmpty(passw.newPassword))
                throw new Exception("新密码不能为空");


            SYSUSEREntity sysuser = DbHelper.Select(new SYSUSEREntity() { USERID = employee.Id });

            if (sysuser.PASSWORD == salt(sysuser.USERID, passw.oldPassword))  //原密码验证
            {
                sysuser.PASSWORD = salt(sysuser.USERID, passw.newPassword);  //修改为新密码
                DbHelper.Save(sysuser);
            }
            else
            {
                throw new Exception("原密码验证失败,修改密码失败");
            }
        }

        //密码加密
        string salt(string userid, string pass)
        {
            return MD5Encryption.Encrypt(userid + LoginSalt + pass);
        }

        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            string sql = "select a.goodsid,a.goodsdm goodscode,a.name,a.type,nvl(a.price,0) price,nvl(a.member_price,0) member_price,b.shopid,d.orgcode";
            sql += "        from GOODS a,GOODS_SHOP b,SHOP c,ORG d";
            sql += "       where a.goodsid=b.goodsid and b.shopid=c.shopid and c.orgid=d.orgid";
            sql += "         and a.STATUS = 2";

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

            sql = $"select nvl(max(dealid),0) from his_sale where posno = '{employee.PlatformId}'";
            long lastDealid_his = long.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());

            return (lastDealid > lastDealid_his) ? lastDealid : lastDealid_his;
        }

        public UserYYYResult GetClerkShop(PersonInfo req)
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
            string posNo;

            if (String.IsNullOrEmpty(filter.posno))
                posNo = employee.PlatformId;
            else
                posNo = filter.posno;

            string sql = $"select count(1) from sale where posno='{posNo}' and dealid={filter.dealid}";
            int saleCount = int.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString());
            if (saleCount == 0)
            {
                strTable = "his_";
            }

            string sqlSale = "select posno,dealid,sale_time,account_date,cashierid,sale_amount,change_amount,";
            sqlSale += " nvl(member_cardid,-1) member_cardid,nvl(crm_recordid,-1) crm_recordid,";
            sqlSale += $" posno_old,nvl(dealid_old,-1) dealid_old from {strTable}sale";
            sqlSale += $" where posno='{posNo}' and dealid={filter.dealid}";

            string sqlGoods = "select sheetid,inx,shopid,goodsid,goodscode,price,quantity,";
            sqlGoods += $" sale_amount,discount_amount,coupon_amount from {strTable}sale_goods";
            sqlGoods += $" where posno='{posNo}' and dealid={filter.dealid}";

            string sqlPay = $"select a.payid,a.amount,b.name payname,b.type paytype from {strTable}sale_pay a,pay b";
            sqlPay += $" where a.payid=b.payid and a.posno='{posNo}' and a.dealid={filter.dealid}";

            string sqlClerk = $"select sheetid,clerkid from {strTable}sale_clerk";
            sqlClerk += $" where posno='{posNo}' and dealid={filter.dealid}";

            string sqlPayRecord = $"select a.inx,a.payid,a.cardno,a.bank,a.bankid,a.amount,a.serialno,a.refno,a.opertime,b.type paytype from payrecord a,pay b";
            sqlPayRecord += $" where a.payid=b.payid and a.posno='{posNo}' and a.dealid={filter.dealid}";

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
                clerklist = DbHelper.ExecuteObject<ClerkResult>(sqlClerk),
                payRecord = DbHelper.ExecuteObject<PayRecord>(sqlPayRecord)
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

            string sql = $"select sale_amount from SALE where POSNO='{posNo}' and DEALID={dealId} ";

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
                int payRecordCount = 0;
                if (request.payRecord != null)
                    payRecordCount = request.payRecord.Count;

                string[] sqlarr = new string[1 + goodsCount + payCount + clerkCount + payRecordCount + goodsCount * payCount];

                sqlarr[0] = "insert into sale(posno,dealid,sale_time,account_date,cashierid,sale_amount,";
                sqlarr[0] += "change_amount,member_cardid,crm_recordid,posno_old,dealid_old)";
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

                    sqlarr[i] = "insert into sale_pay(posno,dealid,payid,amount)";
                    sqlarr[i] += $"values('{posNo}',{request.dealid},{request.paylist[j].payid},{request.paylist[j].amount})";
                    j++;
                }

                j = 0;
                for (int i = 1 + goodsCount + payCount; i <= goodsCount + payCount + clerkCount; i++)
                {
                    sqlarr[i] = "insert into sale_clerk(posno,dealid,sheetid,clerkid)";
                    sqlarr[i] += $"values('{posNo}',{request.dealid},{request.clerklist[j].sheetid},{request.clerklist[j].clerkid})";
                    j++;
                }

                j = 0;
                for (int i = 1 + goodsCount + payCount + clerkCount; i <= goodsCount + payCount + clerkCount + payRecordCount; i++)
                {
                    sqlarr[i] = "insert into payrecord(posno,dealid,inx,payid,cardno,bank,bankid,amount,serialno,refno,opertime)";
                    sqlarr[i] += $"values('{posNo}',{request.dealid},{request.payRecord[j].inx},{request.payRecord[j].payid},";
                    sqlarr[i] += $"'{request.payRecord[j].cardno}','{request.payRecord[j].bank}',{request.payRecord[j].bankid},";
                    sqlarr[i] += $"{request.payRecord[j].amount},'{request.payRecord[j].serialno}','{request.payRecord[j].refno}',";

                    if (request.payRecord[j].opertime.ToString().IsEmpty())
                        sqlarr[i] += "sysdate)";
                    else
                        sqlarr[i] += $" to_date('{request.payRecord[j].opertime}', 'yyyy-mm-dd HH24:MI:SS'))";
                    j++;
                }

                decimal goodsPayAmount = 0;
                decimal payAmount = 0;
                j = 1 + goodsCount + payCount + clerkCount + payRecordCount;
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
                    using (var Tran = DbHelper.BeginTransaction())
                    {
                        insertCount = DbHelper.ExecuteNonQuery(sqlarr);
                        Tran.Commit();
                    }
                }
                catch (Exception e)
                {

                    throw new Exception("提交数据库时发生异常:" + e);
                }

            }

            /*   if (insertCount != 1 + goodsCount + payCount + clerkCount + goodsCount * payCount)
               {
                   throw new Exception("写入数据不完整!");
               } */
        }

        public SaleSummaryResult GetSaleSummary(SaleSummaryFilter filter)
        {
            string sql = $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                   + "       to_char(s.sale_time,'yyyy-mm-dd HH24:MI:SS') sale_time,p.payid,y.name payname,y.type paytype, p.amount"
                   + "  from sale s, sale_pay p,pay y"
                   + " where s.posno = p.posno"
                   + "   and s.dealid = p.dealid"
                   + "   and p.payid = y.payid";
            if (String.IsNullOrEmpty(filter.posno))
                sql += $" and s.posno = '{employee.PlatformId}'";
            else
                sql += $" and s.posno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sql += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sql += $" and trunc(s.sale_time) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.sale_time) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sql += $" and trunc(s.sale_time) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.sale_time) <= trunc(sysdate)";
            }

            sql += " union all ";

            sql += $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                  + "       to_char(s.sale_time,'yyyy-mm-dd HH24:MI:SS') sale_time,p.payid,y.name payname,y.type paytype, p.amount"
                  + "  from his_sale s, his_sale_pay p,pay y"
                  + " where s.posno = p.posno"
                  + "   and s.dealid = p.dealid"
                  + "   and p.payid = y.payid";
            if (String.IsNullOrEmpty(filter.posno))
                sql += $" and s.posno = '{employee.PlatformId}'";
            else
                sql += $" and s.posno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sql += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sql += $" and trunc(s.sale_time) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.sale_time) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sql += $" and trunc(s.sale_time) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sql += $" and trunc(s.sale_time) <= trunc(sysdate)";
            }

            sql += " order by 4 desc"; //按交易时间倒序排列

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
            if (String.IsNullOrEmpty(filter.posno))
                sqlsum += $" and s.posno = '{employee.PlatformId}'";
            else
                sqlsum += $" and s.posno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sqlsum += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sqlsum += $" and trunc(s.sale_time) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.sale_time) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sqlsum += $" and trunc(s.sale_time) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.sale_time) <= trunc(sysdate)";
            }

            sqlsum += " union all ";

            sqlsum += $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                  + "       p.payid,y.name payname, p.amount"
                  + "  from his_sale s, his_sale_pay p,pay y"
                  + " where s.posno = p.posno"
                  + "   and s.dealid = p.dealid"
                  + "   and p.payid = y.payid";
            if (String.IsNullOrEmpty(filter.posno))
                sqlsum += $" and s.posno = '{employee.PlatformId}'";
            else
                sqlsum += $" and s.posno = '{filter.posno}'";

            if (String.IsNullOrEmpty(filter.saledate_begin) && String.IsNullOrEmpty(filter.saledate_end))
                sqlsum += $" and trunc(s.sale_time) = trunc(sysdate)";
            else
            {
                if (!String.IsNullOrEmpty(filter.saledate_begin))
                    sqlsum += $" and trunc(s.sale_time) >= to_date('{filter.saledate_begin.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.sale_time) >= trunc(sysdate)";
                if (!String.IsNullOrEmpty(filter.saledate_end))
                    sqlsum += $" and trunc(s.sale_time) <= to_date('{filter.saledate_end.ToDateTime().ToString("yyyy-MM-dd")}','yyyy-MM-dd')";
                else
                    sqlsum += $" and trunc(s.sale_time) <= trunc(sysdate)";
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
        PosWebServiceSoap _posapi;

        PosWebServiceSoap PosAPI
        {
            get
            {
                if (_posapi == null)
                {
                    _posapi = WCF.CreateWCFServiceByURL<PosWebServiceSoap>(GetConfig("2001"));
                }
                return _posapi;
            }
        }
        #endregion

        #endregion

/**
    * 
    * 调用长益crm接口
    * 
    * 
    **/

        public CalcAccountsPayableResult CalcAccountsPayable(ReqGetGoods reqMth)
        {
            if (reqMth == null)
            {
                throw new Exception("请求参数为空");
            }
            

            CalcAccountsPayableResult payableResult = new CalcAccountsPayableResult();

            MemberCard vipcard = new MemberCard();
            CashCardDetails cashCard = new CashCardDetails();
            List<Goods> GoodsList = new List<Goods>();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();
            List<Payment> DevicePayments = new List<Payment>();

            int iHTH = 0;
            string Shop = reqMth.storeCode;
            string posNo = employee.PlatformId, userCode = employee.Code,
                 cardCodeToCheck = "", verifyCode = "", password = "",
                 CondValue = "", sVIPCode = "", sDeptCode = "";
            int i = 0, j = 0, transId = 0, shopId = 0, deptid = 0,
                  CrmBillId = 0, backType = 0, bulkGoodsType = 0, iVIPID = -1;

            string Operator = employee.Id;
            string sTitle = "SysVer", sVer = "", message = "", msg = "";

            Goods goods = new Goods();

            //1.1:检查基本输入数据

            //  string input = " 店:" + Shop + " 设备号:" + posNo + " 用户代码:" + userCode + " 输入:" + sInput;
            //   CommonUtils.WriteSKTLog(0, posNo, "计算销售价格" + input);


            //1.2:Json取数据
            //   ReqGetGoods reqMth = new ReqGetGoods();

            shopId = Convert.ToInt32(Shop);

            if (string.IsNullOrEmpty(reqMth.ValidID))
                reqMth.ValidID = "";

            if (string.IsNullOrEmpty(reqMth.deptCode))
                reqMth.deptCode = "";

            iHTH = reqMth.contractID;
            Stopwatch st = new Stopwatch();

            try
            {
                st.Start();

                //2.1: 取收款台 

                DoGetPayments(posNo, out DevicePayments, out msg);

                /* result = DoGetPayments2(Device, out DevicePayments, out msg);
                 if (result != 0)
                 {
                     CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<4.1> 查询数据定义失败：没有收款方式定义");

                     result = RsltCode_Wrong_NoDef;
                     msg = "计算销售价格失败：没有收款方式定义";
                     return result;
                 } */


                //2.2:如果有会员.则取会员      
                //  int iMemberType = 0;
                int iCanVIPDisc = reqMth.vipIsDiscount;
                /*  if (string.IsNullOrEmpty(reqMth.validType))
                      reqMth.validType = Member_CondTypeName_Track;
                  else if (reqMth.validType.Equals(""))
                      reqMth.validType = Member_CondTypeName_Track;

                  if (reqMth.validType.Equals(Member_CondTypeName_Track))
                      iMemberType = Member_CondType_CDNR;
                  else if (reqMth.validType.Equals(Member_CondTypeName_CardNo))
                      iMemberType = Member_CondType_HYK_NO;
                  else if (reqMth.validType.Equals(Member_CondTypeName_HYID))
                      iMemberType = Member_CondType_HYID;
                  else
                      iMemberType = Member_CondType_CDNR; */

                GetMemberInfo(int.Parse(reqMth.validType), reqMth.ValidID, out vipcard, out msg);

                //  ProcCRM.ProcCRMFunc.GetMemberInfo(iMemberType, reqMth.ValidID, out vipcard, out msg);//iMemberType Member_CondType_HYK_NO


                //2.3取CZK信息
                 if (vipcard.id > 0)
                 {
                     cardCodeToCheck = ""; verifyCode = ""; password = ""; CondValue = Convert.ToString(vipcard.id);
                     GetCashCardInfo(1, CondValue, Shop, cardCodeToCheck, verifyCode, password,
                         out cashCard, out msg);

                     int iPayID = -1;
                     string sPayName = "";
                     GetCashCardPayID(ref iPayID, ref sPayName, out msg);
                     if (iPayID > 0)
                     {
                         cashCard.payID = iPayID;
                     }
                 }

                //2.2:取商品

                //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2> 第二步:查询商品 店:" + shopId);
                //   result = -1;

                bool bRslt = false;
                string ItemCode = "";
                //   ErrorMessage message = new ErrorMessage();

                //  string sTitle1 = "FullCutType", sType = "";
                //   sType = CommonUtils.GetReqStr(sTitle1);
                //   if (string.IsNullOrEmpty(sType))
                //       sType = FullCut_ERP;

                for (i = 0; i < reqMth.goodsList.Count; i++)
                {
                    ItemCode = reqMth.goodsList[i].code;
                    if (string.IsNullOrEmpty(reqMth.goodsList[i].deptCode))
                        sDeptCode = "";
                    else
                        sDeptCode = reqMth.goodsList[i].deptCode;

                        deptid = reqMth.goodsList[i].deptID;

                    //处理负库存标记
                    //  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.1.1> 第二步:查询商品 部门代码:" + sDeptCode +
                    //      " 部门ID:" + deptid);
                    bRslt = DoGetGoodsInfo(ItemCode, deptid, backType, bulkGoodsType, Shop, posNo,
                          out goods);

                    if (!bRslt)
                        throw new Exception("商品"+ ItemCode + "不存在!");

                    //  if (!bRslt)
                    //  {
                    //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.3> 查询数据定义失败：没有定义商品:" + ItemCode);

                    //   result = RsltCode_Wrong_NoDef;
                    //   msg = "计算销售价格失败：没有定义商品:" + ItemCode;
                    //   return result;
                    //  }

                    double fCount = 0;
                    fCount = Convert.ToDouble(reqMth.goodsList[i].count);

                    //2.2.2:付:数量,总价
                    goods.SaleCount = fCount;
                    goods.FrontDiscount = reqMth.goodsList[i].frontendOffAmount;
                    goods.Discount = goods.FrontDiscount + goods.BackDiscount + goods.MemberDiscount
                                     + goods.DecreaseDiscount + goods.ChangeDiscount;

                    //CommonUtils.GetSPDisc(goods);
                    goods.DeptId = deptid;
                    goods.DeptCode = sDeptCode;
                    if ((goods.Price == 0) && (reqMth.goodsList[i].price != 0))
                        goods.Price = reqMth.goodsList[i].price;

                    if ((goods.SaleMoney == 0) && (goods.Price != 0))
                        goods.SaleMoney = RoundMoney(goods.SaleCount * goods.Price);

                    // 负库存销售标记
                    //    CommonUtils.WriteSKTLog(1, posNo,
                    //        "计算销售价格<2.1.1.1> 商品数量:" + goods.SaleCount + " 金额:" + goods.SaleMoney + " 零售:" + goods.Price + " 数量:" + goods.SaleCount +
                    //       " 负库存标记:【" + goods.Fkcxsbj.ToString() + "]");

                    //2.2.3:后台折
                    //   int i_Rslt = 0;
                    //   i_Rslt = DiscountProc.getGoodsBackDiscount(posNo, goods, ref msg);
                    //   if (i_Rslt != 0)
                    //   {
                    //       CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<1.5> 查询后台折扣定义失败:" + msg);
                    //       break;
                    //   }

                    //   CommonUtils.WriteSKTLog(1, posNo,
                    //       " 计算销售价格<2.1.1.2> 下一步准备添加商品 商品数量:" + goods.SaleCount + " 金额:" + goods.SaleMoney +
                    //       " 零售:" + goods.Price + " 数量:" + goods.SaleCount +
                    //      " 负库存标记:【" + goods.Fkcxsbj.ToString() + "]");

                    //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.1.2> 后台折扣 计算完成 准备添加商品");
                    GoodsList.Add(goods);
                }

                /*  bool bResult1 = CheckGoodStock(Shop, posNo, GoodsList, out msg);
                  if (!bResult1)
                  {
                      result = RsltCode_Wrong_NoDef;
                      msg = "商品库存失败:" + msg;

                      CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<4.3.1> " + msg);
                      return result;
                  }

                  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.1.3> 准备VIP折扣计算 打折标记:" + iCanVIPDisc + " VIP的ID:" + vipcard.id + " " +
                      MethodInput.Serialize(GoodsList));  */
                //2.3:计算VIP折扣
                if ((iCanVIPDisc == 1) && (vipcard.id > 0))
                {
                    if (!ComputeVipDiscount(Shop, posNo, vipcard, GoodsList, out msg))
                    {
                        throw new Exception("计算商品售价失败!");
                    }

                    //   ErpProcS80.CheckGoodCanVIPZK(posNo, ref GoodsList, out msg);

                    //如果有VIP折扣。就要处理一下VIP和后台的有关系　
                    //   ErpProcFunc.ProcGoodDisc(posNo, GoodsList, ref msg);
                }

                for (i = 0; i < GoodsList.Count(); i++)
                {
                    GoodsList[i].Discount = GoodsList[i].FrontDiscount + GoodsList[i].BackDiscount
                        + GoodsList[i].MemberDiscount + GoodsList[i].DecreaseDiscount + GoodsList[i].ChangeDiscount;

                    //CommonUtils.GetSPDisc(GoodsList[i]);
                }

                int mTotalYQJE = 0;
                bool bResult = false;
                //  if (sType.Equals(FullCut_ERP))
                //      bResult = CalculateDecDisc_ERP(posNo, mTotalYQJE, ref GoodsList, out msg);
                //2.5: 上传XSJL
                // CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.2.1.1> 处理四舍五入");
                // CommonUtils.ProcChangeDisc(posNo, ref GoodsList);

                //  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.2.1.2> 准备保存");

                Member member = new Member();
                member.MemberId = vipcard.id;
                member.MemberNo = vipcard.memberNo;
                member.MemberType = vipcard.memberType;
                /*  if (!PrepareSaleToDatabase(posNo, shopId, member, GoodsList, out transId, out msg))
                  {
                      CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<4.2.1> 保存销售失败:" + msg);
                      result = RsltCode_Wrong_Calc;
                      msg = "计算商品售价失败:" + msg;
                      return result;
                  } */

                transId = int.Parse(GetLastDealid().ToString()) + 1;

                // CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.3.1> 准备上传商品");
                //3.2:取数据:上传商品

                if (!UploadSp(posNo, Operator, transId, Shop, member, GoodsList, out CrmBillId, out msg))
                {
                    msg = "上传商品失败:" + msg;
                    //   return result;

                    throw new Exception(msg);
                }

                //  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.3.2> 准备计算CRM满减:类型[" + sType + "]");
                //  if (sType.Equals(FullCut_CRM))
                     bResult = CalculateDecDisc_CRM(posNo, mTotalYQJE, CrmBillId, ref GoodsList, out msg);

                //  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.5.1> 查询返券:会员ID:" + vipcard.id);
                if (vipcard.id > 0)
                {
                    string sCountPwd = "NOCHECKPWD";
                    cardCodeToCheck = "";
                    verifyCode = "";
                    password = "";
                    CondValue = Convert.ToString(vipcard.id);
                    if (GetVipCoupon(1, CondValue, Shop, cardCodeToCheck, verifyCode,
                        CrmBillId, out ListCoupon, out iVIPID, out sVIPCode, out msg))

                        if (ListCoupon != null)
                        {
                            if (ListCoupon.Count > 0)
                            {
                                for (i = 0; i < ListCoupon.Count; i++)
                                {
                                    for (j = 0; j < DevicePayments.Count; j++)
                                    {
                                        if (ListCoupon[i].couponId == DevicePayments[j].CouponId)
                                        {
                                            ListCoupon[i].payID = DevicePayments[j].Id.ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    //DoGetPayments(posNo, ref ListCoupon, out msg);
                }
                //  result = 0;


                //3:生成输出商品
                //如果想存一下数据.此处,可以存一下 
                //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.6.1> 生成返回数据");

                if (GoodsList == null)
                    GoodsList = new List<Goods>();
                if (ListCoupon == null)
                    ListCoupon = new List<CouponDetails>();

                //   result = 0;
                UniCalcAccountsPayable(posNo, 0, transId, CrmBillId, "", GoodsList, vipcard, cashCard, ListCoupon, out payableResult);

                //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.3> 计算成功 返回:");

            }
            finally
            {
                st.Stop();
            }

            return payableResult;
        }

        public bool GetCashCardInfo(int condType, string condValue, string storeCode,
            string cardCodeToCheck, string verifyCode, string password,
            out CashCardDetails publicCashCard, out string msg)
        {
            msg = "";
            bool result = false;
            CashCard cashCard = new CashCard();
            publicCashCard = new CashCardDetails();

            if (condValue.Equals(""))
                return result;

            try
            {
                //   result = client.GetCashCard(crmSoapHeader, condType, condValue, cardCodeToCheck, verifyCode, password, storeCode,
                //       out msg, out cashCard);
                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = "CRMUSER";
                crmSoapHeader.Password = "CRMUSER";
                GetCashCardRequest req = new GetCashCardRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.condType = condType;
                req.condValue = condValue;
                req.cardCodeToCheck = cardCodeToCheck;
                req.verifyCode = verifyCode;
                req.password = password;
                req.storeCode = storeCode;

                GetCashCardResponse resp = PosAPI.GetCashCard(req);

                result = resp.GetCashCardResult;
                msg = resp.msg;
                cashCard = resp.cashCard;

              //  CommonUtils.WriteSKTLog(1, "000", "[GetCashCardInfo]" + "<1.9.5.2> CZK准备取卡号 完成");

                if (cashCard != null)
                    AssignLocalToPublic_CashCard(cashCard, out publicCashCard);
            }
            catch (Exception e)
            {
                result = false;
                msg = "查询储值失败:" + msg + " " + e.Message;
            }

            return result;
        }

        private static bool AssignLocalToPublic_CashCard(CashCard sour, out CashCardDetails desc)
        {
            desc.cardId = sour.CardId;
            desc.cardNo = sour.CardCode;
            desc.cardTypeId = sour.CardTypeId;
            desc.amount = sour.Balance;
            desc.useMoney = 0;
            desc.payID = -1;
            return true;
        }


        private bool GetCashCardPayID(ref int iPayID, ref string sPayName, out string msg)
        {
            msg = "";
            iPayID = -1;
            sPayName = "";
            bool result = false;

           // string sDBType = CommonUtils.GetDBType();

            try
            {
              //  DbConnection conn = DBConnManager.GetDBConnection("ERPDB");
                    string sql = "select PAYID,NAME from PAY where TYPE=2";

                //  sql.Append(" select C.HYKTYPE,C.PAYID,S.NAME from PAYMENT_CASHCARD C,SKFS S where S.CODE=C.PAYID AND C.HYKTYPE=" );
                //  query.SQL.Text = sql.ToString();

                //    CommonUtils.WriteSKTLog(10, posId, "查询储值卡收款方式<1>语句:" + sql.ToString());

                //    query.Open();

                DataTable dt = DbHelper.ExecuteTable(sql);

                if (dt.IsNotNull())
                    {
                        result = true;
                        iPayID = dt.Rows[0][0].ToString().ToInt();
                        sPayName = dt.Rows[0][1].ToString();
                     //   CommonUtils.WriteSKTLog(1, posId, "查询储值卡收款方式<1.2>查询结果:" +
                     //      "收款方式:" + iPayID + " " + sPayName);
                    }

                  //  query.Close();
                   // CommonUtils.WriteSKTLog(1, posId, "查询收款方式<2.1>处理完成");
            }
            catch (Exception e)
            {
                result = false;
                msg = "查询储值卡收款方式失败 " + e.Message.ToString();
             //   CommonUtils.WriteSKTLog(12, posId, "查询储值卡收款方式<4.2>错误:" + msg);
                return false;
            }

            //CommonUtils.WriteSKTLog(12, posId, "查询收款方式<3.1>处理完成 返回");
            return result;
        }

        public bool DoGetGoodsInfo(string code, int deptid, int backType, int bulkGoodsType, string shopCode, string posId,
           out Goods goods)
        {

            int iPriceAttr = 0; //商品价格属性
            goods = new Goods();
            try
            {
                if (backType == 1)//是选单退货
                {
                    goods.Id = int.Parse(code);
                }
                int status;
                status = 0;

                //  int iHSFS = 0;
                string sql = "select GOODSID,GOODSDM,BARCODE,NAME,KINDID,PRICE,MEMBER_PRICE,STATUS from GOODS ";

                if (backType == 1)//是选单退货 
                {
                    sql += $" where SP_ID={goods.Id}";
                }
                else
                {
                    sql += $" where GOODSDM='{code}' OR BARCODE='{code}'";
                }

                DataTable dt = DbHelper.ExecuteTable(sql);


                if (dt.IsNotNull())
                {
                    goods = new Goods();
                    goods.Id = dt.Rows[0]["GOODSID"].ToString().ToInt();
                    goods.Code = dt.Rows[0]["GOODSDM"].ToString();
                    goods.BarCode = dt.Rows[0]["BARCODE"].ToString();
                    goods.Name = dt.Rows[0]["NAME"].ToString();
                    goods.ClassType = dt.Rows[0]["KINDID"].ToString();
                    goods.Status = dt.Rows[0]["STATUS"].ToString().ToInt();

                    //   goods.Unit = query.FieldByName("UNIT").AsString;
                    //   goods.Logo = query.FieldByName("SB").AsInteger;    //sb
                    //   goods.Packaged = query.FieldByName("PACKED").AsBoolean;
                    //   goods.GoodsType = query.FieldByName("SPTYPE").AsInteger;

                    goods.Price =  dt.Rows[0]["PRICE"].ToString().ToDouble();
                                     //CommonUtils.RoundMoney(query.FieldByName("LSDJ").AsCurrency * 100); //query.FieldByName("LSDJ").AsInteger;
                                     //   goods.MinPrice = CommonUtils.RoundMoney(query.FieldByName("ZDSJ").AsCurrency * 100); // query.FieldByName("ZDSJ").AsInteger;
                    goods.VipPrice = dt.Rows[0]["MEMBER_PRICE"].ToString().ToDouble();

                    //CommonUtils.RoundMoney(query.FieldByName("HYLSDJ").AsCurrency * 100); //query.FieldByName("HYLSDJ").AsInteger;

                    iPriceAttr = 0;

                }
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public int UniCalcAccountsPayable(string posNo, int iCode, int transId, int crmBillId, string sPrompt,
            List<Goods> GoodsList,
             MemberCard vipcard, CashCardDetails cashCard,
            List<CouponDetails> ListCoupon,
            out CalcAccountsPayableResult desc)
        {

            //    CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.1> ");

            int i = 0;
            double mTotalCanUse = 0;
            //1.1：创建变量
            desc = new CalcAccountsPayableResult();

            desc.code = iCode;
            desc.text = sPrompt;


            //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.2> ");

            desc.erpTranID = transId;
            desc.crmTranID = crmBillId;
            desc.GoodsList = new List<UniGoods>();

            //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.3> ");

            UniGoods uniGood;
            if (iCode == 0)
            {
                desc.GoodsList.Clear();

                // CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.5> ");

                for (i = 0; i < GoodsList.Count; i++)
                {
                    uniGood = new UniGoods();

                    //  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.6> ");

                    AssignGoodToUniGood(GoodsList[i], out uniGood);

                    uniGood.discount = GoodsList[i].FrontDiscount + GoodsList[i].BackDiscount + GoodsList[i].MemberDiscount
                        + GoodsList[i].DecreaseDiscount + GoodsList[i].ChangeDiscount;

                    //GetSPDisc(GoodsList[i]);


                    //    CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.7> ");

                    //2016.02.17：输出数据时，折扣计入
                    uniGood.saleMoney = uniGood.saleMoney - uniGood.discount;
                    desc.GoodsList.Add(uniGood);

                    //     CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.8> ");
                }
            }

            //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.9> ");
            desc.MemberInfo = new MemberCard();

            if (vipcard.memberNo != null)
            {
                desc.MemberInfo.id = vipcard.id;
                desc.MemberInfo.memberNo = vipcard.memberNo;
                desc.MemberInfo.memberType = vipcard.memberType;
                desc.MemberInfo.totalCent = vipcard.totalCent.ToString();
                desc.MemberInfo.ticketCent = "";

                desc.MemberInfo.sex = vipcard.sex;
                desc.MemberInfo.validID = vipcard.validID;
                desc.MemberInfo.validity = vipcard.validity;
                desc.MemberInfo.validType = vipcard.validType;
                desc.MemberInfo.mobilePhone = vipcard.mobilePhone;
                desc.MemberInfo.name = vipcard.name;
                desc.MemberInfo.memberTypeName = vipcard.memberTypeName;
                //validType validID mobilePhone sex validity
            }



            //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.10> ");


            //CZK, 优惠券.此处有时间再加一下查询
            desc.CashCard = new CashCardDetails();

            if (cashCard.cardNo != null)
            {
                desc.CashCard.cardId = cashCard.cardId;
                desc.CashCard.cardNo = cashCard.cardNo;
                desc.CashCard.amount = cashCard.amount;
                desc.CashCard.payID = cashCard.payID;
                desc.CashCard.useMoney = 0;
            }
            //cashCard

            desc.CouponList = new List<TPayableCoupon>();
            if ((ListCoupon != null) && (ListCoupon.Count > 0))
            {
                mTotalCanUse = 0;
                TPayableCoupon CouponItem;

                for (i = 0; i < ListCoupon.Count; i++)
                {
                    CouponItem = new TPayableCoupon();
                    mTotalCanUse = mTotalCanUse + ListCoupon[i].amountCanUse;
                }

                if (mTotalCanUse > 0)
                {
                    for (i = 0; i < ListCoupon.Count; i++)
                    {
                        CouponItem = new TPayableCoupon();

                        CouponItem.balance = ListCoupon[i].amount;
                        CouponItem.accountsPayable = ListCoupon[i].amountCanUse;
                        CouponItem.cardId = ListCoupon[i].cardId;
                        CouponItem.couponId = ListCoupon[i].couponId;
                        CouponItem.couponName = ListCoupon[i].couponName;
                        CouponItem.couponType = ListCoupon[i].couponType;
                        CouponItem.payID = ListCoupon[i].payID;
                        CouponItem.payName = ListCoupon[i].payName;
                        CouponItem.validity = ListCoupon[i].valid_date;

                        desc.CouponList.Add(CouponItem);
                    }
                }
            }

            //    CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.11> ");
            return 0;
        }


        private bool AssignGoodToUniGood(Goods Sour, out UniGoods desc)
        {
            bool result = false;

            desc = new UniGoods();
            desc.code = Sour.Code;
            desc.barCode = Sour.BarCode;
            desc.classCode = Sour.ClassType;
            //desc.contractID = 0; //暂时没有
            desc.deptCode = Sour.DeptCode;
            desc.deptId = Sour.DeptId;
            desc.discountBillId = Sour.DiscountBillId;
            desc.refNo_HY = Sour.IRefNo_HY;
            desc.refNo_MJ = Sour.IRefNo_MJ;
            desc.refNo_ZK = Sour.IRefNo_ZK;
            desc.backDiscount = Sour.BackDiscount;
            desc.frontDiscount = Sour.FrontDiscount;
            desc.memberDiscount = Sour.MemberDiscount;
            desc.decreaseDiscount = Sour.DecreaseDiscount;
            desc.changeDiscount = Sour.ChangeDiscount;

            desc.discount = Sour.Discount;
            desc.id = Sour.Id;
            desc.name = Sour.Name;
            desc.price = Sour.Price;
            desc.saleCount = Sour.SaleCount;

            desc.saleMoney = Sour.SaleMoney;
            //desc.Specification = "";
            desc.goodsType = Sour.GoodsType;
            desc.remarks = "";

            result = true;
            return result;
        }


        public bool GetVipCoupon(int iCondType, string sCondValue, string sStoreCode,
            string sCheck, string sVerify, int iServerID,
            out List<CouponDetails> ListCoupon, out int iVIPID, out string sVIPCode, out string msg)
        {
            int i = 0, j = 0;
            bool result = false;
            msg = "";

            iVIPID = -1;
            sVIPCode = "";

            ListCoupon = new List<CouponDetails>();
            Coupon[] PayCoupon;
            CouponPayLimit[] payLimits;

            if (sCondValue.Equals(""))
                return result;

            PayCoupon = new Coupon[100];
            payLimits = new CouponPayLimit[100];

            ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
            crmSoapHeader.UserId = "CRMUSER";
            crmSoapHeader.Password = "CRMUSER";

            try
            {

                GetVipCouponToPayRequest req = new GetVipCouponToPayRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.condType = iCondType;
                req.condValue = sCondValue;
                req.cardCodeToCheck = sCheck;
                req.verifyCode = sVerify;
                req.storeCode = sStoreCode;
                req.serverBillId = iServerID;

                GetVipCouponToPayResponse rep = PosAPI.GetVipCouponToPay(req);


                result = rep.GetVipCouponToPayResult;
                msg = rep.msg;

                if (!result)
                    return result;

                PayCoupon = rep.coupons;
                payLimits = rep.payLimits;

                iVIPID = rep.vipId;
                sVIPCode = rep.vipCode;

                ListCoupon.Clear();

                CouponDetails CouponItem;
                Coupon CurCoupon;
                CouponPayLimit CurLimit;

                //2016.12.13：原来:顺序按:券的顺序  修改后:顺序按:可用顺序
                //第一级循环:payLimits, 第二级:PayCoupon 
                for (i = 0; i < payLimits.Length; i++)
                {
                    CurLimit = payLimits[i];
                    if (CurLimit.CouponType >= 0)
                    {
                        for (j = 0; j < PayCoupon.Length; j++)
                        {
                            CurCoupon = PayCoupon[j];
                            if (CurCoupon.CouponType == CurLimit.CouponType)
                            {
                                CouponItem = new CouponDetails();
                                CouponItem.cardId = iVIPID;
                                CouponItem.cardNo = sVIPCode;
                                CouponItem.couponId = CurCoupon.CouponType;
                                CouponItem.couponName = CurCoupon.CouponTypeName;
                                CouponItem.couponType = CurCoupon.CouponType;
                                CouponItem.amount = CurCoupon.Balance;
                                CouponItem.amountCanUse = CurLimit.LimitMoney;

                                CouponItem.returnMoney = 0;
                                CouponItem.valid_date = "";
                                CouponItem.payID = "";

                                ListCoupon.Add(CouponItem);
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                msg = "查询优惠券失败:" + msg + " " + e.Message;
            }

            return result;
        }

        public static double RoundMoney(double money)
        {
            return Math.Round(money, 2);
        }

        public bool ComputeVipDiscount(string shopCode, string posNo, MemberCard vipcard, List<Goods> GoodsList, out string msg)
        {
            msg = "";

            string ProjectName = "";
            //    ProjectName = CommonUtils.GetReqStr(ErpProc.SetConfig_ProjectName);

            //    CommonUtils.WriteSKTLog(10, posNo, "计算会员折扣<1.1> 商品数:" + GoodsList.Count() + " 会员ID:" + vipcard.id + " 项目:" + ProjectName);


            //    string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";
            //    CRMUSer = CommonUtils.GetReqStr("CRMUser");
            //    CRMPwd = CommonUtils.GetReqStr("CRMPwd");

            ArticleVipDisc[] articleVipDisc;
            DeptArticleCode[] deptArticleCode = new DeptArticleCode[GoodsList.Count];

            for (int i = 0; i <= GoodsList.Count - 1; i++)
            {
                deptArticleCode[i] = new DeptArticleCode();
                deptArticleCode[i].ArticleCode = GoodsList[i].Code;
                deptArticleCode[i].DeptCode = GoodsList[i].DeptCode;
                //    CommonUtils.WriteSKTLog(11, posNo, "计算会员折扣<1.2> 商品[" + GoodsList[i].Code + "] 传入数据:" + deptArticleCode[i].DeptCode);
            }

            ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
            crmSoapHeader.UserId = "CRMUSER";
            crmSoapHeader.Password = "CRMUSER";

            try
            {
                //  ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                //  crmSoapHeader.UserId = CRMUSer;// "AAA";
                //  crmSoapHeader.Password = CRMPwd; // "123";
                //  PosWebServiceSoapClient client = client = new PosWebServiceSoapClient();

                bool GetVipDiscResult = false;

                //   CommonUtils.WriteSKTLog(11, posNo, "计算会员折扣<2.1> 准备发送折扣请求");

                GetArticleVipDiscRequest req = new GetArticleVipDiscRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.vipId = vipcard.id;
                req.vipType = vipcard.memberType;
                req.storeCode = shopCode;
                req.articles = deptArticleCode;


                GetArticleVipDiscResponse rep = PosAPI.GetArticleVipDisc(req);

                GetVipDiscResult = rep.GetArticleVipDiscResult;

                //  GetVipDiscResult = client.GetArticleVipDisc(crmSoapHeader, vipcard.id, vipcard.memberType, shopCode, deptArticleCode,
                //      out msg, out articleVipDisc);

                if (GetVipDiscResult)
                {
                    articleVipDisc = rep.discs;
                    for (int i = 0; i <= GoodsList.Count - 1; i++)
                    {
                        double disc = 0;
                        double mTotalSPZK = GoodsList[i].FrontDiscount + GoodsList[i].BackDiscount;
                        disc = RoundMoney(((GoodsList[i].Price * GoodsList[i].SaleCount) - mTotalSPZK) * (1 - articleVipDisc[i].DiscRate));
                        GoodsList[i].MemberDiscount = disc;
                        GoodsList[i].MemberOffRate = articleVipDisc[i].DiscRate; //付会员折扣
                                                                                 //

                        //   CommonUtils.WriteSKTLog(11, posNo, "计算会员折扣<2.3> [2018.01.20] " +
                        //       "会员折扣=[( 零售 X 数量 ) -  前台折 - 后台折 ] X (1-折扣率)   " +
                        //     disc + " = [( " + GoodsList[i].Price + " X " + GoodsList[i].SaleCount + ") - " +
                        //     GoodsList[i].FrontDiscount + " - " + GoodsList[i].BackDiscount + ") X ( 1 -  " + articleVipDisc[i].DiscRate + "))"
                        //       );

                        double mDisc1 = GoodsList[i].MemberDiscount, mDisc2 = GoodsList[i].MemberDiscount;


                        //    CommonUtils.ProcVIPDisc(posNo, mDisc1, ref mDisc2);
                        GoodsList[i].MemberDiscount = mDisc2;
                    }
                }
                else
                {
                    //  CommonUtils.WriteSKTLog(12, posNo, "计算会员折扣<4.1> 计算失败");
                    return false;
                }
            }
            catch
            {
                msg = "连接CRM服务器失败，取整单商品的会员折扣失败";
                //   CommonUtils.WriteSKTLog(12, posNo, "计算会员折扣<4.2> 计算失败" + msg);
                return false;
            }
            return true;
        }

        public bool UploadSp(string posNo, string sOperator, int transId, string StoreCode, Member member, List<Goods> GoodsList,
            out int CrmBillId, out string msg)
        {
            CrmBillId = 0;
            msg = "";

            //  CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.1>");

            try
            {
                // CanReturnCoupon=false|
                //   string sTitle1 = "", sTitle2 = "", sTitle3 = "", sTitle4 = "", sTitle5 = "";
                //   string sValue1 = "", sValue2 = "", sValue3 = "", sValue4 = "", sValue5 = "";

                //   sTitle1 = "CanReturnCoupon";
                //   string sGet = ConfigurationManager.AppSettings["SetString"];
                //   CommonUtils.GetReqStr(sGet, sTitle1, sTitle2, sTitle3, sTitle4, sTitle5, ref sValue1, ref sValue2, ref sValue3, ref sValue4, ref sValue5);
                //   string canReturnCoupon = sValue1;

                //   string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";
                //   CRMUSer = CommonUtils.GetReqStr("CRMUser");
                //   CRMPwd = CommonUtils.GetReqStr("CRMPwd");

                //   ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                //   crmSoapHeader.UserId = CRMUSer; // "AAA";
                //   crmSoapHeader.Password = CRMPwd; // "123";
                //   PosWebServiceSoapClient client = new PosWebServiceSoapClient();

                RSaleBillHead billHead = new RSaleBillHead();

                //billHead.BillId = 0;// transId;
                billHead.BillId = transId;// transId;
                billHead.BillType = 0;

                billHead.Cashier = sOperator;
                billHead.PosId = posNo;
                billHead.SaleTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                billHead.AccountDate = DateTime.Now.ToString("yyyy-MM-dd");
                billHead.StoreCode = StoreCode;
                billHead.VipId = member.MemberId;

                //  CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.2>" + " 处理商品数据");

                List<RSaleBillArticle> articleList = new List<RSaleBillArticle>();
                for (int i = 0; i <= GoodsList.Count - 1; i++)
                {
                    if (GoodsList[i].Id > 0)
                    {
                        RSaleBillArticle article = new RSaleBillArticle();
                        GoodsList[i].Discount = GoodsList[i].FrontDiscount + GoodsList[i].BackDiscount
                            + GoodsList[i].MemberDiscount + GoodsList[i].DecreaseDiscount
                            + GoodsList[i].ChangeDiscount;

                        //CommonUtils.GetSPDisc(GoodsList[i]);
                        article.ArticleCode = GoodsList[i].Code;

                        article.DeptCode =  GoodsList[i].DeptCode;

                        article.DiscMoney = GoodsList[i].Discount;
                        article.Inx = i;
                        article.IsNoCent = false;
                        article.IsNoProm = false;
                        article.SaleMoney = GoodsList[i].SaleMoney - GoodsList[i].Discount;
                        article.SaleNum = (double)GoodsList[i].SaleCount;
                        article.DiscMoney = GoodsList[i].Discount;

                        //   if (canReturnCoupon.Equals("false"))
                        //       article.IsNoProm = true;


                        //    CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.3>" + " 商品数据:" +
                        //        " Code[" + article.ArticleCode + "] " +
                        //        " DeptCode[" + article.DeptCode + "] " +
                        //        " DiscMoney[" + article.DiscMoney + "] " +
                        //        " SaleMoney[" + article.SaleMoney + "] " +
                        //        " SaleNum[" + article.SaleNum + "] " +
                        //        " DiscMoney[" + article.DiscMoney + "] ");

                        articleList.Add(article);
                    }
                }

                RSaleBillArticle[] articles = new RSaleBillArticle[articleList.Count];
                articleList.CopyTo(articles);

                // int crmBillId;
                double decMoney;
                RSaleBillArticleDecMoney[] articleDecMoneys;
                RSaleBillArticlePromFlag[] articlePromFlags;

                bool saveResult = false;


                //  CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.5>" + " 开始调用上传接口:");

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = "CRMUSER";
                crmSoapHeader.Password = "CRMUSER";
                SaveRSaleBillArticlesRequest req = new SaveRSaleBillArticlesRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.billHead = billHead;
                req.billArticles = articles;

                SaveRSaleBillArticlesResponse rep = PosAPI.SaveRSaleBillArticles(req);

                saveResult = rep.SaveRSaleBillArticlesResult;

                // saveResult = client.SaveRSaleBillArticles(crmSoapHeader, billHead, articles, out msg, out crmBillId,
                //     out decMoney, out articleDecMoneys, out articlePromFlags);

                //  CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.6>" + " 完成调用上传接口:");

                if (saveResult)
                {
                    CrmBillId = rep.serverBillId;
                    decMoney = rep.decMoney;
                    articleDecMoneys = rep.articleDecMoneys;
                    articlePromFlags = rep.articlePromFlags;
                    return true;
                }
                else
                {
                    msg = rep.msg;
                    return false;

                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();

                //    CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.4.1>" + " 错误:" + msg);

                return false;
            }
        }

        public GetCardPayableResult GetCardPayable(ReqGetCardPayable reqMth)
        {
            string msg = "";
            GetCardPayableResult payableResult = new GetCardPayableResult();

            MemberCard vipcard = new MemberCard();
            CashCardDetails cashCard = new CashCardDetails();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();
            List<Payment> DevicePayments = new List<Payment>();

            string Device = employee.PlatformId;
            string shop = reqMth.storeCode ;

            ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
            crmSoapHeader.UserId = "CRMUSER";
            crmSoapHeader.Password = "CRMUSER";

            //  int i = 0, iDataType = UniCode_Json;

            //1.2:Json取数据
            //  ReqGetCardPayable reqMth = new ReqGetCardPayable();

            //  string userCode = "", cardCodeToCheck = "", verifyCode = "", password = "", CondValue = "", sVIPCode = "";
            //  int iPayID = -1, transId = 0, iVIPID = -1;
            //  string sPayName = "", sTitle = "计算卡券付款";
            string password = "";
            int transId = 0;

            /*   if(iDataType == UniCode_Json)
            {
               if( !sInput.TryToObj<ReqGetCardPayable>(out reqMth))
                {
                    throw new Exception("参数解析失败 ");
                }
            }  */

            if (string.IsNullOrEmpty(reqMth.validID))
                reqMth.validID = "";
            if (string.IsNullOrEmpty(reqMth.couponPassword))
                reqMth.couponPassword = "";

            Stopwatch st = new Stopwatch();

            try
            {
                st.Start();

                DoGetPayments(Device, out DevicePayments, out msg);

                //取会员      
                /*  int iMemberType = 0;
                  if (string.IsNullOrEmpty(reqMth.validType))
                      reqMth.validType = Member_CondTypeName_Track;
                  else if (reqMth.validType.Equals(""))
                      reqMth.validType = Member_CondTypeName_Track;

                  if (reqMth.validType.Equals(Member_CondTypeName_Track))
                      iMemberType = Member_CondType_CDNR;
                  else if (reqMth.validType.Equals(Member_CondTypeName_CardNo))
                      iMemberType = Member_CondType_HYK_NO;
                  else if (reqMth.validType.Equals(Member_CondTypeName_HYID))
                      iMemberType = Member_CondType_HYID;
                  else if (reqMth.validType.Equals(Member_CondTypeName_qrcode))
                      iMemberType = Member_CondType_qrcode;

                  else if (reqMth.validType.Equals(Member_CondTypeName_Phone))
                      iMemberType = Member_CondType_SJHM;

                  else
                      iMemberType = Member_CondType_CDNR; */

                password = reqMth.password;

                string conValue = reqMth.validID;
                string ValidIDType = reqMth.validType;

                int condType = 3;
                if (conValue.ToString().Length == 11)
                    condType = 3;
                else
                    condType = 2;

                // string cardCodeToCheck = "", verifyCode = "";


                if (!GetMemberInfo(condType, conValue, out vipcard, out msg))
                {
                    throw new Exception(msg);
                }


                //计算券
                if (vipcard.id > 0)
                {

                    /*    GetVipCouponToPayRequest couponRequest = new GetVipCouponToPayRequest();

                        couponRequest.condType = 2;
                        couponRequest.condValue = Convert.ToString(vipcard.memberNo);
                        couponRequest.cardCodeToCheck = "";
                        couponRequest.verifyCode = "";
                        couponRequest.storeCode = "001";
                        couponRequest.serverBillID = reqMth.crmTranID; */


                    int iVIPID;
                    string sVIPCode;



                    //        ProcCRM.ProcCRMFunc.GetVipCoupon(1, CondValue, Shop, cardCodeToCheck, verifyCode,
                    //        reqMth.couponPassword, reqMth.crmTranID,
                    //       out ListCoupon, out iVIPID, out sVIPCode, out msg);

                    if (GetVipCoupon(condType, conValue, shop,
                                     "", "", reqMth.crmTranID,
                                     out ListCoupon, out iVIPID, out sVIPCode, out msg))
                        DoGetCouponPayments(DevicePayments, ref ListCoupon, ref msg);
                    else
                    {
                        throw new Exception(msg);
                    }
                }
                else
                {
                    GetVipCouponToPayRequest couponRequest = new GetVipCouponToPayRequest();
                    couponRequest.ABCSoapHeader = crmSoapHeader;
                    couponRequest.condType = 1;
                    couponRequest.condValue = Convert.ToString(vipcard.id);
                    couponRequest.cardCodeToCheck = "";
                    couponRequest.verifyCode = "";
                    couponRequest.serverBillId = reqMth.crmTranID;

                    GetVipCouponToPayResponse couponRes = PosAPI.GetVipCouponToPay(couponRequest);

                    if (couponRes.GetVipCouponToPayResult)
                        DoGetCouponPayments(DevicePayments, ref ListCoupon, ref msg);
                    else
                    {
                        throw new Exception(couponRes.msg);
                    }

                }


                if (ListCoupon == null)
                    ListCoupon = new List<CouponDetails>();

                UniGetCardPayable(0, transId, reqMth.crmTranID, "", vipcard, cashCard, ListCoupon, out payableResult);
            }
            finally
            {
                st.Stop();
            }

            return payableResult;

        }

        private static bool AssignLocalToPublic_Member(VipCard sour, out MemberCard desc)
        {
            bool result = false;
            desc = new MemberCard();
            desc.totalCent = sour.ValidCent.ToString();
            desc.id = sour.CardId;
            desc.memberNo = sour.CardCode;
            desc.memberType = sour.CardTypeId;
            desc.ticketCent = "";

            desc.name = sour.VipName;
            desc.mobilePhone = sour.Mobile;
          //  desc.sex = "";
            desc.validType = "";
            desc.validID = ""; 
            desc.typeLevel = 0;


            desc.sex = "";
             if (sour.SexType == 1)
                 desc.sex = "Female";
             else if (sour.SexType == 0)
                 desc.sex = "Male"; 
            desc.memberTypeName = sour.CardTypeName;

            result = true;
            return result;
        }


        public GetMemberCardDetailsResult GetMemberInfo(ReqMemberCard reqMC)
        {
            string msg="";
            string Device = employee.PlatformId;
            string Shop = reqMC.storeCode;
            GetMemberCardDetailsResult desc = new GetMemberCardDetailsResult();

          //  int iDataType = UniCode_Json;
            MemberCard vipcard = new MemberCard();
            CashCardDetails cashCard = new CashCardDetails();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();
            

            int result = -1;
            result = DoUniGetMemberInfo(Device, Shop, reqMC, out vipcard, out cashCard, out ListCoupon, out msg);

         //   CommonUtils.WriteSKTLog(1, Device, "取会员信息<5.1.1> 准备汇总结果 ");

            if (ListCoupon == null)
                ListCoupon = new List<CouponDetails>();

            GetMemberDetails(Device, result, msg, vipcard, cashCard, ListCoupon, out desc);

          //  if (iDataType == UniCode_Json)
          //      sOut = MethodInput.Serialize(desc);
          //  else
          //      sOut = MethodInput.XmlSerialize2(desc);
         //   CommonUtils.WriteSKTLog(1, Device, "取会员信息:输出:" + sOut);

            return desc;
        }

        public int DoUniGetMemberInfo(string Device, string Shop, ReqMemberCard reqMth,
            out MemberCard vipcard, out CashCardDetails cashCard, out List<CouponDetails> ListCoupon, out string msg)
        {
            msg = "";
          //  string input = "功能:取会员信息<DoUniGetMemberInfo>: 设备号码:" + Device + "  店:" + Shop + "  输入:" + sInput;
          //  CommonUtils.WriteSKTLog(0, Device, input);

            string phoneCode = "", cardCode = "", cardCodeToCheck = "", verifyCode = "",
                password = "", CondValue = "", ValidIDType = "", ValidID = "", sTitle = "功能:取会员信息";
            // MobilePhone = "", MemberCard = "",
            //  string ProjectName = "", subProjectName = "";
            //  ProjectName = CommonUtils.GetReqStr(ErpProc.SetConfig_ProjectName);
            //  subProjectName = CommonUtils.GetReqStr(ErpProc.SetConfig_SubProjectName);

            //  int iDataType = UniCode_Json;
            int result = -1, i = 0, j = 0;
            List<Payment> DevicePayments = new List<Payment>();

            vipcard = new MemberCard();
            cashCard = new CashCardDetails();
            ListCoupon = new List<CouponDetails>();

            //1.1:检查基本输入数据

            if (Shop.Equals(""))
            {
             //   result = RsltCode_Wrong_Para;
                msg = "数据检查失败：<取会员信息> 店号为空";
                return result;
            }


            if (Device.Equals(""))
            {
                msg = "数据检查失败：<取会员信息> 设备号为空";
             //   result = RsltCode_Wrong_Para;
                return result;
            }

         //   input = "功能:取会员信息<1.1>: 准备转换输入数据:" + input;
         //   CommonUtils.WriteSKTLog(1, Device, "取会员信息:" + input);

            //1.2:Json分解输入
           // ReqMemberCard reqMth = new ReqMemberCard();

            try
            {

               // if (reqMth.memberNo == null)
                //    reqMth.memberNo = "";
               // if (reqMth.mobilePhone == null)
               //     reqMth.mobilePhone = "";
                if (reqMth.validType == null)
                    reqMth.validType = "";
                if (reqMth.validID == null)
                    reqMth.validID = "";
              //  if (reqMth.couponPassword == null)
              //      reqMth.couponPassword = "";

             //   MobilePhone = reqMth.mobilePhone;
             //   MemberCard = reqMth.memberNo;
                ValidID = reqMth.validID;
                ValidIDType = reqMth.validType;

             //   if (MobilePhone.Equals("") && MemberCard.Equals("") && ValidID.Equals(""))
             //   {
             //       msg = "数据检查失败：<取会员信息2> 输入为空";
                  //  result = RsltCode_Wrong_Para;
             //       return result;
            //    }

            }
            catch (Exception e)
            {
                msg = "数据检查失败：传入参数失败:" + e.Message;
             //   result = RsltCode_Wrong_Para;
                return result;
            }


       //     input = "功能:取会员信息<1.2>: 输入数据:" + input + " 各项:设备号:" + Device + " 电话:" + phoneCode + " 卡号:" + cardCode;
       //     CommonUtils.WriteSKTLog(1, Device, "取会员信息:" + input);


            Stopwatch st = new Stopwatch();

            try
            {
                st.Start();

                //2.1: 取收款台 
                //CommonUtils.WriteInputLogBySKT(1, posNo, "计算销售价格<1>", "第一步:取收款台");

                //result = DoCheckStation(posNo, sMac, out msg);

                //if (result != 0)
                //{
                //     CommonUtils.WriteInputLogBySKT(1, posNo, "取会员信息<1.3>", "查询数据定义失败：没有定义这个设备号:" + posNo);
                //    return UniMakeStrMemberInfo(iDataType, RsltCode_Wrong_Para, "取会员信息失败：没有定义这个设备号:" + posNo, "", transId, vipcard, cashCard, ListCoupon);//02
                // }


              //  CommonUtils.WriteSKTLog(1, Device, sTitle + "<2.2.1> 取:收款台的收款方式:" + Device);
                result = DoGetPayments(Device, out DevicePayments, out msg);
                if (result != 0)
                {
                  //  CommonUtils.WriteSKTLog(1, Device, sTitle + "<4.2.1> 取收款方式失败:" + msg);
                  //  result = RsltCode_Wrong_Calc;
                    msg = sTitle + "失败:" + msg;
                    return result;
                }

                //   string sCrmVer = ProcCRMFunc.GetCrmVer();
                //   CommonUtils.WriteSKTLog(1, Device, "[2018.07.05]取会员信息<2.1> 准备取会员定义 CRM的类型[ " + sCrmVer + "] ");

                //2.2:如果有会员.则取会员2017.06.16        
                //0：磁道内容，1: 会员卡ID ,2：卡号,3:手机号，4：身份证号                 
                int iMemberType = ValidIDType.ToInt();

                GetMemberInfo(iMemberType, ValidID, out vipcard, out msg);

                /*  iMemberType = Member_CondType_CDNR;
                  if (ValidIDType.Equals(Member_CondTypeName_SFZ))
                      iMemberType = Member_CondType_SFZBH;
                  else if (ValidIDType.Equals(Member_CondTypeName_HZ))
                      iMemberType = Member_CondType_SFZBH;
                  else if (ValidIDType.Equals(Member_CondTypeName_CardNo))
                      iMemberType = Member_CondType_HYK_NO;
                  else if (ValidIDType.Equals(Member_CondTypeName_HYID))
                      iMemberType = Member_CondType_HYID;
                  else if (ValidIDType.Equals(Member_CondTypeName_Track))
                      iMemberType = Member_CondType_CDNR;
                  else if (ValidIDType.Equals(Member_CondTypeName_Phone))
                      iMemberType = Member_CondType_SJHM;
                  else if (ValidIDType.Equals(Member_CondTypeName_qrcode))
                      iMemberType = Member_CondType_qrcode; */

                //    CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.1> [2017.06.16_现2018.07.09] 按证件号3 取定义< " + ValidID +
                //        " 输入类型:" + ValidIDType + " 确定类型:" + iMemberType + " Sub项目:" + subProjectName +
                //        " CRM的类型[ " + sCrmVer + "] ");


                /*  if (ErpProc.SubProjectName_HNDT.Equals(subProjectName))
                  {
                      if (iMemberType == Member_CondType_qrcode)
                      {
                          CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.2.2> 2017.10.02 按电子会员 解码前< " + ValidID +
                            " 输入类型:" + ValidIDType + " 确定类型:" + iMemberType);


                          ValidID = CommonUtils.QRCodeDesString(Device, ValidID);

                          CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.2.3> 2017.10.02 按电子会员 解码后<" + ValidID +
                            "> 输入类型:" + ValidIDType + " 确定类型:" + iMemberType);
                          if (string.IsNullOrEmpty(ValidID))
                              ValidID = "";
                          if ("".Equals(ValidID))
                          {
                              CommonUtils.WriteSKTLog(1, Device, sTitle + "<4.2.2> 电子会员号解析错误:" + msg);
                              result = RsltCode_Wrong_Calc;
                              msg = sTitle + "失败:" + msg;
                              return result;
                          }
                          iMemberType = Member_CondType_HYK_NO;
                          CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.2.5> 2017.10.02 按电子会员 解码后<" + ValidID +
                            "> 输入类型:" + ValidIDType + " 转变类型为CDNR 确定类型:" + iMemberType);
                      }
                  }

                  if (sCrmVer == ProcCRMFunc.CRMVER_PC_HSMZ.ToUpper())
                  {
                      CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.2.1> 2018.07.05 如果类型是5,则按0处理  定义< " + iMemberType + ">");
                      if (iMemberType == Member_CondType_qrcode)
                      {
                          iMemberType = Member_CondType_CDNR;
                          CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.2.2> 2018.07.05 处理后定义< " + iMemberType + ">");
                      }
                  } */

                //   if (iMemberType == 3)  //手机号 
                //       GetMemberInfo(ValidID, out vipcard, out msg);
                //   else
                //       GetMemberInfo(iMemberType, ValidID, out vipcard, out msg);
                //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.2> 取定义完成 提示:" + msg);
                //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<2.5.3> 取到的信息: " + MethodInput.Serialize(vipcard));
                /*if (vipcard.id <= 0)
                {                    
                    msg = "取会员信息失败" + msg;
                    result = RsltCode_Wrong_NoMember;
                    return result;
                }*/


                string sCouponCode = "";
                int iServerID = 0, iCouponVIPID = 0;
                bool bGetCash = false, bGetCoupon = false, bRight = false;
                //2.3取CZK信息
                if (vipcard.id > 0)
                {
                    cardCodeToCheck = ""; verifyCode = ""; password = ""; CondValue = Convert.ToString(vipcard.id);
                  //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<3.2.1.1> 有会员号码_处理储值卡 ");
                    bGetCash = GetCashCardInfo(1, CondValue, Shop, cardCodeToCheck, verifyCode, password, out cashCard, out msg);

                   // CommonUtils.WriteSKTLog(1, Device, sTitle + "<3.2> 完成取储值卡 ID:" + cashCard.cardId + " No:" + cashCard.cardNo + " 计数:" + DevicePayments.Count);
                    int iPayID = -1;
                    string sPayName = "";
                    GetCashCardPayID(ref iPayID, ref sPayName, out msg);
                    if (iPayID > 0)
                    {
                        cashCard.payID = iPayID;
                    }

                  //  string sCounpPwd = "";
                  //  sCounpPwd = reqMth.couponPassword;
                   // CommonUtils.WriteSKTLog(1, Device, "取会员信息<3.2.1.2> 有会员号码_处理优惠券 券密码:" + sCounpPwd);
                    bGetCoupon = GetVipCouponShow(Device, 1, CondValue, Shop, cardCodeToCheck, verifyCode,// sCounpPwd,
                        iServerID, out ListCoupon, out iCouponVIPID, out sCouponCode, out msg);

                    if (bGetCoupon)
                        DoGetCouponPayments(DevicePayments, ref ListCoupon, ref msg);

                    msg = "读取会员信息成功";
                  //  result = RsltCode_Success;

                  //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<3.3.1.3> 有会员号码_准备返回 " + msg);

                    return result;
                }
                else
                {

                    iMemberType = 0;  //磁道内容
                    cardCodeToCheck = ""; verifyCode = ""; password = ""; CondValue = ValidID;
                  //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<3.2.2.1> 无会员ID_处理储值卡 ");
                    bGetCash = GetCashCardInfo(iMemberType, CondValue, Shop, cardCodeToCheck, verifyCode, password,
                        out cashCard, out msg);

                  //  CommonUtils.WriteSKTLog(1, Device, sTitle + "<3.2> 完成取储值卡 ID:" + cashCard.cardId + " No:" + cashCard.cardNo + " 计数:" + DevicePayments.Count);
                    //收CZK所用的收款方式ID  
                    if ((cashCard.cardId > 0) && (!cashCard.cardNo.Equals("")))
                    {
                        for (i = 0; i < DevicePayments.Count; i++)
                        {
                         //   CommonUtils.WriteSKTLog(1, Device, sTitle + "<3.2> Type:" + DevicePayments[i].TypeCode + " 设置:" + SKFSTYPECODE_MONEYCARD);

                            if (DevicePayments[i].TypeCode == "2")
                            {
                                cashCard.payID = DevicePayments[i].Id;
                                break;
                            }
                        }
                    }

                   // string sCounpPwd = "";
                   // sCounpPwd = reqMth.couponPassword;
                 //   CommonUtils.WriteSKTLog(1, Device, "取会员信息<3.2.2.2> 无会员ID_处理优惠券  券密码:" + sCounpPwd);
                    bGetCoupon = GetVipCouponShow(Device, iMemberType, CondValue, Shop, cardCodeToCheck, verifyCode,
                         iServerID, out ListCoupon, out iCouponVIPID, out sCouponCode, out msg);

                    if (bGetCoupon)
                        DoGetCouponPayments(DevicePayments, ref ListCoupon, ref msg);

                    if ((bGetCash) || (bGetCoupon))
                        bRight = true;
                    if (bRight)
                    {
                        msg = "读取会员信息成功";
                        return result;
                    }
                    else
                    {
                        msg = "读取会员信息失败";
                        return result;
                    }

                }
            }
            finally
            {
                st.Stop();
            }

          //  return result;
        }


        public int GetMemberDetails(string Device, int iCode, string sPrompt,
            MemberCard sourVipcard,
            CashCardDetails sourCashCard, List<CouponDetails> sourCoupons,
            out GetMemberCardDetailsResult desc)
        {
            int i = 0;

         //   CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.1> 汇总结果 ");

            //1.1：创建变量
            desc = new GetMemberCardDetailsResult();

            VipCard vipcard = new VipCard();
            CashCard cashCard = new CashCard();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();

        //    CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.2> 汇总结果 ");

            desc.code = iCode;
            desc.text = sPrompt;
            desc.MemberInfo = new MemberCard();
            desc.CashCard = new CashCardDetails();
            desc.CouponList = new List<CouponDetails>();

         //   CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.3> 汇总结果 ");

            if (iCode != 0)
                return -1;


          //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.4> 汇总会员: " + MethodInput.Serialize(sourVipcard));
            //2.1:会员相关
            IniMemberCard(out desc.MemberInfo);
            if (sourVipcard.memberNo != null)
            {
                desc.MemberInfo.id = sourVipcard.id;
                desc.MemberInfo.memberNo = sourVipcard.memberNo;
                desc.MemberInfo.memberType = sourVipcard.memberType;
                desc.MemberInfo.totalCent = sourVipcard.totalCent.ToString();
                desc.MemberInfo.ticketCent = "";

             //   CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.5> 汇总结果 ");

                desc.MemberInfo.name = sourVipcard.name;
                desc.MemberInfo.mobilePhone = sourVipcard.mobilePhone;
                desc.MemberInfo.sex = sourVipcard.sex;
                desc.MemberInfo.validType = sourVipcard.validType;
                desc.MemberInfo.validID = sourVipcard.validID;
                desc.MemberInfo.validity = sourVipcard.validity;
                desc.MemberInfo.memberTypeName = sourVipcard.memberTypeName;
            }

            if (sourCashCard.cardNo != null)
            {
              //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.6> 汇总储值: " + MethodInput.Serialize(sourCashCard));
                //2.2储值相关
                desc.CashCard.cardId = sourCashCard.cardId;
                desc.CashCard.cardNo = sourCashCard.cardNo;
                desc.CashCard.amount = sourCashCard.amount;
                desc.CashCard.useMoney = 0;
                desc.CashCard.payID = sourCashCard.payID;
            }

          //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.7> 汇总优惠券: " + MethodInput.Serialize(sourCoupons));
            //2.3:券相关
            if ((sourCoupons != null) && (sourCoupons.Count > 0))
            {
              //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.8> 汇总结果 ");
                CouponDetails Coupon;
                for (i = 0; i < sourCoupons.Count; i++)
                {
                    bool bHave = false;
             //       CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.9> 汇总结果 ");
                    Coupon = new CouponDetails();

                    bHave = true;
                    if (string.IsNullOrEmpty(sourCoupons[i].payID))
                        bHave = false;
                    else if (sourCoupons[i].payID.Equals(""))
                        bHave = false;
                    if (!bHave)
                        continue;


                    AssignCouponDetails(sourCoupons[i], out Coupon);
                    desc.CouponList.Add(Coupon);
                }
            }

          //  CommonUtils.WriteSKTLog(1, Device, "取会员信息<6.10> 汇总结果 ");
            return 0;
        }

        public static int IniMemberCard(out MemberCard sour)
        {
            sour = new MemberCard();
            sour.id = 0;
            sour.memberNo = "";
            sour.memberType = 0;
            sour.memberTypeName = "";
            sour.mobilePhone = "";
            sour.name = "";
            sour.sex = "";
            sour.ticketCent = "";
            sour.totalCent = "";
            sour.validID = "";
            sour.validity = "";
            sour.validType = "";
            return 0;
        }

        private static bool AssignCouponDetails(CouponDetails Sour, out CouponDetails desc)
        {
            desc = new CouponDetails();
            desc.amount = Sour.amount;
            desc.amountCanUse = Sour.amountCanUse;
            desc.cardId = Sour.cardId;
            desc.cardNo = Sour.cardNo;
            desc.couponId = Sour.couponId;
            desc.couponName = Sour.couponName;
            desc.couponType = Sour.couponType;
            desc.payID = Sour.payID;
            desc.payName = Sour.payName;
            desc.returnMoney = Sour.returnMoney;
            desc.useMoney = Sour.useMoney;
            desc.valid_date = Sour.valid_date;
            return true;
        }

        public bool GetVipCouponShow(string Device, int iCondType, string sCondValue, string sStoreCode,
            string sCheck, string sVerify, int iServerID,
            out List<CouponDetails> publicListCoupon, out int iVIPID, out string sVIPCode, out string msg)
        {
            int i = 0;
            bool result = false;
            bool bNeedDate = true;
            msg = "";

            iVIPID = -1;
            sVIPCode = "";


         //   CommonUtils.WriteSKTLog(1, Device, "查询优惠券<1.1> [GetVipCouponShow_UniWS] : CondType:" + iCondType + " StoreCode:" + sStoreCode +
         //      "  Check " + sCheck.ToString() + " Verify:" + sVerify + " CondValue:" + sCondValue
         //       );

            string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";

            publicListCoupon = new List<CouponDetails>();


            Coupon[] PayCoupon;

            if (sCondValue.Equals(""))
                return result;

            PayCoupon = new Coupon[100];

            try
            {
                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = CRMUSer; 
                crmSoapHeader.Password = CRMPwd;

                //   CommonUtils.WriteSKTLog(1, Device, "查询优惠券<1.2> " + " 准备调用[CRM_GetVipCoupon]");

                //    PosWebServiceSoapClient client = client = new PosWebServiceSoapClient();

                //      result = GetVipCoupon(crmSoapHeader, iCondType, sCondValue, sCheck, sVerify, sStoreCode,
                //         bNeedDate, out msg, out iVIPID, out sVIPCode, out PayCoupon);

                GetVipCouponRequest req = new GetVipCouponRequest();

                req.ABCSoapHeader = crmSoapHeader;
                req.condType = iCondType;
                req.condValue = sCondValue;
                req.storeCode = sStoreCode;
                req.cardCodeToCheck = sCheck;
                req.verifyCode = sVerify;
                req.requireValidDate = bNeedDate;


                 GetVipCouponResponse rep = PosAPI.GetVipCoupon(req);

                result = rep.GetVipCouponResult;


                if (!result) return result;

                msg = rep.msg;
                iVIPID = rep.vipId;
                sVIPCode = rep.vipCode;
                PayCoupon = rep.coupons;

                publicListCoupon.Clear();
                CouponDetails CouponItem;
                Coupon CurCoupon;

                for (i = 0; i < PayCoupon.Length; i++)
                {
                    CurCoupon = PayCoupon[i];
                    if ((CurCoupon.CouponType >= 0) && (!CurCoupon.CouponTypeName.Equals("")))
                    {
                        CouponItem = new CouponDetails();
                        CouponItem.cardId = iVIPID;
                        CouponItem.cardNo = sVIPCode;
                        CouponItem.couponId = CurCoupon.CouponType;
                        CouponItem.couponName = CurCoupon.CouponTypeName;
                        CouponItem.couponType = CurCoupon.CouponType;
                        CouponItem.amount = CurCoupon.Balance;
                        CouponItem.amountCanUse = CouponItem.amount;
                        //CouponItem.valid_date = CurCoupon.
                        CouponItem.returnMoney = 0;
                        CouponItem.payID = "";
                        publicListCoupon.Add(CouponItem);
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                msg = "查询优惠券失败:" + msg + " " + e.Message;
            }

            return result;
        }




        public bool GetMemberInfo(int iMemberType, string MemberCardID, out MemberCard memberCard, out string msg)
        {
            bool result = false;
            memberCard = new MemberCard();
            VipCard vipCard = new VipCard();

            if (MemberCardID.Length == 11)
                iMemberType = 3;

            ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
            crmSoapHeader.UserId = "CRMUSER";
            crmSoapHeader.Password = "CRMUSER";
            GetVipCardRequest request = new GetVipCardRequest();

            request.ABCSoapHeader = crmSoapHeader;
            request.condType = iMemberType;
            request.condValue = MemberCardID;

            try
            {
                GetVipCardResponse res = PosAPI.GetVipCard(request);
                vipCard = res.vipCard;

                if (vipCard != null)
                    AssignLocalToPublic_Member(vipCard, out memberCard);

                msg = res.msg;
                result = res.GetVipCardResult;
            }
            catch (Exception e)
            {
                result = false;
                msg = "调用GetVipCard接口失败:" + e.Message;
            }

            return result;

        }

        public static int DoGetCouponPayments(List<Payment> DevicePayments, ref List<CouponDetails> ListCoupon, ref string msg)
        {
            int result = -1;
            int i = 0, j = 0;

            if (ListCoupon == null)
                return -1;
            if (ListCoupon.Count <= 0)
                return -1;

            if (DevicePayments == null)
                return -1;
            if (DevicePayments.Count <= 0)
                return -1;

            for (i = 0; i < ListCoupon.Count; i++)
            {
                for (j = 0; j < DevicePayments.Count; j++)
                {
                    if (ListCoupon[i].couponId == DevicePayments[j].CouponId)
                    {
                        ListCoupon[i].payID = DevicePayments[j].Id.ToString();
                        ListCoupon[i].payName = DevicePayments[j].Name;
                        break;
                    }
                }
            }

            return result;
        }

        public int UniGetCardPayable(int iCode, int transId, int crmBillId, string sPrompt,
           MemberCard vipcard, CashCardDetails cashCard,
           List<CouponDetails> ListCoupon,
           out GetCardPayableResult desc)
        {

            int i = 0;
            double mTotalCanUse = 0;
            //1.1：创建变量
            desc = new GetCardPayableResult();

            desc.code = iCode;
            desc.text = sPrompt;

            desc.erpTranID = transId;
            desc.crmBillId = crmBillId;


            desc.MemberInfo = new MemberCard();

            if (vipcard.memberNo != null)
            {
                desc.MemberInfo.id = vipcard.id;
                desc.MemberInfo.memberNo = vipcard.memberNo;
                desc.MemberInfo.memberType = vipcard.memberType;
                desc.MemberInfo.totalCent = vipcard.totalCent.ToString();
                desc.MemberInfo.ticketCent = "";

                desc.MemberInfo.sex = vipcard.sex;
                desc.MemberInfo.validID = vipcard.validID;
                desc.MemberInfo.validity = vipcard.validity;
                desc.MemberInfo.validType = vipcard.validType;
                desc.MemberInfo.mobilePhone = vipcard.mobilePhone;
                desc.MemberInfo.name = vipcard.name;
                desc.MemberInfo.memberTypeName = vipcard.memberTypeName;
            }

            //CZK, 优惠券.此处有时间再加一下查询
            desc.CashCard = new CashCardDetails();

            if (cashCard.cardNo != null)
            {
                desc.CashCard.cardId = cashCard.cardId;
                desc.CashCard.cardNo = cashCard.cardNo;
                desc.CashCard.amount = cashCard.amount;
                desc.CashCard.payID = cashCard.payID;
                desc.CashCard.useMoney = 0;
            }
            //cashCard

            desc.CouponList = new List<TPayableCoupon>();
            if ((ListCoupon != null) && (ListCoupon.Count > 0))
            {
                mTotalCanUse = 0;
                TPayableCoupon CouponItem;

                for (i = 0; i < ListCoupon.Count; i++)
                {
                    CouponItem = new TPayableCoupon();
                    mTotalCanUse = mTotalCanUse + ListCoupon[i].amountCanUse;
                }

                if (mTotalCanUse > 0)
                {
                    for (i = 0; i < ListCoupon.Count; i++)
                    {
                        CouponItem = new TPayableCoupon();

                        //2017.09.29:SHIJS:原因:有时间CRM返的余额小，可用大，要处理一下
                        CouponItem.balance = ListCoupon[i].amount;

                        if (ListCoupon[i].amountCanUse > ListCoupon[i].amount)
                            CouponItem.accountsPayable = ListCoupon[i].amount;
                        else
                            CouponItem.accountsPayable = ListCoupon[i].amountCanUse;

                        CouponItem.cardId = ListCoupon[i].cardId;
                        CouponItem.couponId = ListCoupon[i].couponId;
                        CouponItem.couponName = ListCoupon[i].couponName;
                        CouponItem.couponType = ListCoupon[i].couponType;
                        CouponItem.payID = ListCoupon[i].payID;
                        CouponItem.payName = ListCoupon[i].payName;
                        CouponItem.validity = ListCoupon[i].valid_date;

                        desc.CouponList.Add(CouponItem);
                    }
                }
            }
            return 0;
        }


        public ConfirmDealResult ConfirmDeal(ReqConfirmDeal ReqConfirm)
        {
            string Shop = ReqConfirm.storeCode;   
            string Device = employee.PlatformId;
            string Operator = employee.Code;
            string msg = "";
            int result = -1, i = 0, j = 0, CrmBillId = 0, CrmMoneyCardTransID = 0, iHyId = 0;
            string PromniDealID = "", ErpTranID = "", MemberCardID = "",
                 posNo = "", userCode = "";

            ErrorMessage errorMessage;
            ConfirmDealResult confirmResult = new ConfirmDealResult();

            bool bValue = false;
            string  ddJLBH = "";

            int transId = 0, iPerson = 0;
            double mTotalMoney = 0;
            double fCent = 0;
            Member member = new Member();
            Person CurPerson = new Person();

            Goods goods = new Goods();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();

            List<Payment> sktPayments = new List<Payment>();
            TDealMemberCard MemberInfo = new TDealMemberCard();
            List<TDealReturnCoupon> ReturnCouponList = new List<TDealReturnCoupon>();
            List<TDealSaleMoneyLeft> CanReturnCouponList = new List<TDealSaleMoneyLeft>();

            bool bCheckMember = false;
            bool bCheckCrmTran = true;  //是否检查CRM交易
           // bCheckMember = true;

            posNo = Device;
            userCode = Operator;

            //1.1:检查基本输入数据
            string input = "";


            //1:判断输入数据
            if (Shop.Equals(""))
            {
                result = -1;
                msg = "数据检查失败：计算销售价格,门店代码为空";
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }

            if (Device.Equals(""))
            {
                result = -1;
                msg = "数据检查失败：计算销售价格,设备号为空";
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }


            if (ReqConfirm == null)
            {
                result = -1;
                msg = "数据检查失败：传入数据为空";
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }

            //2.2:判断基础数据
            try
            {
                ErpTranID = ReqConfirm.erpTranID;
                CrmBillId = Convert.ToInt32(ReqConfirm.crmTranID);
                PromniDealID = ReqConfirm.outOrder;
                MemberCardID = ReqConfirm.validID;

                ddJLBH = ReqConfirm.DDJLBH;


                if (string.IsNullOrEmpty(ErpTranID))
                    ErpTranID = "";
                if (string.IsNullOrEmpty(PromniDealID))
                    PromniDealID = "";
                if (string.IsNullOrEmpty(MemberCardID))
                    MemberCardID = "";
                if (string.IsNullOrEmpty(ddJLBH))
                    ddJLBH = "";

                if (ErpTranID.Equals(""))
                {
                    result = -1;
                    msg = "数据检查失败：<保存销售>ERP交易号为空";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                if (bCheckCrmTran)
                {
                    if (CrmBillId <= 0)
                    {
                        result = -1;
                        msg = "数据检查失败：<保存销售>CRM交易号"+ CrmBillId.ToString();
                        confirmResult.code = result;
                        confirmResult.text = msg;
                        throw new Exception(msg);

                    }
                }

                if (bCheckMember && (MemberCardID.Equals("")))
                {
                    result = -1;
                    msg = "数据检查失败：<保存销售>保存销售,会员为空";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                if (ReqConfirm.goodsList == null)
                {
                    result = -1;
                    msg = "数据检查失败: <保存销售> 商品数据为空";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                if (ReqConfirm.goodsList.Count() <= 0)
                {
                    result = -1;
                    msg = "数据检查失败: <保存销售> 商品计数为空";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                if (ReqConfirm.paysList == null)
                {
                    result = -1;
                    msg = "数据检查失败: <保存销售> 付款数据为空";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                if (ReqConfirm.paysList.Count() <= 0)
                {
                    result = -1;
                    msg = "数据检查失败: <保存销售> 付款计数为空";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }
                if (ReqConfirm.creditDetailList == null)
                {
                    ReqConfirm.creditDetailList = new List<CreditDetail>();
                }
            }
            catch (Exception e)
            {
                result = -1;
                msg = "< 保存销售 > 检查请求数据失败" + e.Message;
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }


            //2.3:转换基础数据.判断数据库中的数据
            try
            {
                transId = 0;
                transId = Convert.ToInt32(ErpTranID);
                if (transId <= 0)
                {
                    result = -1;
                    msg = "数据检查失败：记录号小于或者等于0[" + transId + "-" + ErpTranID + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                //JsonDoGetMaxId(Device, 0, out iRemoteTranID, out msg);
                long iRemoteTranID = GetLastDealid()+1;

                if (iRemoteTranID <= 0)
                {
                    result = -1;
                    msg = "数据检查失败：取记录号失败：记录号小于或者等于0[" + iRemoteTranID + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);

                }
                if (transId < iRemoteTranID)
                {
                    result = -1;
                    msg = "数据检查失败：记录号错误[" + transId + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }


                DoGetPayments(posNo, out sktPayments, out msg);
                if ((sktPayments == null))
                {
                    result = -1;
                    msg = "数据检查失败:本收款台没有定义收款方式:[ 收款台:" + posNo + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                } 

                if ((sktPayments != null) && (sktPayments.Count() < 0))
                {
                    result = -1;
                    msg = "数据检查失败:本收款台没有定义收款方式:[ 收款台:" + posNo + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                } 

                if (!CheckSaveData(posNo, sktPayments, ReqConfirm, ref msg))
                {
                    result = -1;
                    msg = "< 保存销售 > 查询数据一致失败:[ " + msg + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                result = -1;

                //  result = DoGetPersonInfo(posNo, 0, Operator, "", WorkType_NoSet, out CurPerson, out msg);

                CurPerson.PersonId = employee.Id.ToInt();
                CurPerson.PersonCode = Operator;
                CurPerson.PersonName = employee.Name;

                result = 0;
                if (result != 0)
                {
                    result = -1;
                    msg = "查询数据定义失败：没有营业员:" + Operator;
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                iPerson = CurPerson.PersonId;
            }
            catch (Exception e)
            {
                result = -1;
                msg = "取数据失败" + e.Message;
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }

            //2.5:将输入数据转换为内容数据
            List<Goods> GoodList = new List<Goods>();
            List<Payment> PayList = new List<Payment>();
            List<CashCardDetails> CashCardList = new List<CashCardDetails>();
            List<CouponDetails> CouponList = new List<CouponDetails>();

            try
            {
                mTotalMoney = 0;
                for (i = 0; i < ReqConfirm.goodsList.Count(); i++)
                {
                    Goods GoodItem = new Goods();
                    GoodItem.SubTicktInx = ReqConfirm.goodsList[i].tickInx;
                    GoodItem.SubGoodsInx = ReqConfirm.goodsList[i].inx;
                    GoodItem.PersonId = ReqConfirm.goodsList[i].assistantId;
                    GoodItem.Id = ReqConfirm.goodsList[i].id;
                    GoodItem.Code = ReqConfirm.goodsList[i].code;
                    GoodItem.DeptCode = ReqConfirm.goodsList[i].deptCode;
                    GoodItem.DeptId = ReqConfirm.goodsList[i].deptID;
                    GoodItem.BackDiscount = ReqConfirm.goodsList[i].backendOffAmount;
                    GoodItem.DiscountBillId = ReqConfirm.goodsList[i].backendOffID;
                    GoodItem.IRefNo_MJ = ReqConfirm.goodsList[i].fullCutOffID;
                    GoodItem.VipDiscBillId = ReqConfirm.goodsList[i].memberOffID;
                    GoodItem.MemberDiscount = ReqConfirm.goodsList[i].memberOff;
                    GoodItem.FrontDiscount = ReqConfirm.goodsList[i].frontendOffAmount;
                    GoodItem.DiscoaddDiscount = ReqConfirm.goodsList[i].fullCutOffAmount;
                  //  GoodItem.MemberDiscount = ReqConfirm.goodsList[i].memberOff;
                    GoodItem.ChangeDiscount = ReqConfirm.goodsList[i].changeDiscount;
                    GoodItem.Discount = ReqConfirm.goodsList[i].totalOffAmount;
                    GoodItem.Name = ReqConfirm.goodsList[i].name;
                    GoodItem.Price = ReqConfirm.goodsList[i].price;
                    GoodItem.SaleCount = ReqConfirm.goodsList[i].count;
                    GoodItem.ShopId = GetGoodsShopId(ReqConfirm.goodsList[i].id);
                    GoodItem.SaleMoney = ReqConfirm.goodsList[i].accountsPayable;//+ ReqConfirm.goodsList[i].totalOffAmount;

                    /* if (ProjectName_TJ_JYB.Equals(ProjectName))
                     {
                         //2018.04.14:处理方式,TJJYB,判断商品的HSFS
                         bool bRslt = false;
                         ErrorMessage message = new ErrorMessage();
                         int deptid = 0, backType = 0, bulkGoodsType = 0;
                         string sDeptCode = "", ItemCode = "";
                         if (string.IsNullOrEmpty(ReqConfirm.goodsList[i].deptCode))
                             sDeptCode = "";
                         else
                             sDeptCode = ReqConfirm.goodsList[i].deptCode;

                         ItemCode = ReqConfirm.goodsList[i].code;
                         if (ReqConfirm.goodsList[i].deptID == null)
                             deptid = 0;
                         else
                             deptid = ReqConfirm.goodsList[i].deptID;


                         CommonUtils.WriteSKTLog(1, posNo, "保存销售<1.6.2.2>如果项目是TJJYB 需要重新取一下商品的HSFS:CODE=" + ItemCode);

                         Goods GoodtTemp = new Goods();
                         bRslt = DoGetGoodsInfo(ItemCode, sDeptCode, deptid, backType, bulkGoodsType, Shop, Device,
                              out GoodtTemp, out message);
                         GoodItem.IHsfs = GoodtTemp.IHsfs;//ddJLBH

                         GoodItem.OTherStr1 = ddJLBH;
                         CommonUtils.WriteSKTLog(1, posNo,
                             " 保存销售<1.6.2.3>[TJJYB]_[2018.08.28] 取到的商品的HSFS:" + GoodItem.IHsfs +
                             " 订单记录号[" + GoodItem.OTherStr1 + "]");
                     } */

                    GoodList.Add(GoodItem);

                    mTotalMoney = mTotalMoney + ReqConfirm.goodsList[i].accountsPayable;
                }

                //付款方式
                for (i = 0; i < ReqConfirm.paysList.Count(); i++)
                {
                    Payment PayItem = new Payment();
                    PayItem.Id = ReqConfirm.paysList[i].Id;
                    PayItem.PayedMoney = ReqConfirm.paysList[i].PayMoney;

                    for (j = 0; j < sktPayments.Count; j++)
                    {
                        if (sktPayments[j].Id == PayItem.Id)
                        {
                            PayItem.PaymentType = sktPayments[j].PaymentType;
                            PayItem.CouponId = sktPayments[j].CouponId;
                            PayItem.Yhjid = sktPayments[j].Yhjid;

                            break;
                        }
                    }
                    PayList.Add(PayItem);
                }


                //优惠券
                if ((ReqConfirm.couponsList != null) && (ReqConfirm.couponsList.Count() > 0))
                {
                    for (i = 0; i < ReqConfirm.couponsList.Count(); i++)
                    {
                        CouponDetails CouponItem = new CouponDetails();
                        CouponItem.amount = ReqConfirm.couponsList[i].Balance;
                        CouponItem.amountCanUse = ReqConfirm.couponsList[i].AccountsPayable;
                        CouponItem.cardId = ReqConfirm.couponsList[i].CardId;
                        CouponItem.cardNo = "";
                        CouponItem.couponId = ReqConfirm.couponsList[i].CouponId;
                        CouponItem.couponName = ReqConfirm.couponsList[i].CouponName;
                        CouponItem.couponType = ReqConfirm.couponsList[i].CouponType;
                        CouponItem.payID = ReqConfirm.couponsList[i].PayID;
                        CouponItem.useMoney = ReqConfirm.couponsList[i].OutOfPocketAmount;
                        CouponList.Add(CouponItem);
                    }
                }

                //储值卡
                if ((ReqConfirm.cashCashList != null) && (ReqConfirm.cashCashList.Count() > 0))
                {
                    for (i = 0; i < ReqConfirm.cashCashList.Count(); i++)
                    {
                        CashCardDetails CashItem = new CashCardDetails();
                        CashItem.amount = ReqConfirm.cashCashList[i].amount;
                        CashItem.cardId = ReqConfirm.cashCashList[i].cardId;
                        CashItem.cardNo = ReqConfirm.cashCashList[i].cardNo;
                        CashItem.useMoney = ReqConfirm.cashCashList[i].useMoney;
                        CashCardList.Add(CashItem);
                    }
                }
            }
            catch (Exception e)
            {
                result = -1;
                msg = "<保存销售>传入数据有错误,失败:" + e.Message;
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }


            //3.1:取VIP相关信息
            input = "设备号:" + Device + " 用户代码:" + userCode + " 会员号:" + MemberCardID;

            iHyId = 0;
            MemberCard vipcard = new MemberCard();


            try
            {
                   member.MemberId = -1;
                   member.MemberNo = "";
                   member.MemberType = -1;


                /*  int iMemberType = 0;
                  if (string.IsNullOrEmpty(ReqConfirm.validType))
                      ReqConfirm.validType = Member_CondTypeName_Track;
                  else if (ReqConfirm.validType.Equals(""))
                      ReqConfirm.validType = Member_CondTypeName_Track;

                  if (ReqConfirm.validType.Equals(Member_CondTypeName_Track))
                      iMemberType = Member_CondType_CDNR;
                  else if (ReqConfirm.validType.Equals(Member_CondTypeName_CardNo))
                      iMemberType = Member_CondType_HYK_NO;
                  else if (ReqConfirm.validType.Equals(Member_CondTypeName_HYID))
                      iMemberType = Member_CondType_HYID;
                  else
                      iMemberType = Member_CondType_CDNR; */

                // iMemberType = Member_CondType_CDNR; //2016.06.22:强设设置为:CDNR

                //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<1.7.2> 开始查询会员:" + MemberCardID);

                //if ((bCheckMember)  && (!MemberCardID.Equals("") ))

                int iMemberType = int.Parse(ReqConfirm.validType);
                MemberCardID = ReqConfirm.validID;

                if (!MemberCardID.Equals(""))
                {

                    if (GetMemberInfo(iMemberType, MemberCardID, out vipcard, out msg))

                    {
                        member.MemberId = vipcard.id;
                        member.MemberNo = vipcard.memberNo;
                        member.MemberType = vipcard.memberType;
                        iHyId = member.MemberId;

                        member.TypeLevel = vipcard.typeLevel;
                        if (vipcard.openId.IsEmpty())
                            member.WxOpenId = "";
                        else
                            member.WxOpenId = vipcard.openId;
                    }
                    else
                    {

                        throw new Exception(msg);
                    }

                    if (vipcard.id <= 0)
                    {
                        throw new Exception("检查数据失败：会员数据不存在");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("检查数据失败：" + msg);
            }


          //  string jdMsg = "";
            //3.2:开始保存
            Stopwatch st = new Stopwatch();
            try
            {
                st.Start();
                //3.1:取数据:收款台
                result = 0;
                if (member.MemberId > 0)
                {
                    int iVIPID = 0;
                    //  string sCountPwd = "NOCHECKPWD";
                    string cardCodeToCheck = "", verifyCode = "", sVIPCode = "", CondValue = Convert.ToString(member.MemberId);

                    //ProcCRM.ProcCRMFunc.GetVipCoupon(1, CondValue, Shop, cardCodeToCheck, verifyCode, sCountPwd, CrmBillId,
                    //out ListCoupon, out iVIPID, out sVIPCode, out msg);

                    GetVipCoupon(1, CondValue, Shop, cardCodeToCheck, verifyCode, CrmBillId,
                        out ListCoupon, out iVIPID, out sVIPCode, out msg);
                }

                // 分摊纸券
                //    DevicePaperVoucher(posNo, PayList, ref GoodList, ref msg);

                CrmMoneyCardTransID = 0;
                int CrmCouponTransId = 0;

                if (!BeforeSaveTransation(Shop, posNo, transId, CrmBillId, CashCardList, CouponList, PayList,
                    ref GoodList, out CrmMoneyCardTransID,
                    out CrmCouponTransId, out fCent, out msg))
                {
                    result = -1;
                    msg = "保存销售:失败：" + msg;
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                bValue = false;

                //保存erp销售记录

                 SaleRequest saleReq = new SaleRequest();

                 //sale
                 saleReq.posno = posNo;
                 saleReq.dealid = transId;

                if (PromniDealID == "") //入参outorder 记录第三方CRM卡号 猫酷会员卡号
                    saleReq.member_cardid = member.MemberId.ToString();
                else
                    saleReq.member_cardid = PromniDealID;

                 saleReq.crm_recordid = CrmBillId;
                 saleReq.cashierid = iPerson;

                 double totalMoney=0;

                 for (int t = 0; t < GoodList.Count; t++)
                 {
                     totalMoney = totalMoney + (GoodList[t].SaleMoney - GoodList[t].Discount);
                 }

                 saleReq.sale_amount = decimal.Parse(totalMoney.ToString());

                 saleReq.sale_time = DateTime.Now;
                 saleReq.account_date = DateTime.Now.Date;

                 //sale_goods
                 List<GoodsResult> goodsLst = new List<GoodsResult>();

                 for (int g = 0; g <= GoodList.Count - 1; g++)
                 {
                    GoodsResult goodsOne = new GoodsResult();
                    goodsOne.sheetid = 0;
                    goodsOne.inx = g;
                    goodsOne.goodsid = GoodList[g].Id;
                    goodsOne.goodscode = GoodList[g].Code;
                    goodsOne.price = decimal.Parse(GoodList[g].Price.ToString());
                    goodsOne.quantity = float.Parse(GoodList[g].SaleCount.ToString());
                    goodsOne.sale_amount = decimal.Parse((GoodList[g].SaleMoney).ToString()); //-GoodList[g].Discount
                //wangkx  扣折暂时记为0
                    goodsOne.discount_amount = 0; //decimal.Parse(GoodList[g].Discount.ToString());
                    goodsOne.coupon_amount = decimal.Parse((GoodList[g].PreferentialMoney + GoodList[g].DecreasePreferential).ToString());
                    goodsOne.shopid = GoodList[g].ShopId;
                    goodsLst.Add(goodsOne);
                 }

                 saleReq.goodslist = goodsLst;

                 //sale_pay
                 List<PayResult> payLst = new List<PayResult>();

                 for (int p = 0; p <= PayList.Count - 1; p++)
                 {
                    PayResult payOne = new PayResult();
                    payOne.payid = PayList[p].Id;
                    payOne.amount = decimal.Parse(PayList[p].PayedMoney.ToString());
                    payLst.Add(payOne);
                 }

                 saleReq.paylist = payLst;

                 //sale_clerk
                 List<ClerkResult> clerkLst = new List<ClerkResult>();
                 ClerkResult clerkOne = new ClerkResult();

                 clerkOne.sheetid = 0;
                 clerkOne.clerkid = iPerson;
                 clerkLst.Add(clerkOne);

                 saleReq.clerklist = clerkLst;

                //PayRecord

                if (!ReqConfirm.creditDetailList.IsEmpty())
                {
                    List<PayRecord> payRcd = new List<PayRecord>();

                    for (int p = 0; p <= ReqConfirm.creditDetailList.Count - 1; p++)
                    {
                        PayRecord payRcdOne = new PayRecord();
                        payRcdOne.inx = ReqConfirm.creditDetailList[p].inx;
                        payRcdOne.payid = ReqConfirm.creditDetailList[p].payid;
                        payRcdOne.cardno = ReqConfirm.creditDetailList[p].cardno;
                        payRcdOne.bank = ReqConfirm.creditDetailList[p].bank;
                        payRcdOne.bankid = ReqConfirm.creditDetailList[p].bankid;
                        payRcdOne.amount = ReqConfirm.creditDetailList[p].amount;
                        payRcdOne.serialno = ReqConfirm.creditDetailList[p].serialno;
                        payRcdOne.refno = ReqConfirm.creditDetailList[p].refno;
                        payRcdOne.opertime = ReqConfirm.creditDetailList[p].opertime;
                        payRcd.Add(payRcdOne);
                    }

                    saleReq.payRecord = payRcd;
                }
                else
                {
                    saleReq.payRecord = null;
                }


                errorMessage = new ErrorMessage();
                 try
                 {
                     Sale(saleReq);
                     DeleteCrmTrans(posNo, CrmMoneyCardTransID, CrmCouponTransId);
                     bValue = true;
                 }
                 catch (Exception e)
                 {
                     bValue = false;
                     errorMessage.ErrorType = 1;
                     errorMessage.Message = "保存erp销售数据出错:" + e;
                 }


                  if (!bValue)
                     {
                         result = -1;
                         string error = "";
                         if (errorMessage.Message.Equals(""))
                             msg = ErrorMessageToString(errorMessage);
                         else
                             msg = errorMessage.Message;

                         confirmResult.code = result;
                         confirmResult.text = msg;  // + jdMsg;

                         if (CrmMoneyCardTransID > 0)
                         {
                            //保存销售失败:储值卡冲正
                            CancelMoneyCard(posNo, out error);
                         } 
                         if (CrmCouponTransId > 0)
                         {
                            //  保存销售失败:优惠券冲正
                            CancelCoupon(posNo, out error);
                         }
                     }
                     else
                     {  
                         ReturnCouponList.Clear();
                         CanReturnCouponList.Clear();
                        if (!CheckOut(posNo, CrmBillId, out ReturnCouponList, out CanReturnCouponList, out msg))
                        {
                            result = -1;
                            //提交CRM失败
                        }

                    } 

                //5:返回数据
                if (result == 0)
                {
                    MemberCard tempMemberCard = new MemberCard();

                    //if ((bCheckMember) && (vipcard.name != null))
                    if (vipcard.name != null)
                    {
                        //  CommonUtils.WriteSKTLog(1, posNo, "保存销售<3.2.1>计算总积分: 累加前:" + vipcard.totalCent.ToString());

                        string TotalCent = "";
                        double fValue1 = 0, fValue2 = 0;
                        try
                        {
                            fValue1 = Convert.ToDouble(vipcard.totalCent);
                            fValue2 = fValue1 + fCent;
                            TotalCent = fValue2.ToString();

                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售<3.2.2>计算总积分: " +
                            //        fValue2.ToString() + " = " + fCent.ToString() + " + " + fValue1.ToString());

                        }
                        catch (Exception e)
                        {
                            TotalCent = vipcard.totalCent;

                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售<3.2.3>计算总积分:处理错误[按处理之前] " +
                            //      vipcard.totalCent.ToString() + "  错误:" + e.Message);
                        }

                        MemberInfo.id = vipcard.id;
                        MemberInfo.name = vipcard.name;
                        MemberInfo.memberNo = vipcard.memberNo;
                        MemberInfo.memberType = member.MemberType.ToString();
                        MemberInfo.mobilePhone = tempMemberCard.mobilePhone;
                        MemberInfo.sex = tempMemberCard.sex;
                        MemberInfo.validID = tempMemberCard.validID;
                        MemberInfo.validType = tempMemberCard.validType;
                        MemberInfo.ticketCent = fCent.ToString();
                        MemberInfo.totalCent = TotalCent.ToString(); //vipcard.totalCent.ToString();
                    }



                    string sSaleTime;

                    try
                    {
                        sSaleTime = DbHelper.ExecuteTable($"select to_char(sale_time,'yyyy-mm-dd HH24:MI:SS') from sale where posno='{posNo}' and dealid={transId}").Rows[0][0].ToString();
                    }
                    catch (Exception)
                    {

                        sSaleTime = "";
                    }

                    UniConfirmDealResult(posNo, result, transId, CrmBillId, sSaleTime, //sSaleTime原为"",成功时记本次交易时间
                      MemberInfo, GoodList, ReturnCouponList, CanReturnCouponList, out confirmResult);

                   // confirmResult.text = confirmResult.text + jdMsg;

                    //SendMsgTranOk

                    // CommonUtils.WriteSKTLog(1, posNo, "保存销售<3.3.1> 发送信息 OpenId:" + member.WxOpenId + " " + confirmResult.text);
                    //openid
                    /*  if (!CommonUtils.isEmpty(member.WxOpenId))
                      {
                          string sMoney = MoneyToDouble(mTotalMoney).ToString();
                          //SendMsgOrderSucess  SendMsgTranOk
                          CommonUtils.SendMsgOrderSucess(posNo, member.WxOpenId, "", "", sMoney.ToString(), member.ValidCent.ToString(), "");
                      } */

                    /*sOut = UniMakeStrSaveSale(Device, iDataType, RsltCode_Success, "保存销售成功:单号:" + transId, "", transId, CrmBillId, GoodsList,
                        MemberInfo, ReturnCouponList);*/
                }
                else
                {
                    result = -1;
                    msg = "保存销售:失败：" + msg;
                    confirmResult.code = result;
                    confirmResult.text = msg; //+ jdMsg;
                    throw new Exception(msg);
                }


                /*
                else
                    sOut = UniMakeStrSaveSale(Device, iDataType, RsltCode_Wrong_Proc, "保存销售:失败：" + msg, "", transId, CrmBillId, GoodsList,
                        MemberInfo, ReturnCouponList);*/

                //  CommonUtils.WriteSKTLog(1, posNo, "保存销售<3.6>返回信息:<" + sOut + ">");

            }
            finally
            {
                st.Stop();
            }

            return confirmResult;
        }


        public int UniConfirmDealResult(string sDevice, int iCode, int ErpTranID, int CrmTranID, string sPrompt,
            TDealMemberCard MemberInfo,
            List<Goods> GoodsList,
            List<TDealReturnCoupon> ReturnCouponList,
            List<TDealSaleMoneyLeft> CanReturnCouponList,
            out ConfirmDealResult desc)
        {
            int i = 0, result = -1;
            desc = new ConfirmDealResult();
            desc.code = iCode;
            desc.text = sPrompt;
            desc.erpTranID = ErpTranID;
            desc.crmTranID = CrmTranID;

            if (iCode == 0)
            {
                desc.MemberInfo = new TDealMemberCard();
                desc.GoodsList = new List<TTranGoods>();
                desc.ReturnCouponList = new List<TDealReturnCoupon>();
                desc.ReturningCouponList = new List<TDealSaleMoneyLeft>();

                AssignDealMemberCard(MemberInfo, ref desc.MemberInfo);

                TTranGoods TranGood = new TTranGoods();
                for (i = 0; i < GoodsList.Count; i++)
                {
                    TranGood = new TTranGoods();
                    AssignTranGoods(GoodsList[i], ref TranGood);
                    desc.GoodsList.Add(TranGood);
                }

                for (i = 0; i < ReturnCouponList.Count; i++)
                {
                    TDealReturnCoupon ReturnCoupon = new TDealReturnCoupon();
                    AssignReturnCoupon(ReturnCouponList[i], ref ReturnCoupon);
                    desc.ReturnCouponList.Add(ReturnCoupon);
                }

                for (i = 0; i < CanReturnCouponList.Count; i++)
                {
                    TDealSaleMoneyLeft item = new TDealSaleMoneyLeft();
                    AssignCanReturnCoupon(CanReturnCouponList[i], ref item);//2018.03.31
                    desc.ReturningCouponList.Add(item);
                }

                string sHyId = "";
                if (desc.MemberInfo.id > 0)
                {
                    sHyId = desc.MemberInfo.id.ToString();
                }
                //  desc.qrUrl = CommonUtils.GetQRUrl(sDevice, sHyId, CrmTranID.ToString());
            }
            return result;
        }

        public int AssignCanReturnCoupon(TDealSaleMoneyLeft sour,
           ref TDealSaleMoneyLeft desc)
        {
            int result = 0;
            desc.AddupTypeDesc = sour.AddupTypeDesc;
            desc.CouponTypeName = sour.CouponTypeName;
            desc.PromotionName = sour.PromotionName;
            desc.RuleName = sour.RuleName;
            desc.SaleMoney = sour.SaleMoney;
            return result;
        }

        public int AssignDealMemberCard(TDealMemberCard sour, ref TDealMemberCard desc)
        {
            int result = 0;
            desc.id = sour.id;
            desc.memberNo = sour.memberNo;
            desc.memberType = sour.memberType;
            desc.mobilePhone = sour.mobilePhone;
            desc.name = sour.name;
            desc.sex = sour.sex;
            desc.ticketCent = sour.ticketCent;
            desc.totalCent = sour.totalCent;
            desc.validID = sour.validID;
            desc.validType = sour.validType;

            return result;
        }

        public int AssignTranGoods(Goods sour, ref TTranGoods desc)
        {
            int result = 0;
            desc.accountsPayable = sour.SaleMoney;
            desc.backendOffAmount = sour.BackDiscount;
            desc.backendOffID = sour.BackDiscountBill.BillId;
            desc.code = sour.Code;
            desc.count = sour.SaleCount;
            desc.deptCode = sour.DeptCode;
            desc.deptID = sour.DeptId;
            desc.frontendOffAmount = sour.FrontDiscount;
            desc.fullCutOffAmount = sour.DecreaseDiscount;
            desc.fullCutOffID = sour.IRefNo_MJ;
            desc.id = sour.Id;
            desc.inx = sour.SubTicktInx;
            desc.memberOff = sour.MemberDiscount;
            desc.memberOffID = sour.IRefNo_HY;
            desc.name = sour.Name;
            desc.price = sour.Price;
            desc.totalOffAmount = sour.Discount;
            return result;
        }

        public int AssignReturnCoupon(TDealReturnCoupon sour,
            ref TDealReturnCoupon desc)
        {
            int result = 0;
            desc.Balance = sour.Balance;
            desc.CardId = sour.CardId;
            desc.CouponId = sour.CouponId;
            desc.CouponName = sour.CouponName;
            desc.CouponType = sour.CouponType;
            desc.ReturnMoney = sour.ReturnMoney;
            desc.ValidDate = sour.ValidDate;
            return result;
        }

        public bool CheckOut(string posNo, int CrmBillId,
             out List<TDealReturnCoupon> ReturnCouponList, out List<TDealSaleMoneyLeft> CanReturnCouponList, out string msg)
        {
            msg = "";
            ReturnCouponList = new List<TDealReturnCoupon>();
            CanReturnCouponList = new List<TDealSaleMoneyLeft>();

            //CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.2> 提交CRM 准备开始 ");
            try
            {

                double billCent;
                double vipCent;
                OfferCoupon[] gainedCoupons;
                string offerCouponVipCode = "";
                SaleMoneyLeftWhenPromCalc[] saleMoneyLefts;

                bool result;
                int i = 0;

                // CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.5> 准备向CRM提交  ");

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = "CRMUSER";
                crmSoapHeader.Password = "CRMUSER";

                CheckOutRSaleBillRequest req = new CheckOutRSaleBillRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.serverBillId = CrmBillId;
                CheckOutRSaleBillResponse res = PosAPI.CheckOutRSaleBill(req);

                billCent = res.billCent;
                vipCent = res.vipCent;
                gainedCoupons = res.offerCoupons;
                saleMoneyLefts = res.leftSaleMoneys;
                msg = res.msg;

                result = res.CheckOutRSaleBillResult;

                // result = client.CheckOutRSaleBill(crmSoapHeader, CrmBillId, out msg, out billCent, out vipCent,
                //     out offerCouponVipCode, out gainedCoupons, out saleMoneyLefts);


                //  string sValue1 = "", sValue2 = "";
                //   sValue1 = MethodInput.Serialize(gainedCoupons);
                //   sValue2 = MethodInput.Serialize(saleMoneyLefts);

                //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.6> CRM执行0104[confirm]完成  " +
                //     " gainedCoupons返回数据[ " + sValue1 + " ]" +
                //     " saleMoneyLefts[ " + sValue2 + " ]"
                //     );

                if (result)
                {
                    //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.1> CRM执行0104[confirm]-->成功 ");
                    //这里处理返券，积分。
                    if (gainedCoupons != null)
                    {
                        // CommonUtils.WriteSKTLog(1, posNo,
                        //     "保存销售:<2.6.2.7.2.1> CRM执行0104[confirm] 返券不为空，准备取返券数据 条数:" + gainedCoupons.Count());

                        for (i = 0; i < gainedCoupons.Count(); i++)
                        {
                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.2.2> 取返券数据 第" + i + "条");

                            TDealReturnCoupon ReturnCoupon = new TDealReturnCoupon();

                            //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.2.3> 取券类型:CouponType[" + gainedCoupons[i].CouponType + "]");
                            ReturnCoupon.CouponId = gainedCoupons[i].CouponType;

                            //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.2.5> 取券名称:CouponTypeName[" + gainedCoupons[i].CouponTypeName + "]");
                            ReturnCoupon.CouponName = gainedCoupons[i].CouponTypeName;

                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.2.6> 取券日期:ValidDate[" + gainedCoupons[i].ValidDate + "]");
                            ReturnCoupon.ValidDate = gainedCoupons[i].ValidDate;

                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.2.7> 取返券金额:OfferMoney[" + gainedCoupons[i].OfferMoney + "]");
                            ReturnCoupon.ReturnMoney = RoundMoney(gainedCoupons[i].OfferMoney * 100);

                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.2.8> 取券余额:Balance[" + gainedCoupons[i].Balance + "]");
                            ReturnCoupon.Balance = RoundMoney(gainedCoupons[i].Balance * 100);

                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.2.9> 添加数据");
                            ReturnCouponList.Add(ReturnCoupon);
                        }
                        //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3> 取返券数据--->完成 ");
                    }

                    if (saleMoneyLefts != null)
                    {
                        for (i = 0; i < saleMoneyLefts.Count(); i++)
                        {
                            //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3.2> 取返券数据 第" + i + "条");

                            TDealSaleMoneyLeft RetDealSaleMoneyLeft = new TDealSaleMoneyLeft();

                            RetDealSaleMoneyLeft.AddupTypeDesc = saleMoneyLefts[i].AddupTypeDesc;
                            //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3.1.1> 待返券:AddupTypeDesc[" + RetDealSaleMoneyLeft.AddupTypeDesc + "]");

                            RetDealSaleMoneyLeft.CouponTypeName = saleMoneyLefts[i].CouponTypeName;
                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3.1.2> 待返券:CouponTypeName[" + RetDealSaleMoneyLeft.CouponTypeName + "]");


                            RetDealSaleMoneyLeft.PromotionName = saleMoneyLefts[i].PromotionName;
                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3.1.3> 待返券:PromotionName[" + RetDealSaleMoneyLeft.PromotionName + "]");

                            RetDealSaleMoneyLeft.RuleName = saleMoneyLefts[i].RuleName;
                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3.1.4> 待返券:RuleName[" + RetDealSaleMoneyLeft.RuleName + "]");

                            RetDealSaleMoneyLeft.SaleMoney = saleMoneyLefts[i].SaleMoney;
                            //   CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3.1.5> 待返券:SaleMoney[" + RetDealSaleMoneyLeft.SaleMoney + "]");

                            //    CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.3.1.6> 待返券 添加数据");
                            CanReturnCouponList.Add(RetDealSaleMoneyLeft);
                        }
                    }

                    //  sValue1 = MethodInput.Serialize(ReturnCouponList);
                    //  sValue2 = MethodInput.Serialize(CanReturnCouponList);


                    //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.7.5> 取返券数据--->完成 " +
                    //      " 返券数据:[ " + sValue1 + " ]" +
                    //      " 待返券数据:[ " + sValue2 + " ]");
                }
                else
                {
                    //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.8.1> CRM执行0104[confirm]-->失败 ");
                    //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.8.2> CRM执行0104[confirm]-->失败 原因[ " + msg + "]");
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();

                //  CommonUtils.WriteSKTLog(1, posNo, "保存销售:<2.6.2.9.1> CRM执行0104[confirm]-->异常失败 原因[ " + msg + "]");
                return false;
            }
            return true;
        }


        public bool BeforeSaveTransation(string sShop, string posNo, int iJlbh, int CrmBillId, List<CashCardDetails> cards, List<CouponDetails> Coupons,
            List<Payment> payList, ref List<Goods> GoodsList,
            out int CrmMoneyCardTransId, out int CrmCouponTransId, out double fCent, out string msg)
        {
            fCent = 0;
            CrmCouponTransId = 0;
            CrmMoneyCardTransId = 0;
           // bool bCheckCrmTran;

            msg = "";

           // bCheckCrmTran = true;
            try
            {
                if (!Prepare(posNo, CrmBillId, payList, ref GoodsList, out fCent, out msg))
                {
                    return false;
                }
             
                 if (!SaveMoneyCard(sShop, posNo, iJlbh, CrmBillId, cards, out CrmMoneyCardTransId, out msg))
                 {
                     return false;
                 }

                if (!SaveCoupons(posNo, CrmBillId, Coupons, out CrmCouponTransId, out msg))
                    return false;
            }
            catch (Exception e)
            {
                // CommonUtils.WriteSKTLog(1, posNo, "<0104_预提交> <3.4> 失败:" + e.Message.ToString());
                msg = "预提交出错:" + e.Message.ToString();
                return false;
            }

            return true;
        }

        public bool Prepare(string posNo, int CrmBillId, List<Payment> payList, ref List<Goods> GoodsList, out double fCent, out string msg)
        {
            msg = "";
            fCent = 0;
            int i = 0, j = 0;

          //  int PayBackCouponVipId = 0;
            bool CouponPaid = false;
          //  string offerCouponVipCode = "";
          //  bool bNeedVipToOfferCoupon, bNeedBuyCent;


            try
            {
                RSaleBillPayment[] pays = new RSaleBillPayment[payList.Count];

                for (i = 0; i < payList.Count; i++)
                {
                    pays[i] = new RSaleBillPayment();
                    pays[i].PayMoney = payList[i].PayedMoney;
                    pays[i].PayTypeCode = Convert.ToString(payList[i].Id);
                }

                RSaleBillArticleCent[] articleCents;
                RSaleBillArticleCoupon[] articleCoupons;
                RSaleBillArticleCoupon curArticle;
                RSaleBillArticleCent curCent;
                OfferBackCoupon[] offerBackCoupon;
                CouponPayback[] payBackCoupons;

                int iCurLine = 0, iCouponType = -1;
                double mYHJE = 0;
                bool PrepareCheckOutResult;
                double fRate = 0;

                //保存销售

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = "CRMUSER";
                crmSoapHeader.Password = "CRMUSER";
                PrepareCheckOutRSaleBillRequest req = new PrepareCheckOutRSaleBillRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.serverBillId = CrmBillId;
                req.payments = pays;
                req.payBackCouponVipId = 0;
                req.couponPaid = CouponPaid;

                PrepareCheckOutRSaleBillResponse res = PosAPI.PrepareCheckOutRSaleBill(req);


                fCent = res.billCent;
                articleCents = res.articleCents;
                articleCoupons = res.articleCoupons;
                PrepareCheckOutResult = res.PrepareCheckOutRSaleBillResult;
                msg = res.msg;

                // PrepareCheckOutResult = client.PrepareCheckOutRSaleBill(crmSoapHeader, CrmBillId,
                //    pays, PayBackCouponVipId, CouponPaid, out msg, out fCent, out bNeedVipToOfferCoupon, out bNeedBuyCent,
                //    out offerCouponVipCode, out articleCents, out articleCoupons, out offerBackCoupon, out payBackCoupons);

                // if (PrepareCheckOutResult)
                //      CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><1.2>完成提交_成功");
                //  else
                //     CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><1.2>完成提交_失败");

                if (PrepareCheckOutResult)
                {
                    //如果这里成功，处理积分 与 返券，目前先不做

                    //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.0.1>准备分摊优惠 ");

                    if (articleCoupons != null)
                    //    CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.1>准备分摊优惠:无用券商品数:");
                    // else
                    {
                        // CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.1>准备分摊优惠:返回用券商品数: " + articleCoupons.Length);
                        for (i = 0; i < articleCoupons.Length; i++)
                        {
                            curArticle = articleCoupons[i];
                            iCurLine = curArticle.ArticleInx;

                            for (j = 0; j < payList.Count(); j++)
                            {
                                if (curArticle.CouponType != payList[j].CouponId)
                                    continue;

                                fRate = 0;
                                break;
                            }

                            if (fRate > 1)
                                fRate = 0;
                            if (fRate < 0)
                                fRate = 0;

                            //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.2>券当前行: " + iCurLine + " 商品总数:" + GoodsList.Count);

                            if ((iCurLine >= 0) && (iCurLine < GoodsList.Count))
                            {
                                mYHJE = curArticle.SharedMoney;
                                mYHJE = RoundMoney(mYHJE * (1 - fRate));
                                iCouponType = curArticle.CouponType;

                                //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售>" +
                                //      "<2.3>券金额:" + mYHJE + " CRM分摊金额:" + curArticle.SharedMoney +
                                //      " 券类型:" + iCouponType + " 现金率:" + fRate);

                                GoodsList[iCurLine].OTherInt1 = GoodsList[iCurLine].OTherInt1 + mYHJE;
                                GoodsList[iCurLine].PreferentialMoney = GoodsList[iCurLine].PreferentialMoney + mYHJE;

                                //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.5>商品用券金额:" + GoodsList[iCurLine].PreferentialMoney);
                            }
                        }
                    }

                    //
                    iCurLine = -1;
                    //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.0.2>准备分摊积分 ");

                    if (articleCents != null)
                    //     CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.6.1>准备分摊积分:积分数据是NULL ");
                    // else
                    {
                        //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.6.2>准备分摊积分:返回积分数据: " + articleCents.Length);
                        for (i = 0; i < articleCents.Length; i++)
                        {
                            curCent = articleCents[i];
                            iCurLine = curCent.ArticleInx;

                            //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.6.3.1>积分数据: 序号:" + iCurLine + " 积分:" + curCent.Cent +
                            //      " 开始查询数据 商品计数:" + GoodsList.Count());

                            for (j = 0; j < GoodsList.Count(); j++)
                            {
                                //  CommonUtils.WriteSKTLog(1, posNo, "<0104_ERP保存销售><2.6.3.2> 积分数据: 序号:" + iCurLine + " 商品序号:" + j);

                                if (curCent.ArticleInx == j)
                                {
                                    GoodsList[j].Fzk_Rate = curCent.Cent;
                                    //     CommonUtils.WriteSKTLog(1, posNo,
                                    //           "<0104_ERP保存销售><2.6.3.3> 积分数据: 序号:" + iCurLine + " 商品序号:" + j + " 积分:" + GoodsList[j].Fzk_Rate);
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }

        public bool SaveCoupons(string posNo, int CrmBillId, List<CouponDetails> cards,
            out int CrmCouponTransId, out string msg)
        {
            CrmCouponTransId = 0;
            msg = "";
            string error = "";

            //    CommonUtils.WriteSKTLog(1, posNo, "<0104_保存券> UniWS <3.3.1> CRMID： " + CrmBillId);

            try
            {
                //判断.后面要再加
                if (!CancelCoupon(posNo, out msg))
                {
                    msg = "存在优惠券冲正信息，请处理后再进行优惠券交易！";
                    return false;
                } 

                /* string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";
                 CRMUSer = CommonUtils.GetReqStr("CRMUser");
                 CRMPwd = CommonUtils.GetReqStr("CRMPwd");

                 ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                 crmSoapHeader.UserId = CRMUSer; // "AAA";
                 crmSoapHeader.Password = CRMPwd;// "123";
                 PosWebServiceSoapClient client = client = new PosWebServiceSoapClient(); */

                List<CouponPayment> CouponList = new List<CouponPayment>();
                double totalMoney = 0;


                for (int j = 0; j < cards.Count; j++)
                {
                    CouponPayment payment = new CouponPayment();
                    payment.CouponType = cards[j].couponType;
                    payment.PayMoney = MoneyToDouble(cards[j].useMoney);  ///CommonUtils.MoneyToDouble(cards[j].amount);
                    payment.VipId = cards[j].cardId;
                    payment.CouponType = cards[j].couponType;

                    //    CommonUtils.WriteSKTLog(1, posNo, "<0104_保存券> UniWS <3.3.2> 保存 会员ID： " + payment.VipId +
                    //        " 券ID:" + payment.CouponType);

                    CouponList.Add(payment);
                    totalMoney += payment.PayMoney;
                }

                if (CouponList.Count == 0)
                {
                    return true;
                }
                CouponPayment[] cashCardPayments = new CouponPayment[CouponList.Count];
                CouponList.CopyTo(cashCardPayments);

                int transId;
                bool result;

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = "CRMUSER";
                crmSoapHeader.Password = "CRMUSER";

                PrepareTransCouponPaymentRequest req = new PrepareTransCouponPaymentRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.serverBillId = CrmBillId;
                req.payments = cashCardPayments;

                PrepareTransCouponPaymentResponse res = PosAPI.PrepareTransCouponPayment(req);

                transId = res.transId;
                msg = res.msg;
                result = res.PrepareTransCouponPaymentResult;


                // result = client.PrepareTransCouponPayment(crmSoapHeader, CrmBillId, cashCardPayments, out msg, out transId);

                if (result)
                {
                    //写冲正文件  2:YHQ 操作类型
                    //wangkx
                    /* if (!ErpProc.WriteCancelFile(posNo, transId, 0, totalMoney, 2, out msg))
                     {
                         Cancel(posNo, 2, transId, CrmBillId, totalMoney, out msg);
                         return false;
                     } */


                    ConfirmTransCouponPaymentRequest ctreq = new ConfirmTransCouponPaymentRequest();
                    ctreq.ABCSoapHeader = crmSoapHeader;
                    ctreq.transId = transId;
                    ctreq.serverBillId = CrmBillId;
                    ctreq.transMoney = totalMoney;

                    ConfirmTransCouponPaymentResponse ctres = PosAPI.ConfirmTransCouponPayment(ctreq);
                    msg = ctres.msg;
                    result = ctres.ConfirmTransCouponPaymentResult;

                    // result = client.ConfirmTransCouponPayment(crmSoapHeader, transId, CrmBillId, totalMoney, out msg);

                    /*  if (!result)
                      {
                          //msg = error;
                          //wangkx
                          Cancel(posNo, 2, transId, CrmBillId, totalMoney, out error);
                          return false;
                      } */

                    CrmCouponTransId = transId;
                }
                else
                {
                    //msg = error;
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }



        public double MoneyToDouble(double money)
        {
            return (double)money;
        }


        private int DoGetPayments(string posId, out List<Payment> payments, out string msg)
        {
            msg = "";
            payments = new List<Payment>();

            string sql = $"select b.payid,b.name,b.type,b.fk,b.jf,b.zlfs,b.flag,b.couponid,b.bj_fq,b.bj_mbjz,b.jfbl ";
            sql += $"        from station_pay a,pay b";
            sql += $"       where a.payid = b.payid";
            sql += $"         and void_flag = 1 and a.stationbh = '{posId}'";
            sql += $"       order by b.flag";

            DataTable payList = DbHelper.ExecuteTable(sql);

            for (int i = 0; i <= payList.Rows.Count - 1; i++)
            {
                Payment pay = new Payment();
                payments.Add(pay);

                pay.Id = payList.Rows[i][0].ToString().ToInt();
                pay.Name = payList.Rows[i][1].ToString();
                pay.PaymentType = payList.Rows[i][2].ToString().ToInt();

                if (!payList.Rows[i][7].ToString().IsEmpty())
                    pay.CouponId = payList.Rows[i][7].ToString().ToInt();
                else
                    pay.CouponId = -1;

                pay.ChangeType = payList.Rows[i][5].ToString().ToInt();

                if (payList.Rows[i][4].ToString().ToInt() == 1)
                    pay.IsPoint = true;
                else
                    pay.IsPoint = false;
                pay.IsDecreaseDiscount = true;
                pay.IsOfferCoupon = true;
                pay.DirectInput = true;
                pay.PayedMoney = 0;
                pay.MorePayedMoney = 0;
                pay.RealUsedMoney = 0;

                pay.TypeCode = pay.PaymentType.ToString();

            }

            return 0;
        }



        public bool CheckSaveData(string posNo, List<Payment> sktPayments, ReqConfirmDeal ReqConfirm,
            ref string msg)
        {
            msg = "";
            int i = 0, j = 0, k = 0;
            double totalCash = 0, totalCoupon = 0;
            bool result = false;

            //1:判断1:数目是否为0,为0:错误
            if (sktPayments.Count() <= 0)
            {
                msg = "错误:收款台可有收款方式数目为空";
                return result;
            }
            if (ReqConfirm.paysList.Count() <= 0)
            {
                msg = "错误:输入付款数目为空";
                return result;
            }

            TTranPayments curPay, curPay2;
            Payment defPay;

            //2:检查是否:相同的付款方式用了两次:实现方式,两个循环,不同i,j时ID相同，即为相同记录
            for (i = 0; i < ReqConfirm.paysList.Count(); i++)
            {
                curPay = ReqConfirm.paysList[i];
                for (j = 0; j < ReqConfirm.paysList.Count(); j++)
                {
                    curPay2 = ReqConfirm.paysList[j];

                    if (curPay.Id == curPay2.Id)
                    {
                        if (i != j)
                        {
                            msg = "错误:付款方式列表中有相同记录[ID:" + curPay.Id + " " +
                                curPay.PayMoney + "--" + curPay2.PayMoney + "]";
                            return result;
                        }
                    }
                }
            }

            //3:判断券的数据，CZK的数据是否一致
            for (i = 0; i < ReqConfirm.paysList.Count(); i++)
            {
                totalCoupon = 0;
                curPay = ReqConfirm.paysList[i];
                //if (curPay.Id)
                for (j = 0; j < sktPayments.Count(); j++)
                {
                    defPay = sktPayments[j];

                    //当前是CZK 
                    if ((curPay.Id == defPay.Id) && (defPay.TypeCode == "2"))
                    {
                        if (ReqConfirm.cashCashList == null)
                        {
                            msg = "错误:付款方式是储值卡类型,但卡明细为空null[" + defPay.Name + "]";
                            return result;
                        }
                        if (ReqConfirm.cashCashList.Count() <= 0)
                        {
                            msg = "错误:付款方式是储值卡类型,但卡明细为空[" + defPay.Name + "]";
                            return result;
                        }
                        for (k = 0; k < ReqConfirm.cashCashList.Count(); k++)
                        {
                            totalCash = totalCash + ReqConfirm.cashCashList[k].useMoney;
                        }

                        if (totalCash != curPay.PayMoney)
                        {
                            msg = "错误:储值卡付款类型,付款金额不等于明细[" + curPay.PayMoney + "--" + totalCash + "]";
                            return result;
                        }
                    }
                    else if ((curPay.Id == defPay.Id) && (defPay.TypeCode == "3"))
                    {
                        if (ReqConfirm.couponsList == null)
                        {
                            msg = "错误:付款方式是优惠券类型,但券明细为空null[" + defPay.Name + "]";
                            return result;
                        }

                        if (ReqConfirm.couponsList.Count() <= 0)
                        {
                            msg = "错误:付款方式是优惠券类型,但券明细为空[" + defPay.Name + "]";
                            return result;
                        }

                        for (k = 0; k < ReqConfirm.couponsList.Count(); k++)
                        {
                            if (ReqConfirm.couponsList[k].PayID.Equals(curPay.Id.ToString()))
                                totalCoupon = totalCoupon + ReqConfirm.couponsList[k].OutOfPocketAmount;
                        }

                        if (totalCoupon != curPay.PayMoney)
                        {
                            msg = "错误:优惠券付款类型[" + curPay.Id + "],付款金额不等于明细[" + curPay.PayMoney + "--" + totalCoupon + "]";
                            return result;
                        }
                    }
                }
            }
            result = true;
            return result;
        }


        //退货预提交
        public RespBackable CalcAccountsBackable(ReqBackAble req)
        {
            if (req == null)
                throw new Exception("提交数据为空！");


            string Shop = req.storeCode;
            string msg = "";
            int result = -1;

            if (Shop.Equals(""))
                throw new Exception("门店代码为空！");

            RespBackable backAbleResult = new RespBackable();

            MemberCard vipcard = new MemberCard();
            CashCardDetails cashCard = new CashCardDetails();
            List<Goods> GoodsList = new List<Goods>();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();
            List<Payment> DevicePayments = new List<Payment>();

            int iHTH = 0;
            string posNo = employee.PlatformId, userCode = "",
                 cardCodeToCheck = "", verifyCode = "", password = "", CondValue = "", sVIPCode = "", sDeptCode = "";
            int i = 0, j = 0, transId = 0, shopId = 0, deptid = 0,
                  CrmBillId = 0, backType = 0, bulkGoodsType = 0, iVIPID = -1;


           // string sTitle = "SysVer", sVer = "", sFuncCode = "<退货预算>:";

            Goods goods = new Goods();


           // posNo = Device;
            userCode = employee.Code;
            string sResult = "";

            int OldErpTranId = 0;
            string OldPosId = "";

            if (req.goodsList.Count() <= 0)
            {
                throw new Exception("数据检查失败：商品内容为空!");
            }

            if (string.IsNullOrEmpty(req.ValidID))
                req.ValidID = "";

            if (string.IsNullOrEmpty(req.deptCode))
                req.deptCode = "";

            OldPosId = req.oldDeviceNo;
            OldErpTranId = Convert.ToInt32(req.oldErpTranID);

            if (string.IsNullOrEmpty(OldPosId))
            {
                throw new Exception("数据检查失败：原来退款收款台号为空!");
            }

            if (OldErpTranId <= 0)
            {
                throw new Exception("数据检查失败：原来退款交易号小于等于0!");
            }

            //判断是否重复退货

            string sqlsale = $"select 1 from sale where posno_old='{OldPosId}' and dealid_old={OldErpTranId}";

            DataTable saleCount = DbHelper.ExecuteTable(sqlsale);
            
            if (saleCount.Rows.Count > 0)
            {
                result = -1;
                msg = "款台号[" + OldPosId + "] 交易号[" + req.oldErpTranID + "]已退货,不能重复退货!";
                throw new Exception(msg);
            }

            sqlsale = $"select 1 from his_sale where posno_old='{OldPosId}' and dealid_old={OldErpTranId}";
            saleCount = DbHelper.ExecuteTable(sqlsale);

            if (saleCount.Rows.Count > 0)
            {
                result = -1;
                msg = "款台号[" + OldPosId + "] 交易号[" + req.oldErpTranID + "]已退货,不能重复退货!";
                throw new Exception(msg);
            }


            iHTH = req.contractID;
            Stopwatch st = new Stopwatch();

            try
            {
                st.Start();

                result = -1;
                //2.1: 取收款台 
                //CommonUtils.WriteSKTLog(1, posNo, sFuncCode + "<1.1> " + "第一步:取收款台");

            //    CommonUtils.WriteSKTLog(1, Device, sFuncCode + "<1.1> 取:收款台的收款方式:收款台号:" + Device);
                result = DoGetPayments(posNo, out DevicePayments, out msg);
                if (result != 0)
                {
                    throw new Exception("该款台没有收款方式定义!");
                }


                //2.2:如果有会员.则取会员      
                int iMemberType = 0;
                int iCanVIPDisc = req.vipIsDiscount;
                /*  if (string.IsNullOrEmpty(req.validType))
                      req.validType = Member_CondTypeName_Track;
                  else if (req.validType.Equals(""))
                      req.validType = Member_CondTypeName_Track;

                  if (req.validType.Equals(Member_CondTypeName_Track))
                      iMemberType = Member_CondType_CDNR;
                  else if (req.validType.Equals(Member_CondTypeName_CardNo))
                      iMemberType = Member_CondType_HYK_NO;
                  else if (req.validType.Equals(Member_CondTypeName_HYID))
                      iMemberType = Member_CondType_HYID;
                  else
                      iMemberType = Member_CondType_CDNR; */

                iMemberType = int.Parse(req.validType);

                GetMemberInfo(iMemberType, req.ValidID, out vipcard, out msg);


                //2.3取CZK信息
                if (vipcard.id > 0)
                {
                    cardCodeToCheck = ""; verifyCode = ""; password = ""; CondValue = Convert.ToString(vipcard.id);
                    GetCashCardInfo(1, CondValue, Shop, cardCodeToCheck, verifyCode, password,
                        out cashCard, out msg);

                    int iPayID = -1;
                    string sPayName = "";
                    GetCashCardPayID(ref iPayID, ref sPayName, out msg);
                    if (iPayID > 0)
                    {
                        cashCard.payID = iPayID;
                    }
                }

                //2.2:取商品
             //   CommonUtils.WriteSKTLog(1, posNo, sFuncCode + " <2> 第二步:查询商品 店:" + shopId);
                result = -1;

                bool bRslt = false;
                string ItemCode = "",sql = "";
             //   ErrorMessage message = new ErrorMessage();

            //    string sTitle1 = "FullCutType", sType = "";
            //    sType = CommonUtils.GetReqStr(sTitle1);
            //    if (string.IsNullOrEmpty(sType))
            //        sType = FullCut_ERP;

                for (i = 0; i < req.goodsList.Count; i++)
                {
                    ItemCode = req.goodsList[i].code;
                    bRslt = DoGetGoodsInfo(ItemCode,deptid, backType, bulkGoodsType, Shop, posNo,
                          out goods);

                    if (!bRslt)
                    {
                        throw new Exception("商品"+ ItemCode + "未定义!");
                    }


                    if (string.IsNullOrEmpty(req.goodsList[i].deptCode))
                    {
                        sql = "select d.orgcode from GOODS a,GOODS_SHOP b,SHOP c,ORG d";   //取部门
                        sql += "  where a.goodsid=b.goodsid and b.shopid=c.shopid and c.orgid=d.orgid ";
                        sql += $"  and a.goodsid = {goods.Id} ";

                        DataTable dt = DbHelper.ExecuteTable(sql);

                        if (dt.IsNotNull())
                        {
                            goods.DeptCode = dt.Rows[0][0].ToString();
                        }
                    }



                        double fCount = 0;
                    fCount = Convert.ToDouble(req.goodsList[i].count);

                    //2.2.2:付:数量,总价
                    goods.SaleCount = fCount;
                    goods.FrontDiscount = req.goodsList[i].frontendOffAmount;
                    goods.MemberDiscount = req.goodsList[i].memberOff;
                    goods.BackDiscount = req.goodsList[i].backendOffAmount;
                    goods.DecreaseDiscount = req.goodsList[i].fullCutOffAmount;
                    goods.Discount = GetSPDisc(goods);

                    if ((goods.Price == 0) && (req.goodsList[i].price != 0))
                        goods.Price = req.goodsList[i].price;

                    if ((goods.SaleMoney == 0) && (goods.Price != 0))
                        goods.SaleMoney = RoundMoney(goods.SaleCount * goods.Price);

                    goods.SubTicktInx = req.goodsList[i].tickInx;
                    goods.SubTicktInx_old = req.goodsList[i].tickInx;

                    goods.SubGoodsInx = req.goodsList[i].inx;
                    goods.SubGoodsInx_old = req.goodsList[i].inx;
                    goods.CrmInx = req.goodsList[i].inx;

                    GoodsList.Add(goods);
                }

                for (i = 0; i < GoodsList.Count(); i++)
                {
                    GoodsList[i].Discount = GetSPDisc(GoodsList[i]);
                }

                Member member = new Member();
                member.MemberId = vipcard.id;
                member.MemberNo = vipcard.memberNo;
                member.MemberType = vipcard.memberType;

                //取当前最大交易号
                transId = int.Parse(GetLastDealid().ToString()) + 1;

                //3.2:取数据:上传商品
                if (!BackUploadSp(posNo, userCode, transId, Shop, member, GoodsList,
                    OldErpTranId, req.oldDeviceNo,
                    out CrmBillId, out msg))
                {
                    throw new Exception("上传商品失败:"+ msg);
                }

                if (vipcard.id > 0)
                {
                    string sCountPwd = "NOCHECKPWD";
                    cardCodeToCheck = ""; verifyCode = ""; password = ""; CondValue = Convert.ToString(vipcard.id);
                    GetVipCoupon(1, CondValue, Shop, cardCodeToCheck, verifyCode, CrmBillId,
                        out ListCoupon, out iVIPID, out sVIPCode, out msg);

                    if (ListCoupon != null)
                    {
                        if (ListCoupon.Count > 0)
                        {
                            for (i = 0; i < ListCoupon.Count; i++)
                            {
                                for (j = 0; j < DevicePayments.Count; j++)
                                {
                                    if (ListCoupon[i].couponId == DevicePayments[j].CouponId)
                                    {
                                        ListCoupon[i].payID = DevicePayments[j].Id.ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                result = 0;


                //3:生成输出商品
                //如果想存一下数据.此处,可以存一下 
             //   CommonUtils.WriteSKTLog(1, posNo, sFuncCode + " <2.6.1> 生成返回数据");

                if (GoodsList == null)
                    GoodsList = new List<Goods>();
                if (ListCoupon == null)
                    ListCoupon = new List<CouponDetails>();

                result = 0;
                UniCalcAccountsBackable(posNo, 0, transId, CrmBillId, "", GoodsList, vipcard, cashCard, ListCoupon, out backAbleResult);


             //   sResult = CommonUtils.UniMakeStr(iDataType, backAbleResult);

             //   CommonUtils.WriteSKTLog(1, posNo, sFuncCode + " <3.3> 计算成功 返回: " + sResult);

            }
            finally
            {
                st.Stop();
            }

            return backAbleResult;
        }

        public static double GetSPDisc(Goods goodItem)
        {
            double result = 0;
            result = goodItem.FrontDiscount + goodItem.BackDiscount + goodItem.MemberDiscount + goodItem.DecreaseDiscount +
                goodItem.ChangeDiscount;
            return result;
        }

        public bool BackUploadSp(string posNo, string sOperator, int transId, string StoreCode, Member member, List<Goods> GoodsList,
           int OldTicketId, string OldPosId, out int CrmBillId, out string msg)
        {
            CrmBillId = 0;
            msg = "";
            try
            {
                // CanReturnCoupon=false;
                //  string sTitle1 = "", sTitle2 = "", sTitle3 = "", sTitle4 = "", sTitle5 = "";
                string sValue1 = ""; //, sValue2 = "", sValue3 = "", sValue4 = "", sValue5 = "";

             //   sTitle1 = "CanReturnCoupon";
             //   string sGet = ConfigurationManager.AppSettings["SetString"];
             //   CommonUtils.GetReqStr(sGet, sTitle1, sTitle2, sTitle3, sTitle4, sTitle5, ref sValue1, ref sValue2, ref sValue3, ref sValue4, ref sValue5);
                string canReturnCoupon = sValue1;

                string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = CRMUSer; 
                crmSoapHeader.Password = CRMPwd;
              //  PosWebServiceSoapClient client = new PosWebServiceSoapClient();

                RSaleBillHead billHead = new RSaleBillHead();

                //billHead.BillId = 0;// transId;
                billHead.BillId = transId;// transId;
                billHead.BillType = 1;

                billHead.Cashier = sOperator;
                billHead.PosId = posNo;
                billHead.SaleTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                billHead.AccountDate = DateTime.Now.ToString("yyyy-MM-dd");
                billHead.StoreCode = StoreCode;
                billHead.VipId = member.MemberId;

                billHead.OriginalBillId = OldTicketId;
                billHead.OriginalPosId = OldPosId;
                List<RSaleBillArticle> articleList = new List<RSaleBillArticle>();
                for (int i = 0; i <= GoodsList.Count - 1; i++)
                {
                    if (GoodsList[i].Id > 0)
                    {
                        RSaleBillArticle article = new RSaleBillArticle();
                        GoodsList[i].Discount = GetSPDisc(GoodsList[i]);
                        article.ArticleCode = GoodsList[i].Code;
                        article.DeptCode = GoodsList[i].DeptCode;


                        article.DiscMoney = GoodsList[i].Discount;

                        article.Inx = i;
                        article.IsNoCent = false;
                        article.IsNoProm = false;
                        article.SaleMoney = MoneyToDouble(GoodsList[i].SaleMoney - GoodsList[i].Discount);
                        article.SaleNum = (double)GoodsList[i].SaleCount;
                        article.DiscMoney = MoneyToDouble(GoodsList[i].Discount);


                        article.DiscMoney = article.DiscMoney * (-1);
                        article.SaleMoney = article.SaleMoney * (-1);
                        article.SaleNum = article.SaleNum * (-1);

                        if (canReturnCoupon.Equals("false"))
                            article.IsNoProm = true;

                        articleList.Add(article);
                    }
                }

                RSaleBillArticle[] articles = new RSaleBillArticle[articleList.Count];
                articleList.CopyTo(articles);

                int crmBillId;
           /*     double decMoney;
                RSaleBillArticleDecMoney[] articleDecMoneys;
                RSaleBillArticlePromFlag[] articlePromFlags;  */

                OfferBackCoupon[] offBackCoupon;
                CouponPayback[] payBackCoupons;

                bool saveResult = false;
             //   saveResult = client.SaveRSaleBackBillArticles(crmSoapHeader, billHead, articles, out msg, out crmBillId,
             //       out offBackCoupon, out payBackCoupons);


                SaveRSaleBackBillArticlesRequest req = new SaveRSaleBackBillArticlesRequest();

                req.ABCSoapHeader = crmSoapHeader;
                req.billHead = billHead;
                req.billArticles = articles;

                SaveRSaleBackBillArticlesResponse res = PosAPI.SaveRSaleBackBillArticles(req);

                saveResult = res.SaveRSaleBackBillArticlesResult;
                msg = res.msg;
                crmBillId = res.serverBillId;
                offBackCoupon = res.offerBackCoupons;
                payBackCoupons = res.paybackCoupons;

                if (saveResult)
                {
                    CrmBillId = crmBillId;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
        }

        public int UniCalcAccountsBackable(string posNo, int iCode, int transId, int crmBillId, string sPrompt,
           List<Goods> GoodsList,
            MemberCard vipcard, CashCardDetails cashCard,
           List<CouponDetails> ListCoupon,
           out RespBackable desc)
        {

            string sFunc = "<退货预算>:";
            int i = 0;
            double mTotalCanUse = 0;
            //1.1：创建变量
            desc = new RespBackable();

            desc.code = iCode;
            desc.text = sPrompt;
            desc.erpTranID = transId;
            desc.crmTranID = crmBillId;
            desc.GoodsList = new List<UniGoods>();
            UniGoods uniGood;
            if (iCode == 0)
            {
                desc.GoodsList.Clear();
                for (i = 0; i < GoodsList.Count; i++)
                {
                    uniGood = new UniGoods();
                    AssignGoodToUniGood(GoodsList[i], out uniGood);
                    uniGood.discount = GetSPDisc(GoodsList[i]);

                    //输出数据时，折扣计入
                    uniGood.saleMoney = uniGood.saleMoney - uniGood.discount;
                    desc.GoodsList.Add(uniGood);

                }
            }

            desc.MemberInfo = new MemberCard();

            if (vipcard.memberNo != null)
            {
                desc.MemberInfo.id = vipcard.id;
                desc.MemberInfo.memberNo = vipcard.memberNo;
                desc.MemberInfo.memberType = vipcard.memberType;
                desc.MemberInfo.totalCent = vipcard.totalCent.ToString();
                desc.MemberInfo.ticketCent = "";

                desc.MemberInfo.sex = vipcard.sex;
                desc.MemberInfo.validID = vipcard.validID;
                desc.MemberInfo.validity = vipcard.validity;
                desc.MemberInfo.validType = vipcard.validType;
                desc.MemberInfo.mobilePhone = vipcard.mobilePhone;
                desc.MemberInfo.name = vipcard.name;
                desc.MemberInfo.memberTypeName = vipcard.memberTypeName;
                //validType validID mobilePhone sex validity
            }

            //CZK, 优惠券.此处有时间再加一下查询
            desc.CashCard = new CashCardDetails();

            if (cashCard.cardNo != null)
            {
                desc.CashCard.cardId = cashCard.cardId;
                desc.CashCard.cardNo = cashCard.cardNo;
                desc.CashCard.amount = cashCard.amount;
                desc.CashCard.payID = cashCard.payID;
                desc.CashCard.useMoney = 0;
            }
            //cashCard

            desc.CouponList = new List<TPayableCoupon>();
            if ((ListCoupon != null) && (ListCoupon.Count > 0))
            {
                mTotalCanUse = 0;
                TPayableCoupon CouponItem;

                for (i = 0; i < ListCoupon.Count; i++)
                {
                    CouponItem = new TPayableCoupon();
                    mTotalCanUse = mTotalCanUse + ListCoupon[i].amountCanUse;
                }

                if (mTotalCanUse > 0)
                {
                    for (i = 0; i < ListCoupon.Count; i++)
                    {
                        CouponItem = new TPayableCoupon();

                        CouponItem.balance = ListCoupon[i].amount;
                        CouponItem.accountsPayable = ListCoupon[i].amountCanUse;
                        CouponItem.cardId = ListCoupon[i].cardId;
                        CouponItem.couponId = ListCoupon[i].couponId;
                        CouponItem.couponName = ListCoupon[i].couponName;
                        CouponItem.couponType = ListCoupon[i].couponType;
                        CouponItem.payID = ListCoupon[i].payID;
                        CouponItem.payName = ListCoupon[i].payName;
                        CouponItem.validity = ListCoupon[i].valid_date;

                        desc.CouponList.Add(CouponItem);
                    }
                }
            }
            return 0;
        }



        public ConfirmBackDealResult ConfirmBackDeal (ReqConfirmBackDeal ReqConfirm)
        {
            if (ReqConfirm == null)
                throw new Exception("提交数据为空！");

            string  msg = "";
            string Shop = ReqConfirm.storeCode;
            string Device = employee.PlatformId;
            string Operator = employee.Code;
            int result = -1, i = 0, j = 0, CrmBillId = 0, CrmMoneyCardTransID = 0;
            string PromniDealID = "", ErpTranID = "", MemberCardID = "",
                sOut = "", posNo = "", userCode = "", sFunc = "<保存选单退货> ";

            if (Shop.Equals(""))
                throw new Exception("门店代码为空！");


            ErrorMessage errorMessage;
            ConfirmBackDealResult confirmResult = new ConfirmBackDealResult();

            //2015.08.24
            int transId = 0, iPerson = 0;
            double fCent = 0;
            Member member = new Member();
            Person CurPerson = new Person();

            Goods goods = new Goods();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();

            List<Payment> sktPayments = new List<Payment>();
            TDealMemberCard MemberInfo = new TDealMemberCard();
            List<TDealReturnCoupon> ReturnCouponList = new List<TDealReturnCoupon>();
            List<TDealSaleMoneyLeft> CanReturnCouponList = new List<TDealSaleMoneyLeft>();

            bool bCheckMember = false, bCheckCrmTran = true;
            string OldPosId = "";
            int OldErpTranId = 0;

            posNo = Device;
            userCode = Operator;

            //1.1:检查基本输入数据
          //  string input = "";

          //  if (bCheckMember)
          //      input = "店:" + Shop + " 设备号:" + posNo + " 用户代码:" + userCode +
          //        " 输入数据:长度:" + sInput.Length + " 内容:" + sInput + " [设置检查会员]";
          //  else
          //      input = "店:" + Shop + " 设备号:" + posNo + " 用户代码:" + userCode +
          //        " 输入数据:长度:" + sInput.Length + " 内容:" + sInput + " [设置不检查会员]";

          //  if (bCheckCrmTran)
          //      input = input + " [设置检查CRM交易---是否为空]";
          //  else
          //      input = input + " [设置不检查CRM交易*****是否为空]";

          //  CommonUtils.WriteSKTLog(0, posNo, sFunc + "<1.1> 输入数据:" + input);


            //2.2:判断基础数据
          //  CommonUtils.WriteSKTLog(1, posNo, sFunc + "<1.3.1> 检查输入数据");
            try
            {
                ErpTranID = ReqConfirm.erpTranID;
                CrmBillId = Convert.ToInt32(ReqConfirm.crmTranID);
                PromniDealID = ReqConfirm.outOrder;
                MemberCardID = ReqConfirm.validID;


                OldPosId = ReqConfirm.oldDeviceNo;
                OldErpTranId = Convert.ToInt32(ReqConfirm.oldErpTranID);

                if (string.IsNullOrEmpty(ErpTranID))
                    ErpTranID = "";
                if (string.IsNullOrEmpty(PromniDealID))
                    PromniDealID = "";
                if (string.IsNullOrEmpty(MemberCardID))
                    MemberCardID = "";

                if (string.IsNullOrEmpty(OldPosId))
                    OldPosId = "";


                if (ErpTranID.Equals(""))
                {
                    throw new Exception("ERP交易号为空!");
                }

                if (bCheckCrmTran)
                {
                    if (CrmBillId <= 0)
                    {
                        throw new Exception("CRM交易号为空!");
                    }
                }

                if (bCheckMember && (MemberCardID.Equals("")))
                {
                    throw new Exception("会员为空!");
                }

                if (ReqConfirm.goodsList == null)
                {
                    throw new Exception("商品数据为空!");
                }

                if (ReqConfirm.paysList == null)
                {
                    throw new Exception("付款数据为空!");
                }

               // CommonUtils.WriteSKTLog(1, posNo, sFunc + "<1.3.3> 检查通过 输入数据合法");
            }
            catch (Exception e)
            {
                result = -1;
                msg = "< 保存销售 > 检查请求数据失败" + e.Message;
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }


            //2.3:转换基础数据.判断数据库中的数据
            try
            {
                transId = 0;
                transId = Convert.ToInt32(ErpTranID);
                if (transId <= 0)
                {
                    result = -1;
                    msg = "数据检查失败：请求记录号小于或者等于0[" + transId + "-" + ErpTranID + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }
                //当前最大交易号
                long iRemoteTranID = GetLastDealid();
              //  JsonDoGetMaxId(Device, 0, out iRemoteTranID, out msg);
                iRemoteTranID = iRemoteTranID + 1;
                if (iRemoteTranID <= 0)
                {
                    result = -1;
                    msg = "数据检查失败：取记录号失败：当前交易号小于或者等于0[" + iRemoteTranID + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);

                }
                if (transId < iRemoteTranID)
                {
                    result = -1;
                    msg = sFunc + " 记录号错误：记录号错误[" + transId + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }


                DoGetPayments(posNo, out sktPayments, out msg);
                if ((sktPayments == null))
                {
                    result = -1;
                    msg = sFunc + " 本收款台没有定义收款方式:[ 收款台:" + posNo + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                if ((sktPayments != null) && (sktPayments.Count() < 0))
                {
                    result = -1;
                    msg = sFunc + " 本收款台没有定义收款方式:[ 收款台:" + posNo + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                if (!CheckSaveData(posNo, sktPayments, ReqConfirm, ref msg))
                {
                   
                    result = -1;
                    msg = sFunc + " 查询数据一致失败:[ " + msg + "]";
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                result = -1;
                //  result = DoGetPersonInfo(posNo, 0, Operator, "", WorkType_NoSet, out CurPerson, out msg);
                CurPerson.PersonId = employee.Id.ToInt();
                CurPerson.PersonCode =  employee.Code;
                CurPerson.PersonName = employee.Name;

                result = 0;
                iPerson = CurPerson.PersonId;
            }
            catch (Exception e)
            {
                result = -1;
                msg = sFunc + " 取数据失败" + e.Message;
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }

            //2.5:将输入数据转换为内容数据
            List<Goods> GoodList = new List<Goods>();
            List<Payment> PayList = new List<Payment>();
            List<CashCardDetails> CashCardList = new List<CashCardDetails>();
            List<CouponDetails> CouponList = new List<CouponDetails>();

            try
            {
                for (i = 0; i < ReqConfirm.goodsList.Count(); i++)
                {
                    Goods GoodItem = new Goods();
                    GoodItem.SubTicktInx = ReqConfirm.goodsList[i].tickInx;
                    GoodItem.SubGoodsInx = ReqConfirm.goodsList[i].inx;
                    GoodItem.PersonId = ReqConfirm.goodsList[i].assistantId;
                    GoodItem.Id = ReqConfirm.goodsList[i].id;
                    GoodItem.Code = ReqConfirm.goodsList[i].code;
                    GoodItem.DeptCode = ReqConfirm.goodsList[i].deptCode;
                    GoodItem.DeptId = ReqConfirm.goodsList[i].deptID;
                    GoodItem.ShopId = GetGoodsShopId(ReqConfirm.goodsList[i].id);
                    GoodItem.BackDiscount = ReqConfirm.goodsList[i].backendOffAmount;
                    GoodItem.DiscountBillId = ReqConfirm.goodsList[i].backendOffID;
                    GoodItem.IRefNo_MJ = ReqConfirm.goodsList[i].fullCutOffID;
                    GoodItem.VipDiscBillId = ReqConfirm.goodsList[i].memberOffID;
                    //GoodItem.MemberDiscount = ReqConfirm.goodsList[i].memberOff;
                    GoodItem.FrontDiscount = ReqConfirm.goodsList[i].frontendOffAmount;
                    GoodItem.DiscoaddDiscount = ReqConfirm.goodsList[i].fullCutOffAmount;
                    GoodItem.MemberDiscount = ReqConfirm.goodsList[i].memberOff;
                    GoodItem.Discount = ReqConfirm.goodsList[i].totalOffAmount;
                    GoodItem.SubTicktInx = ReqConfirm.goodsList[i].inx;
                    GoodItem.Name = ReqConfirm.goodsList[i].name;
                    GoodItem.Price = ReqConfirm.goodsList[i].price;
                    GoodItem.SaleCount = ReqConfirm.goodsList[i].count;
                    GoodItem.SaleMoney = ReqConfirm.goodsList[i].accountsPayable; //+ ReqConfirm.goodsList[i].totalOffAmount;

                    GoodItem.BackDiscount = GoodItem.BackDiscount * -1;
                    GoodItem.MemberDiscount = GoodItem.MemberDiscount * -1;
                    GoodItem.FrontDiscount = GoodItem.FrontDiscount * -1;
                    GoodItem.DiscoaddDiscount = GoodItem.DiscoaddDiscount * -1;
                    GoodItem.Discount = GoodItem.Discount * -1;
                    GoodItem.SaleCount = GoodItem.SaleCount * -1;
                    GoodItem.SaleMoney = GoodItem.SaleMoney * -1;

                    GoodList.Add(GoodItem);
                }

                for (i = 0; i < ReqConfirm.paysList.Count(); i++)
                {
                    Payment PayItem = new Payment();
                    PayItem.Id = ReqConfirm.paysList[i].Id;
                    PayItem.PayedMoney = ReqConfirm.paysList[i].PayMoney;
                    PayItem.PayedMoney = PayItem.PayedMoney * -1;

                    for (j = 0; j < sktPayments.Count; j++)
                    {
                        if (sktPayments[j].Id == PayItem.Id)
                        {
                            PayItem.PaymentType = sktPayments[j].PaymentType;
                            break;
                        }
                    }
                    PayList.Add(PayItem);
                }

                if ((ReqConfirm.couponsList != null) && (ReqConfirm.couponsList.Count() > 0))
                {
                    for (i = 0; i < ReqConfirm.couponsList.Count(); i++)
                    {
                        CouponDetails CouponItem = new CouponDetails();
                        CouponItem.amount = ReqConfirm.couponsList[i].Balance;
                        CouponItem.amountCanUse = ReqConfirm.couponsList[i].AccountsPayable;
                        CouponItem.cardId = ReqConfirm.couponsList[i].CardId;
                        CouponItem.cardNo = "";
                        CouponItem.couponId = ReqConfirm.couponsList[i].CouponId;
                        CouponItem.couponName = ReqConfirm.couponsList[i].CouponName;
                        CouponItem.couponType = ReqConfirm.couponsList[i].CouponType;
                        CouponItem.payID = ReqConfirm.couponsList[i].PayID;
                        CouponItem.useMoney = ReqConfirm.couponsList[i].OutOfPocketAmount;
                        CouponItem.useMoney = CouponItem.useMoney * -1;
                        CouponList.Add(CouponItem);
                    }
                }

                if ((ReqConfirm.cashCashList != null) && (ReqConfirm.cashCashList.Count() > 0))
                {
                    for (i = 0; i < ReqConfirm.cashCashList.Count(); i++)
                    {
                        CashCardDetails CashItem = new CashCardDetails();
                        CashItem.amount = ReqConfirm.cashCashList[i].amount;
                        CashItem.cardId = ReqConfirm.cashCashList[i].cardId;
                        CashItem.cardNo = ReqConfirm.cashCashList[i].cardNo;
                        CashItem.useMoney = ReqConfirm.cashCashList[i].useMoney;
                        CashItem.useMoney = CashItem.useMoney * -1;
                        CashCardList.Add(CashItem);
                    }
                }

            }
            catch (Exception e)
            {
                result = -1;
                msg = sFunc + "传入数据有错误,失败:" + e.Message;
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }


            //3.1:取VIP相关信息

            MemberCard vipcard = new MemberCard();
            try
            {
                member.MemberId = -1;
                member.MemberNo = "";
                member.MemberType = -1;


                int iMemberType = 0;
                /* if (string.IsNullOrEmpty(ReqConfirm.validType))
                     ReqConfirm.validType = Member_CondTypeName_Track;
                 else if (ReqConfirm.validType.Equals(""))
                     ReqConfirm.validType = Member_CondTypeName_Track;

                 if (ReqConfirm.validType.Equals(Member_CondTypeName_Track))
                     iMemberType = Member_CondType_CDNR;
                 else if (ReqConfirm.validType.Equals(Member_CondTypeName_CardNo))
                     iMemberType = Member_CondType_HYK_NO;
                 else if (ReqConfirm.validType.Equals(Member_CondTypeName_HYID))
                     iMemberType = Member_CondType_HYID;
                 else
                     iMemberType = Member_CondType_CDNR;  */

                iMemberType = int.Parse(ReqConfirm.validType);

                if (!MemberCardID.Equals(""))
                {
                    if (GetMemberInfo(iMemberType, MemberCardID, out vipcard, out msg))//Member_CondType_HYK_NO
                    {
                        member.MemberId = vipcard.id;
                        member.MemberNo = vipcard.memberNo;
                        member.MemberType = vipcard.memberType;
                    }
                    else
                    {

                        result = -1;
                        msg = sFunc + "取会员数据失败：" + msg;
                        confirmResult.code = result;
                        confirmResult.text = msg;
                        throw new Exception(msg);
                    }

                    if (vipcard.id <= 0)
                    {
                        result = -1;
                        msg = sFunc + "取会员数据失败：会员不存在[" + MemberCardID + "]";
                        confirmResult.code = result;
                        confirmResult.text = msg;
                        throw new Exception(msg);
                    }
                }
            }
            catch (Exception e)
            {
                result = -1;
                msg = sFunc + "取会员数据失败：" + e.Message;
                confirmResult.code = result;
                confirmResult.text = msg;
                throw new Exception(msg);
            }



            //3.2:开始保存
            Stopwatch st = new Stopwatch();
            try
            {
                st.Start();
                //3.1:取数据:收款台
                result = 0;

                //3.2:取数据:上传商品
              //  CommonUtils.WriteSKTLog(1, posNo, sFunc + "<2.2> 如果有会员,计算可以用券 会员ID:" + member.MemberId);
                if (member.MemberId > 0)
                {
                    int iVIPID = 0;
                    string sCountPwd = "NOCHECKPWD";
                    string cardCodeToCheck = "", verifyCode = "", sVIPCode = "", CondValue = Convert.ToString(member.MemberId);
                    GetVipCoupon(1, CondValue, Shop, cardCodeToCheck, verifyCode,CrmBillId,
                        out ListCoupon, out iVIPID, out sVIPCode, out msg);
                }

              //  CommonUtils.WriteSKTLog(1, posNo, sFunc + "<2.3.1.1> 分摊纸券");
              //  DevicePaperVoucher(posNo, PayList, ref GoodList, ref msg);

                //优惠券列表
              //  CommonUtils.WriteSKTLog(1, posNo, sFunc + "<2.3.1.2> CRM:作0104的Prepare 函数[BeforeSave]");

                CrmMoneyCardTransID = 0;
                int CrmCouponTransId = 0;

                
                if (!BeforeSaveBackTransation(Shop, posNo, transId, CrmBillId, CashCardList, CouponList, PayList,
                    ref GoodList, out CrmMoneyCardTransID,
                    out CrmCouponTransId, out fCent, out msg))
                {
                    result = -1;
                    msg = "保存销售:失败：" + msg;
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }

                //保存退货的单据
                if (!CheckOutBackTranToDatabase(Shop, posNo, iPerson, transId, CrmBillId, PromniDealID, member,
                    GoodList, PayList, ReqConfirm.creditDetailList,
                    CrmMoneyCardTransID, CrmCouponTransId, OldPosId, OldErpTranId, out errorMessage))
                {
                    result = -1;
                    string error = "";
                    if (errorMessage.Message.Equals(""))
                        msg = ErrorMessageToString(errorMessage);
                    else
                        msg = errorMessage.Message;

                    confirmResult.code = result;
                    confirmResult.text = msg;

                      if (CrmMoneyCardTransID > 0)
                      {
                        //  储值卡冲正
                        CancelMoneyCard(posNo, out error);
                      }  
                      if (CrmCouponTransId > 0)
                      {
                        //  优惠券冲正
                        CancelCoupon(posNo, out error);
                      }

                    throw new Exception(msg);
                }
                else
                {
                    ReturnCouponList.Clear();
                    CanReturnCouponList.Clear();

                    if (!CheckOut(posNo, CrmBillId, out ReturnCouponList, out CanReturnCouponList, out msg))
                    {
                        result = -1;
                        throw new Exception("提交CRM  失败:" + msg);
                    }
                }

                //5:返回数据
                if (result == 0)
                {
                    MemberCard tempMemberCard = new MemberCard();
                    if (vipcard.name != null)
                    {
                       // CommonUtils.WriteSKTLog(1, posNo, sFunc + "<3.2.1>计算总积分: 累加前:" + vipcard.totalCent.ToString());

                        string TotalCent = "";
                        double fValue1 = 0, fValue2 = 0;
                        try
                        {
                            fValue1 = Convert.ToDouble(vipcard.totalCent);
                            fValue2 = fValue1 + fCent;
                            TotalCent = fValue2.ToString();

                        //    CommonUtils.WriteSKTLog(1, posNo, sFunc + "<3.2.2>计算总积分: " +
                        //         fValue2.ToString() + " = " + fCent.ToString() + " + " + fValue1.ToString());

                        }
                        catch (Exception e)
                        {
                            TotalCent = vipcard.totalCent;
                            throw new Exception("计算总积分: 处理错误[按处理之前] " +
                               vipcard.totalCent.ToString() + "  错误:" + e.Message);
                        }

                        MemberInfo.id = vipcard.id;
                        MemberInfo.name = vipcard.name;
                        MemberInfo.memberNo = vipcard.memberNo;
                        MemberInfo.memberType = member.MemberType.ToString();
                        MemberInfo.mobilePhone = tempMemberCard.mobilePhone;
                        MemberInfo.sex = tempMemberCard.sex;
                        MemberInfo.validID = tempMemberCard.validID;
                        MemberInfo.validType = tempMemberCard.validType;
                        MemberInfo.ticketCent = fCent.ToString();
                        MemberInfo.totalCent = TotalCent.ToString(); 
                    }

                    string sSaleTime;

                    try
                    {
                       sSaleTime = DbHelper.ExecuteTable($"select to_char(sale_time,'yyyy-mm-dd HH24:MI:SS') from sale where posno='{posNo}' and dealid={transId}").Rows[0][0].ToString();
                    }
                    catch (Exception)
                    {

                        sSaleTime = "";
                    }
                   
                    UniConfirmBackDealResult(result, transId, CrmBillId, sSaleTime,  //sSaleTime原为"",成功时记本次交易时间
                      MemberInfo, GoodList, ReturnCouponList, CanReturnCouponList, out confirmResult);

                }
                else
                {
                    result = -1;
                    msg = sFunc + " 失败:" + msg;
                    confirmResult.code = result;
                    confirmResult.text = msg;
                    throw new Exception(msg);
                }
            }
            finally
            {
                st.Stop();
            }
            return confirmResult;
        }

        public int UniConfirmBackDealResult(int iCode, int ErpTranID, int CrmTranID, string sPrompt,
            TDealMemberCard MemberInfo,
            List<Goods> GoodsList,
            List<TDealReturnCoupon> ReturnCouponList,
            List<TDealSaleMoneyLeft> CanReturnCouponList,
            out ConfirmBackDealResult desc)
        {
            int i = 0, result = -1;
            desc = new ConfirmBackDealResult();
            desc.code = iCode;
            desc.text = sPrompt;
            desc.erpTranID = ErpTranID;
            desc.crmTranID = CrmTranID;

            if (iCode == 0)
            {
                desc.MemberInfo = new TDealMemberCard();
                desc.GoodsList = new List<TTranGoods>();
                desc.ReturnCouponList = new List<TDealReturnCoupon>();
                desc.CanReturnCouponList = new List<TDealSaleMoneyLeft>();

                AssignDealMemberCard(MemberInfo, ref desc.MemberInfo);

                TTranGoods TranGood = new TTranGoods();
                for (i = 0; i < GoodsList.Count; i++)
                {
                    TranGood = new TTranGoods();
                    AssignTranGoods(GoodsList[i], ref TranGood);
                    desc.GoodsList.Add(TranGood);
                }

                for (i = 0; i < ReturnCouponList.Count; i++)
                {
                    TDealReturnCoupon ReturnCoupon = new TDealReturnCoupon();
                    AssignReturnCoupon(ReturnCouponList[i], ref ReturnCoupon);
                    desc.ReturnCouponList.Add(ReturnCoupon);
                }

                for (i = 0; i < CanReturnCouponList.Count; i++)
                {
                    TDealSaleMoneyLeft Item = new TDealSaleMoneyLeft();
                    AssignCanReturnCoupon(CanReturnCouponList[i], ref Item);
                    desc.CanReturnCouponList.Add(Item);
                }
            }
            return result;
        }

        public bool CheckSaveData(string posNo, List<Payment> sktPayments, ReqConfirmBackDeal ReqConfirm,
            ref string msg)
        {
            msg = "";
            int i = 0, j = 0, k = 0;
            double totalCash = 0,totalCoupon = 0;
            bool result = false;

            //1:判断1:数目是否为0,为0:错误
            if (sktPayments.Count() <= 0)
            {
                msg = "错误:收款台可有收款方式数目为空";
                return result;
            }
            if (ReqConfirm.paysList.Count() <= 0)
            {
                msg = "错误:输入付款数目为空";
                return result;
            }

            TTranPayments curPay, curPay2;
            Payment defPay;

            //2:检查是否:相同的付款方式用了两次:实现方式,两个循环,不同i,j时ID相同，即为相同记录
            for (i = 0; i < ReqConfirm.paysList.Count(); i++)
            {
                curPay = ReqConfirm.paysList[i];
                for (j = 0; j < ReqConfirm.paysList.Count(); j++)
                {
                    curPay2 = ReqConfirm.paysList[j];

                    if (curPay.Id == curPay2.Id)
                    {
                        if (i != j)
                        {
                            msg = "错误:付款方式列表中有相同记录[ID:" + curPay.Id + " " +
                                curPay.PayMoney + "--" + curPay2.PayMoney + "]";
                            return result;
                        }
                    }
                }
            }

            //3:判断券的数据，CZK的数据是否一致
            for (i = 0; i < ReqConfirm.paysList.Count(); i++)
            {
                totalCoupon = 0;
                curPay = ReqConfirm.paysList[i];
                //if (curPay.Id)
                for (j = 0; j < sktPayments.Count(); j++)
                {
                    defPay = sktPayments[j];

                    //当前是CZK 
                    if ((curPay.Id == defPay.Id) && (defPay.TypeCode == "2"))
                    {
                        if (ReqConfirm.cashCashList == null)
                        {
                            msg = "错误:付款方式是储值卡类型,但卡明细为空null[" + defPay.Name + "]";
                            return result;
                        }
                        if (ReqConfirm.cashCashList.Count() <= 0)
                        {
                            msg = "错误:付款方式是储值卡类型,但卡明细为空[" + defPay.Name + "]";
                            return result;
                        }
                        for (k = 0; k < ReqConfirm.cashCashList.Count(); k++)
                        {
                            totalCash = totalCash + ReqConfirm.cashCashList[k].useMoney;
                        }

                        if (totalCash != curPay.PayMoney)
                        {
                            msg = "错误:储值卡付款类型,付款金额不等于明细[" + curPay.PayMoney + "--" + totalCash + "]";
                            return result;
                        }
                    }
                    else if ((curPay.Id == defPay.Id) && (defPay.TypeCode == "3"))
                    {
                        if (ReqConfirm.couponsList == null)
                        {
                            msg = "错误:付款方式是优惠券类型,但券明细为空null[" + defPay.Name + "]";
                            return result;
                        }

                        if (ReqConfirm.couponsList.Count() <= 0)
                        {
                            msg = "错误:付款方式是优惠券类型,但券明细为空[" + defPay.Name + "]";
                            return result;
                        }

                        for (k = 0; k < ReqConfirm.couponsList.Count(); k++)
                        {
                            if (ReqConfirm.couponsList[k].PayID.Equals(curPay.Id.ToString()))
                                totalCoupon = totalCoupon + ReqConfirm.couponsList[k].OutOfPocketAmount;
                        }

                        if (totalCoupon != curPay.PayMoney)
                        {
                            msg = "错误:优惠券付款类型[" + curPay.Id + "],付款金额不等于明细[" + curPay.PayMoney + "--" + totalCoupon + "]";
                            return result;
                        }
                    }
                }
            }
            result = true;
            return result;
        }

        public bool BeforeSaveBackTransation(string sShop, string posNo, int iJlbh, int CrmBillId, List<CashCardDetails> cards, List<CouponDetails> Coupons,
            List<Payment> payList, ref List<Goods> GoodsList,
            out int CrmMoneyCardTransId, out int CrmCouponTransId, out double fCent, out string msg)
        {
            fCent = 0;
            CrmCouponTransId = 0;
            CrmMoneyCardTransId = 0;

            string sFunc = "<退款预提交> ";

            msg = "";

            try
            {
                if (!BackPrepare(posNo, CrmBillId, payList, ref GoodsList, out fCent, out msg))
                {
                    return false;
                }

                //CZK消费  
                  if (!SaveBackMoneyCard(sShop, posNo, iJlbh, CrmBillId, cards, out CrmMoneyCardTransId, out msg))
                  {
                      return false;
                  }  

                //券消费
                if (!SaveBackCoupons(posNo, CrmBillId, Coupons, out CrmCouponTransId, out msg))
                    return false;
            }
            catch (Exception e)
            {
                msg = "预提交出错:" + e.Message.ToString();
                return false;
            }

            return true;
        }

        public bool SaveBackMoneyCard(string storeCode, string posNo, int iJlbh, int CrmBillId, List<CashCardDetails> cards,
            out int CrmMoneyCardTransId, out string msg)
        {
            CrmMoneyCardTransId = 0;
            msg = "";
            string error = "";

            string sFunc = "保存退储值记录";
           // CommonUtils.WriteSKTLog(1, posNo, "<0104_预提交> <3.2.2.1> 数目:" + cards.Count);

            string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";

            bool isCZK2 = false;
            string sHeadName = "PosWebServiceSoap2", sUrl = "";

            string sPersonCode = "";
            DateTime accountDate = DateTime.Now.Date;

            try
            {
                if (!CancelMoneyCard(posNo, out msg))
                {
                    msg = "存在储值卡冲正信息，请处理后再进行储值卡交易！";
                    return false;
                }

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = CRMUSer;
                crmSoapHeader.Password = CRMPwd; 
              //  PosWebServiceSoapClient client;

              //  client = new PosWebServiceSoapClient();
                List<CashCardPayment> moneyCardList = new List<CashCardPayment>();
                double totalMoney = 0;

                for (int j = 0; j < cards.Count; j++)
                {
                    CashCardPayment payment = new CashCardPayment();
                    payment.PayMoney = MoneyToDouble(cards[j].useMoney);
                    payment.CardId = cards[j].cardId;
                    moneyCardList.Add(payment);
                    totalMoney += payment.PayMoney;
                }

                if (moneyCardList.Count == 0)
                {
                    return true;
                }
                CashCardPayment[] cashCardPayments = new CashCardPayment[moneyCardList.Count];
                moneyCardList.CopyTo(cashCardPayments);
                int transId;
                bool result;


                //   result = client.PrepareTransCashCardPayment2(crmSoapHeader, storeCode, posNo, iJlbh, 
                //sPersonCode, accountDate, cashCardPayments, out msg, out transId);

                //准备储值卡支付交易
                PrepareTransCashCardPayment2Request req = new PrepareTransCashCardPayment2Request();
                req.ABCSoapHeader = crmSoapHeader;
                req.storeCode = storeCode;
                req.posId = posNo;
                req.billId = iJlbh;
                req.cashier = sPersonCode;
                req.accountDate = accountDate;
                req.payments = cashCardPayments;

                PrepareTransCashCardPayment2Response res = PosAPI.PrepareTransCashCardPayment2(req);
                result = res.PrepareTransCashCardPayment2Result;
                msg = res.msg;
                transId = res.transId;

                if (result)
                {
                    //写冲正文件
                    if (!WriteCancelFile(posNo, transId, 0, totalMoney, 1, out msg))
                    {
                        Cancel(posNo, 1, transId, CrmBillId, totalMoney, out msg);
                        return false;
                    }

                     //   result = client.ConfirmTransCashCardPayment(crmSoapHeader, transId, 0, totalMoney, out msg);

                    //确认储值卡支付交易
                    ConfirmTransCashCardPaymentRequest reqC = new ConfirmTransCashCardPaymentRequest();
                    reqC.ABCSoapHeader = crmSoapHeader;
                    reqC.transId = transId;
                    reqC.transMoney = totalMoney;
                    reqC.serverBillId = 0;
                    ConfirmTransCashCardPaymentResponse resC = PosAPI.ConfirmTransCashCardPayment(reqC);
                    result = resC.ConfirmTransCashCardPaymentResult;
                    msg = resC.msg;

                    if (!result)
                    {
                        Cancel(posNo, 1, transId, CrmBillId, totalMoney, out error);
                        return false;
                    }
                    CrmMoneyCardTransId = transId;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }

        public bool CancelMoneyCard(string posNo, out string msg)
        {
            msg = "";
            try
            {
                int billId = 0;
                int transId = 0;
                double money = 0;
                ErrorMessage errorMessage;
                if (ReadTrans(posNo, 1, out billId, out transId, out money, out errorMessage))
                {
                    if (!Cancel(posNo, 1, billId, transId, money, out msg))
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }

        //保存退款
        public bool CheckOutBackTranToDatabase(string Shop, string sktNo, int iPersonID, int jlbh, int CrmBillId, string PromniDealID,
            Member member, List<Goods> goodsList, List<Payment> pays,List<CreditDetail> creditList,
            int CrmMoneyCardTransID, int CrmCouponTransId, string OldPosId, int OldErpTranId,
            out ErrorMessage message)
        {
            message = new ErrorMessage();

            string sMsg = "";
            string sOper = "保存退货交易:";
            bool result = false;


            //保存单据
            result = CheckSaveBackTranData(sktNo, goodsList, pays, ref sMsg);
            if (!result)
            {
                message.ErrorType = 3;
                message.Message = "保存时检查数据失败:" + sMsg;
                return false;
            }

            double mTotal = 0;
            mTotal = GetTotalMoney(goodsList);

            //保存ERP交易数据
            try
            {
                SaleRequest saleReq = new SaleRequest();

                //sale
                saleReq.posno = sktNo;
                saleReq.dealid = jlbh;
                saleReq.member_cardid = member.MemberId.ToString();
                saleReq.crm_recordid = CrmBillId;
                saleReq.cashierid = iPersonID;
                saleReq.posno_old = OldPosId;
                saleReq.dealid_old = OldErpTranId;

                saleReq.sale_amount = decimal.Parse(mTotal.ToString());

                saleReq.sale_time = DateTime.Now;
                saleReq.account_date = DateTime.Now.Date;

                //sale_goods
                List<GoodsResult> goodsLst = new List<GoodsResult>();

                for (int g = 0; g <= goodsList.Count - 1; g++)
                {
                    GoodsResult goodsOne = new GoodsResult();
                    goodsOne.sheetid = 0;
                    goodsOne.inx = g;
                    goodsOne.goodsid = goodsList[g].Id;
                    goodsOne.goodscode = goodsList[g].Code;
                    goodsOne.price = decimal.Parse(goodsList[g].Price.ToString());
                    goodsOne.quantity = float.Parse(goodsList[g].SaleCount.ToString());
                    goodsOne.sale_amount = decimal.Parse((goodsList[g].SaleMoney ).ToString());  //- goodsList[g].Discount
                    //wangkx 暂时折扣记为0
                    goodsOne.discount_amount = 0;// decimal.Parse(goodsList[g].Discount.ToString());
                    goodsOne.coupon_amount = decimal.Parse((goodsList[g].PreferentialMoney + goodsList[g].DecreasePreferential).ToString());
                    goodsOne.shopid = goodsList[g].ShopId;
                    goodsLst.Add(goodsOne);
                }

                saleReq.goodslist = goodsLst;

                //sale_pay
                List<PayResult> payLst = new List<PayResult>();

                for (int p = 0; p <= pays.Count - 1; p++)
                {
                    PayResult payOne = new PayResult();
                    payOne.payid = pays[p].Id;
                    payOne.amount = decimal.Parse(pays[p].PayedMoney.ToString());
                    payLst.Add(payOne);
                }

                saleReq.paylist = payLst;

                //sale_clerk
                List<ClerkResult> clerkLst = new List<ClerkResult>();
                ClerkResult clerkOne = new ClerkResult();

                clerkOne.sheetid = 0;
                clerkOne.clerkid = iPersonID;
                clerkLst.Add(clerkOne);

                saleReq.clerklist = clerkLst;

                //PayRecord

                if (!creditList.IsEmpty())
                {
                    List<PayRecord> payRcd = new List<PayRecord>();

                    for (int p = 0; p <= creditList.Count - 1; p++)
                    {
                        PayRecord payRcdOne = new PayRecord();
                        payRcdOne.inx = creditList[p].inx;
                        payRcdOne.payid = creditList[p].payid;
                        payRcdOne.cardno = creditList[p].cardno;
                        payRcdOne.bank = creditList[p].bank;
                        payRcdOne.bankid = creditList[p].bankid;
                        payRcdOne.amount = creditList[p].amount * (-1);
                        payRcdOne.serialno = creditList[p].serialno;
                        payRcdOne.refno = creditList[p].refno;
                        payRcdOne.opertime = creditList[p].opertime;
                        payRcd.Add(payRcdOne);
                    }

                    saleReq.payRecord = payRcd;
                }
                else
                {
                    saleReq.payRecord = null;
                }


                    Sale(saleReq);
                    DeleteCrmTrans(sktNo, CrmMoneyCardTransID, CrmCouponTransId);

            }
            catch (Exception e)
            {
                message.ErrorType = 1;
                message.Message = sOper + "预保存失败:" + e.Message.ToString();


                return false;
            }
            finally
            {
            }
            return true;
        }


        public static double GetTotalMoney(List<Goods> GoodList)
        {
            int i = 0;
            double result = 0;
            for (i = 0; i < GoodList.Count(); i++)
                result = result + GoodList[i].SaleMoney - GoodList[i].Discount;
            return result;
        }

        public string ErrorMessageToString(ErrorMessage errorMessage)
        {
            if (errorMessage.ErrorType == 1)
            {
                if (errorMessage.Message.IndexOf("ORA-03135", 0) > 0)
                    return "offline";
                else if (errorMessage.Message.IndexOf("ORA-03114", 0) > 0)
                    return "offline";
                else
                    return errorMessage.Message;
            }
            else if (errorMessage.ErrorType == 2)
            {
                return "";
            }
            else
            {
                return "offline";
            }
        }

        public bool BackPrepare(string posNo, int CrmBillId, List<Payment> payList, ref List<Goods> GoodsList, out double fCent, out string msg)
        {
            msg = "";
            fCent = 0;
            int i = 0, j = 0;
            string sFunc = "<ERP保存退款>";

            //新加输出
            int PayBackCouponVipId = 0;
            bool CouponPaid = false;
            string offerCouponVipCode = "";
            bool bNeedVipToOfferCoupon, bNeedBuyCent;

            string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";

            try
            {
                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = CRMUSer; 
                crmSoapHeader.Password = CRMPwd;
              //  PosWebServiceSoapClient client = new PosWebServiceSoapClient();

                RSaleBillPayment[] pays = new RSaleBillPayment[payList.Count];

                for (i = 0; i < payList.Count; i++)
                {
                    pays[i] = new RSaleBillPayment();
                    pays[i].PayMoney = MoneyToDouble(payList[i].PayedMoney);
                    pays[i].PayTypeCode = Convert.ToString(payList[i].Id);
                }

                RSaleBillArticleCent[] articleCents;
                RSaleBillArticleCoupon[] articleCoupons;
                RSaleBillArticleCoupon curArticle;
                OfferBackCoupon[] offerBackCoupon;
                CouponPayback[] payBackCoupons;

                int iCurLine = 0;
                int  iCouponType = -1;
                bool PrepareCheckOutResult;
                double fRate = 0, mYHJE = 0;

                //  PrepareCheckOutResult = client.PrepareCheckOutRSaleBill(crmSoapHeader, CrmBillId,
                //     pays, PayBackCouponVipId, CouponPaid, out msg, out fCent, out bNeedVipToOfferCoupon, out bNeedBuyCent,
                //     out offerCouponVipCode, out articleCents, out articleCoupons, out offerBackCoupon, out payBackCoupons);

                PrepareCheckOutRSaleBillRequest req = new PrepareCheckOutRSaleBillRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.serverBillId = CrmBillId;
                req.payments = pays;
                req.payBackCouponVipId = PayBackCouponVipId;
                req.couponPaid = CouponPaid;

                PrepareCheckOutRSaleBillResponse res = PosAPI.PrepareCheckOutRSaleBill(req);
                PrepareCheckOutResult = res.PrepareCheckOutRSaleBillResult;
                msg = res.msg;
                fCent = res.billCent;
                bNeedVipToOfferCoupon = res.needVipToOfferCoupon;
                bNeedBuyCent = res.needBuyCent;
                offerCouponVipCode = res.offerCouponVipCode;
                articleCents = res.articleCents;
                articleCoupons = res.articleCoupons;
                offerBackCoupon = res.offerBackCoupons;
                payBackCoupons = res.paybackCoupons;

                if (PrepareCheckOutResult)
                {
                    if (articleCoupons != null)
                    {
                        for (i = 0; i < articleCoupons.Length; i++)
                        {
                            curArticle = articleCoupons[i];
                            iCurLine = curArticle.ArticleInx;

                            for (j = 0; j < payList.Count(); j++)
                            {
                                if (curArticle.CouponType != payList[j].CouponId)
                                    continue;

                                fRate = 0;
                                break;
                            }

                            if (fRate > 1)
                                fRate = 0;
                            if (fRate < 0)
                                fRate = 0;

                            if ((iCurLine >= 0) && (iCurLine < GoodsList.Count))
                            {
                                mYHJE = curArticle.SharedMoney;
                                mYHJE = RoundMoney(mYHJE * (1 - fRate));
                                iCouponType = curArticle.CouponType;

                                GoodsList[iCurLine].OTherInt1 = GoodsList[iCurLine].OTherInt1 + mYHJE;
                                GoodsList[iCurLine].PreferentialMoney = GoodsList[iCurLine].PreferentialMoney + mYHJE;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }

            return true;
        }


        public bool SaveBackCoupons(string posNo, int CrmBillId, List<CouponDetails> cards, out int CrmCouponTransId, out string msg)
        {
            CrmCouponTransId = 0;
            msg = "";
            string error = "";

            try
            {
                // 判断
                if (!CancelCoupon(posNo, out msg))
                {
                    msg = "存在优惠券冲正信息，请处理后再进行优惠券交易！";
                    return false;
                }

                string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = CRMUSer;
                crmSoapHeader.Password = CRMPwd;
           //     PosWebServiceSoapClient client = new PosWebServiceSoapClient();

                List<CouponPayment> CouponList = new List<CouponPayment>();
                double totalMoney = 0;


                for (int j = 0; j < cards.Count; j++)
                {
                    CouponPayment payment = new CouponPayment();
                    payment.CouponType = cards[j].couponType;
                    payment.PayMoney = cards[j].useMoney;  ///CommonUtils.MoneyToDouble(cards[j].amount);
                    payment.VipId = cards[j].cardId;
                    payment.CouponType = cards[j].couponType;

                    CouponList.Add(payment);
                    totalMoney += payment.PayMoney;
                }

                if (CouponList.Count == 0)
                {
                    return true;
                }
                CouponPayment[] cashCardPayments = new CouponPayment[CouponList.Count];
                CouponList.CopyTo(cashCardPayments);

                int transId;
                bool result;

                //  result = client.PrepareTransCouponPayment(crmSoapHeader, CrmBillId, cashCardPayments, out msg, out transId);

                PrepareTransCouponPaymentRequest req = new PrepareTransCouponPaymentRequest();

                req.ABCSoapHeader = crmSoapHeader;
                req.serverBillId = CrmBillId;
                req.payments = cashCardPayments;

                PrepareTransCouponPaymentResponse res = PosAPI.PrepareTransCouponPayment(req);

                result = res.PrepareTransCouponPaymentResult;
                msg = res.msg;
                transId = res.transId;


                if (result)
                {
                    //写冲正文件  2:YHQ 操作类型
                    if (!WriteCancelFile(posNo, transId, 0, totalMoney, 2, out msg))
                    {
                        Cancel(posNo, 2, transId, CrmBillId, totalMoney, out msg);
                        return false;
                    }

                    //   result = client.ConfirmTransCouponPayment(crmSoapHeader, transId, CrmBillId, totalMoney, out msg);

                    ConfirmTransCouponPaymentRequest reqCTCP = new ConfirmTransCouponPaymentRequest();

                    reqCTCP.ABCSoapHeader = crmSoapHeader;
                    reqCTCP.transId = transId;
                    reqCTCP.serverBillId = CrmBillId;
                    reqCTCP.transMoney = totalMoney;

                    ConfirmTransCouponPaymentResponse resCTCP = PosAPI.ConfirmTransCouponPayment(reqCTCP);

                    result = resCTCP.ConfirmTransCouponPaymentResult;
                    msg = resCTCP.msg;


                    if (!result)
                    {
                        Cancel(posNo, 2, transId, CrmBillId, totalMoney, out error);
                        return false;
                    }

                    CrmCouponTransId = transId;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }

        public bool CheckSaveBackTranData(string posNo, List<Goods> GoodList, List<Payment> PayList,
            ref string msg)
        {
            msg = "";
           // string sValue = "";
            int i = 0;
            double mItemTotal = 0, mItemTotalDisc = 0, mGoodTotal = 0, mPayTotal = 0, mGoodTotalYHJE = 0, mPayTotalYHJE = 0;
            bool result = false;

            int iSKFSType_Yhq = 3; //券类型

            if (GoodList.Count() <= 0)
            {
                msg = "错误:输入商品数目为空";
                return result;
            }
            if (PayList.Count() <= 0)
            {
                msg = "错误:输入付款数目为空";
                return result;
            }

            for (i = 0; i < GoodList.Count(); i++)
            {
                if (GoodList[i].Id <= 0)
                {
                    msg = "错误:商品明细错误:ID值不能小于或者等于0[" + GoodList[i].Id + "]";
                    return result;
                }

                if (GoodList[i].Code.Equals(""))
                {
                    msg = "错误:商品明细错误:商品码值不能为空";
                    return result;
                }

                if (GoodList[i].DeptCode.Equals(""))
                {
                    msg = "错误:商品明细错误:商品部门码不能为空";
                    return result;
                }

                if ((GoodList[i].DeptId <= 0) && (GoodList[i].DeptCode.Equals("")))
                {
                    msg = "错误:商品明细错误:商品部门值不能小于或者等于0";
                    return result;
                }

                if (GoodList[i].Price <= 0)
                {
                    msg = "错误:商品明细错误:商品零售价不能小于或者等于0";
                    return result;
                }

                if (GoodList[i].SaleCount >= 0)
                {
                    msg = "错误:商品明细错误:[退货交易]商品数量不能大于或者等于0";
                    return result;
                }


                if (GoodList[i].SaleMoney >= 0)
                {
                    msg = "错误:商品明细错误:[退货交易]商品售价不能大于或者等于0";
                    return result;
                }

                mItemTotalDisc = GoodList[i].FrontDiscount + GoodList[i].BackDiscount +
                    GoodList[i].MemberDiscount + GoodList[i].DiscoaddDiscount + GoodList[i].ChangeDiscount;
                if (mItemTotalDisc != GoodList[i].Discount)
                {
                    msg = "错误:商品折扣错误:商品折扣不等于合计折扣3[" + GoodList[i].Discount + "--" + mItemTotalDisc + "]";
                    return result;
                }

                mItemTotal = RoundMoney(GoodList[i].Price * GoodList[i].SaleCount);
                if (mItemTotal != GoodList[i].SaleMoney)
                {
                    msg = "错误:商品售价错误:商品售价不等于合计数据[" + mItemTotal + "--" + GoodList[i].SaleMoney + "]";
                    return result;
                }

                mGoodTotal = mGoodTotal + GoodList[i].SaleMoney - GoodList[i].Discount;
                mGoodTotalYHJE = mGoodTotalYHJE + GoodList[i].PreferentialMoney + GoodList[i].DecreasePreferential;
            }


            for (i = 0; i < PayList.Count(); i++)
            {
                if (PayList[i].Id < 0)
                {
                    msg = "错误:付款明细错误:ID值不能小于0[" + PayList[i].Id + "]";
                    return result;
                }

                if (PayList[i].PayedMoney == 0)
                {
                    msg = "错误:付款明细错误:付款金额不能等于0[" + PayList[i].PayedMoney + "]";
                    return result;
                }

                //准备测试充正
                mPayTotal = mPayTotal + PayList[i].PayedMoney;
                if (PayList[i].PaymentType == iSKFSType_Yhq)
                    mPayTotalYHJE = mPayTotalYHJE + (PayList[i].PayedMoney * (1 - 0));//PayList[i].CashBL; 
            }

            if (mPayTotal != mGoodTotal)
            {
                msg = "错误:商品销售合计不等于付款合计[" + mGoodTotal + "--" + mPayTotal + "]";
                return result;
            }

            if (mPayTotalYHJE != mGoodTotalYHJE)
            {
                msg = "错误:商品优惠合计不等于优惠券付款合计[" + mGoodTotalYHJE + "--" + mPayTotalYHJE + "]";
                return result;
            }

            result = true;
            return result;
        }


        public bool WriteCancelFile(string posNo, int transId, int value1, double money, int type, out string msg)
        {
            msg = "";
            try
            {
                ErrorMessage errorMessage;
                if (!WriteTrans(posNo, type, transId, value1, money, out errorMessage))
                {
                    msg = ErrorMessageToString(errorMessage);
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }

        public bool WriteTrans(string posId, int transType, int billId, int v1, double v2, out ErrorMessage message)
        {
            string msg = "";
            bool result = false;
            message = new ErrorMessage();

            string sql = "insert into CRMJYBZ(SKTNO,CRMJYLX,JLBH,VAL1,VAL2 )"
                        + $" values('{posId}',{transType},{billId},{v1},{v2} )";
            try
            {
                using (var Tran = DbHelper.BeginTransaction())
                {
                    DbHelper.ExecuteNonQuery(sql);
                    Tran.Commit();
                }

                result = true;
            }
            catch (Exception e)
            {
                msg = "保存CRM备注信息失败:" + e;
                result = false;
                message.ErrorType = 1;
                message.Message = msg;
            }
            return result;
        }


        public bool CancelCoupon(string posNo, out string msg)
        {
            msg = "";
            try
            {
                int billId = 0;
                int transId = 0;
                double money = 0;
                ErrorMessage errorMessage;
                if (ReadTrans(posNo, 2, out billId, out transId, out money, out errorMessage))
                {
                    if (!Cancel(posNo, 2, billId, transId, MoneyToDouble(money), out msg))
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }


        public bool ReadTrans(string posId, int transType, out int billId, out int v1, out double v2, out ErrorMessage message)
        {
            message = new ErrorMessage();
            billId = 0;
            v1 = 0;
            v2 = 0;
            bool result = false;

            string sql = $"select JLBH,VAL1,VAL2 from CRMJYBZ where SKTNO = '{posId}' and CRMJYLX = {transType}";

            try
            {
                DataTable dt = DbHelper.ExecuteTable(sql);

                if (dt.IsNotNull())
                {
                    result = true;
                    billId = dt.Rows[0][0].ToString().ToInt();
                    v1 = dt.Rows[0][1].ToString().ToInt();
                    v2 = dt.Rows[0][2].ToString().ToDouble();
                }
            }
            catch (Exception e)
            {
                result = false;
                message.ErrorType = 1;
                message.Message = "读取CRM备注信息失败："+e.Message.ToString();
            }
            return result;
        }

        public bool Cancel(string posNo, int type, int transId, int crmBillId, double totalMoney, out string msg)
        {
            msg = "";

            string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";
            try
            {
                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = CRMUSer; 
                crmSoapHeader.Password = CRMPwd; 
             //   PosWebServiceSoapClient client;

                bool result = false;
                int ServerBillID = 0;

                if (type == 1)
                {

                    //   client = new PosWebServiceSoapClient();

                    //   result = client.CancelTransCashCardPayment(crmSoapHeader, transId, ServerBillID, totalMoney, out msg);

                    CancelTransCashCardPaymentRequest req = new CancelTransCashCardPaymentRequest();
                    req.ABCSoapHeader = crmSoapHeader;
                    req.serverBillId = ServerBillID;
                    req.transId = transId;
                    req.transMoney = totalMoney;

                    CancelTransCashCardPaymentResponse res = PosAPI.CancelTransCashCardPayment(req);

                    result = res.CancelTransCashCardPaymentResult;
                    msg = res.msg;

                    if (!result)
                    {
                        return false;
                    }
                    else
                    {
                        if (!DeleteCancel(posNo, type, transId, out msg))
                        {
                            return false;
                        }
                    }
                }
                else if (type == 2)
                {
                    //  client = new PosWebServiceSoapClient();
                    //  result = client.CancelTransCouponPayment(crmSoapHeader, transId, ServerBillID, totalMoney, out msg);

                    CancelTransCouponPaymentRequest req = new CancelTransCouponPaymentRequest();
                    req.ABCSoapHeader = crmSoapHeader;
                    req.serverBillId = ServerBillID;
                    req.transId = transId;
                    req.transMoney = totalMoney;

                    CancelTransCouponPaymentResponse res = PosAPI.CancelTransCouponPayment(req);

                    result = res.CancelTransCouponPaymentResult;
                    msg = res.msg;


                    if (!result)
                    {
                        return false;
                    }
                    else
                    {
                        if (!DeleteCancel(posNo, type, transId, out msg))
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }

        public bool DeleteCancel(string posNo, int type, int transId, out string msg)
        {
            msg = "";
            try
            {
                ErrorMessage errorMessage;
                if (!DeleteTrans(posNo, type, transId, out errorMessage))
                {
                    msg = ErrorMessageToString(errorMessage);
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }

        public bool DeleteTrans(string posId, int transType, int billId, out ErrorMessage message)
        {
            message = new ErrorMessage();


            string msg = "",sql="";

            if (billId > 0)
                sql = $"delete from CRMJYBZ where SKTNO='{posId}' and CRMJYLX={transType} AND JLBH={billId}";
            else
                sql = $"delete from CRMJYBZ where SKTNO='{posId}' and CRMJYLX={transType}";

            try
            {
                using (var Tran = DbHelper.BeginTransaction())
                {
                    DbHelper.ExecuteNonQuery(sql);
                    Tran.Commit();
                }
            }
            catch (Exception e)
            {
                msg = "删除CRM备注信息出错" + e.Message;
                message.Message = msg;
                return false;
            }
            return true;
        }


        public bool CheckSaveData(string posNo, List<Goods> GoodList, List<Payment> PayList,
            ref string msg)
        {
            msg = "";
            string sValue = "";
            int i = 0;
            double mItemTotal = 0, mItemTotalDisc = 0, mGoodTotal = 0, mPayTotal = 0, mGoodTotalYHJE = 0, mPayTotalYHJE = 0;
            bool result = false;
            if (GoodList.Count() <= 0)
            {
                msg = "错误:输入商品数目为空";
                return result;
            }
            if (PayList.Count() <= 0)
            {
                msg = "错误:输入付款数目为空";
                return result;
            }

            int iSKFSType_Yhq = 3; //纸券的类型


            for (i = 0; i < GoodList.Count(); i++)
            {
                if (GoodList[i].Id <= 0)
                {
                    msg = "错误:商品明细错误:ID值不能小于或者等于0[" + GoodList[i].Id + "]";
                    return result;
                }

                if (GoodList[i].Code.Equals(""))
                {
                    msg = "错误:商品明细错误:商品码值不能为空";
                    return result;
                }

                if (GoodList[i].DeptCode.Equals(""))
                {
                    msg = "错误:商品明细错误:商品部门码不能为空";
                    return result;
                }

                if ((GoodList[i].DeptId <= 0) && (GoodList[i].DeptCode.Equals("")))
                {
                    msg = "错误:商品明细错误:商品部门值不能小于或者等于0";
                    return result;
                }

                if (GoodList[i].Price <= 0)
                {
                    msg = "错误:商品明细错误:商品零售价不能小于或者等于0";
                    return result;
                }

                if (GoodList[i].SaleCount <= 0)
                {
                    msg = "错误:商品明细错误:商品数量不能小于或者等于0";
                    return result;
                }


                if (GoodList[i].SaleMoney <= 0)
                {
                    msg = "错误:商品明细错误:商品售价不能小于或者等于0";
                    return result;
                }

                mItemTotalDisc = GoodList[i].FrontDiscount +
                    GoodList[i].BackDiscount + GoodList[i].MemberDiscount +
                    GoodList[i].DiscoaddDiscount + GoodList[i].ChangeDiscount;
                if (mItemTotalDisc != GoodList[i].Discount)
                {
                    msg = "错误:商品折扣错误:商品折扣不等于合计折扣 1[" + GoodList[i].Discount + "--" + mItemTotalDisc + "]";
                    return result;
                }

                mItemTotal = RoundMoney(GoodList[i].Price * GoodList[i].SaleCount);
                if (mItemTotal != GoodList[i].SaleMoney)
                {
                    msg = "错误:商品售价错误:商品售价不等于合计数据[" + mItemTotal + "--" + GoodList[i].SaleMoney + "]";
                    return result;
                }

                mGoodTotal = mGoodTotal + GoodList[i].SaleMoney - GoodList[i].Discount;
                mGoodTotalYHJE = mGoodTotalYHJE + GoodList[i].PreferentialMoney + GoodList[i].DecreasePreferential;
            }


            for (i = 0; i < PayList.Count(); i++)
            {
                if (PayList[i].Id < 0)
                {
                    msg = "错误:付款明细错误:ID值不能小于0[" + PayList[i].Id + "]";
                    return result;
                }

                if (PayList[i].PayedMoney == 0)
                {
                    msg = "错误:付款明细错误:付款金额不能等于0[" + PayList[i].PayedMoney + "]";
                    return result;
                }

                //充正
                mPayTotal = mPayTotal + PayList[i].PayedMoney;
                if (PayList[i].PaymentType == iSKFSType_Yhq)
                    mPayTotalYHJE = mPayTotalYHJE + PayList[i].PayedMoney;
            }


            if (mPayTotal != mGoodTotal)
            {
                msg = "错误:商品销售合计不等于付款合计[" + mGoodTotal + "--" + mPayTotal + "]";
                return result;
            }

            if (mPayTotalYHJE != mGoodTotalYHJE)
            {
                msg = "错误:商品优惠合计不等于优惠券付款合计[" + mGoodTotalYHJE + "--" + mPayTotalYHJE + "]";
                return result;
            }


            result = true;
            return result;
        }


        private void DeleteCrmTrans(string posId, int Id, int iCouble)
        {
            string sql;
            if (Id > 0)
            {
                sql = $"delete from CRMJYBZ where SKTNO = '{posId}' and JLBH = {Id}";
                DbHelper.ExecuteNonQuery(sql);
            }

            if (iCouble > 0)
            { 
                sql = $"delete from CRMJYBZ where SKTNO = '{posId}' and JLBH = {iCouble}";
                DbHelper.ExecuteNonQuery(sql);
            }
        }

        public int GetGoodsShopId(int goodsid)
        {
            string sql = $"select shopid from goods_shop where goodsid={goodsid}";

            DataTable dt = DbHelper.ExecuteTable(sql);

            if (dt.IsNotNull())
                return dt.Rows[0][0].ToString().ToInt();
            else
                return 0;
        }

        public bool SaveMoneyCard(string storeCode, string posNo, int iJlbh, int CrmBillId, List<CashCardDetails> cards,
            out int CrmMoneyCardTransId, out string msg)
        {
            CrmMoneyCardTransId = 0;
            msg = "";
            string error = "";

            string sPersonCode = "";
            DateTime accountDate = DateTime.Now.Date;

            try
            {
                if (!CancelMoneyCard(posNo, out msg))
                {
                    msg = "存在储值卡冲正信息，请处理后再进行储值卡交易！";
                    return false;
                }

                ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
                crmSoapHeader.UserId = "CRMUSER";
                crmSoapHeader.Password = "CRMUSER";


                //  PosWebServiceSoapClient client;
                //  client = new PosWebServiceSoapClient();


                List<CashCardPayment> moneyCardList = new List<CashCardPayment>();
                double totalMoney = 0;


                for (int j = 0; j < cards.Count; j++)
                {
                    CashCardPayment payment = new CashCardPayment();
                    payment.PayMoney = cards[j].useMoney;
                    payment.CardId = cards[j].cardId;
                    moneyCardList.Add(payment);
                    totalMoney += payment.PayMoney;
                }

                if (moneyCardList.Count == 0)
                {
                    return true;
                }
                CashCardPayment[] cashCardPayments = new CashCardPayment[moneyCardList.Count];
                moneyCardList.CopyTo(cashCardPayments);
                int transId;
                bool result;

                //CRM预提交
                //  result = client.PrepareTransCashCardPayment2(crmSoapHeader, storeCode, posNo, iJlbh, sPersonCode, accountDate, cashCardPayments, out msg, out transId);

                PrepareTransCashCardPayment2Request req = new PrepareTransCashCardPayment2Request();

                req.ABCSoapHeader = crmSoapHeader;
                req.storeCode = storeCode;
                req.posId = posNo;
                req.billId = iJlbh;
                req.cashier = sPersonCode;
                req.accountDate = accountDate;
                req.payments = cashCardPayments;

                PrepareTransCashCardPayment2Response res = PosAPI.PrepareTransCashCardPayment2(req);

                result = res.PrepareTransCashCardPayment2Result;
                msg = res.msg;
                transId = res.transId;

                if (result)
                {

                    //写冲正文件
                    if (!WriteCancelFile(posNo, transId, 0, totalMoney, 1, out msg))
                    {
                        Cancel(posNo, 1, transId, CrmBillId, totalMoney, out msg);
                        return false;
                    }

                    //提交
                    // result = client.ConfirmTransCashCardPayment(crmSoapHeader, transId, 0, totalMoney, out msg);

                    ConfirmTransCashCardPaymentRequest reqC = new ConfirmTransCashCardPaymentRequest();
                    reqC.ABCSoapHeader = crmSoapHeader;
                    reqC.transId = transId;
                    reqC.serverBillId = 0;
                    reqC.transMoney = totalMoney;

                    ConfirmTransCashCardPaymentResponse resC = PosAPI.ConfirmTransCashCardPayment(reqC);
                    result = resC.ConfirmTransCashCardPaymentResult;
                    msg = resC.msg;

                    if (!result)
                    {
                        //保存CZK失败 冲正
                        Cancel(posNo, 1, transId, CrmBillId, totalMoney, out error);
                        return false;
                    }
                    CrmMoneyCardTransId = transId;
                }
                else
                {
                    // 保存CZK失败
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
            return true;
        }


        public bool CalculateDecDisc_CRM(string posid, int Yqje, int iCrmBillID, ref List<Goods> goodsList, out string msg)
        {
            msg = "";
            bool result = false;
            int i = 0;
            double mTotal = 0;
            List<Payment> ErpPays = new List<Payment>();

            for (i = 0; i < goodsList.Count(); i++)
            {
                mTotal = mTotal + goodsList[i].SaleMoney - goodsList[i].Discount;
            }

            //  CommonUtils.WriteSKTLog(1, posid, "计算满减<1.1> 按CRM方式: 商品金额:" + mTotal);


            Payment Item = new Payment();
            Item.Id = 1;
            Item.PayedMoney = mTotal;
            ErpPays.Add(Item);

            result = CalDecMoney(posid, iCrmBillID, ErpPays, ref goodsList, ref msg);

            return result;
        }

        public bool CalDecMoney(string posId, int CrmBillId, List<Payment> ErpPayList,
           ref List<Goods> GoodsList, ref string msg)
        {
            msg = "";
            double decMoney = 0;
            bool result = false, flag = false;
            int i = 0, iCurLine = 0, iPayCount = ErpPayList.Count();
            double mDec = 0;

            //   CommonUtils.WriteSKTLog(1, posId, "CRM满减<1.1>:收款方式列表: " + iPayCount);

            string CRMUSer = "CRMUSER", CRMPwd = "CRMUSER";


            ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
            crmSoapHeader.UserId = CRMUSer;
            crmSoapHeader.Password = CRMPwd;

            //  CommonUtils.WriteSKTLog(1, posId, "CRM满减<1.2>:准备创建连接 ");
            //  PosWebServiceSoapClient client = new PosWebServiceSoapClient();

            //  CommonUtils.WriteSKTLog(1, posId, "CRM满减<1.3>:准备创建变量 ");

            RSaleBillArticleDecMoney[] RSaleBillArticleDecMoneys;

            //RSaleBillPayment[] CrmPayments = new RSaleBillPayment[iPayCount];

            List<RSaleBillPayment> CrmPayments = new List<RSaleBillPayment>(); ;


            // CommonUtils.WriteSKTLog(1, posId, "CRM满减<1.5>:iPayCount ");
            for (i = 0; i < iPayCount; i++)
            {
                RSaleBillPayment CrmPayItem = new RSaleBillPayment();
                //CrmPayItem.PayMoney = (double)(ErpPayList[i].PayedMoney / 100.0);//CommonUtils.MoneyToDouble(

                CrmPayItem.PayMoney = MoneyToDouble(ErpPayList[i].PayedMoney);

                CrmPayItem.PayTypeCode = ErpPayList[i].Id.ToString();

                //   CommonUtils.WriteSKTLog(1, posId, "CRM满减<1.6.1>:付款方式ID: " + CrmPayItem.PayTypeCode +
                //      " CRM金额:" + CrmPayItem.PayMoney.ToString() + " 付款金额:" + ErpPayList[i].PayedMoney.ToString());
                CrmPayments.Add(CrmPayItem);
            }



            try
            {
                //  CommonUtils.WriteSKTLog(1, posId, "CRM满减<2.1>: 发送CRM请求");

                CalcRSaleBillDecMoneyRequest req = new CalcRSaleBillDecMoneyRequest();
                req.ABCSoapHeader = crmSoapHeader;
                req.serverBillId = CrmBillId;
                req.payments = CrmPayments.ToArray();

                CalcRSaleBillDecMoneyResponse res = PosAPI.CalcRSaleBillDecMoney(req);
                flag = res.CalcRSaleBillDecMoneyResult;


                // flag = client.CalcRSaleBillDecMoney(crmSoapHeader, CrmBillId, CrmPayments.ToArray(), out msg,
                //     out decMoney, out RSaleBillArticleDecMoneys);

                //  CommonUtils.WriteSKTLog(1, posId, "CRM满减<2.2>: 结束CRM请求");
                if (flag == true)
                {
                    // CommonUtils.WriteSKTLog(1, posId, "CRM满减<2.3.1>: 操作成功 共满减:" + decMoney);
                    if (res.articleDecMoneys != null)
                    {
                        for (i = 0; i < res.articleDecMoneys.Count(); i++)
                        {
                            iCurLine = res.articleDecMoneys[i].ArticleInx;
                            if ((iCurLine >= 0) && (iCurLine < GoodsList.Count))
                            {
                                mDec = res.articleDecMoneys[i].DecMoney;
                                GoodsList[iCurLine].DecreaseDiscount = mDec;

                                // CommonUtils.WriteSKTLog(1, posId, "CRM满减<2.3.2>: 商品:" + GoodsList[iCurLine].Code + " 满减:" + GoodsList[iCurLine].DecreaseDiscount);
                            }
                        }
                    }
                    // CommonUtils.WriteSKTLog(1, posId, "CRM满减<2.5>: 操作成功");
                }
            }
            catch (Exception e)
            {
                msg = "CRM报错: " + e.Message.ToString() + "" + msg;
                //   CommonUtils.WriteSKTLog(2, posId, "CRM满减<4.1> " + msg);
                flag = false;
            }

            return result;
        }


    }


}
