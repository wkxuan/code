using System;
using System.Collections.Generic;
using z.ERP.Entities.Service.Pos;
using z.Extensions;
using System.Data;
using System.Linq;
using z.ERP.API.PosServiceAPI;
using z.ServiceHelper;
using System.Diagnostics;

namespace z.ERP.Services
{


    public class PosService : ServiceBase
    {
        public const int UniCode_Json = 0;
        public const int UniCode_XML = 1;

        private const int Member_CondType_CDNR = 0;
        private const int Member_CondType_HYID = 1;
        private const int Member_CondType_HYK_NO = 2;
        private const int Member_CondType_SJHM = 3;
        private const int Member_CondType_SFZBH = 4;
        private const int Member_CondType_qrcode = 5;
        //private const int Member_CondType_PHONE = 5;

        private const string Member_CondTypeName_Track = "CardTrack";
        private const string Member_CondTypeName_SFZ = "IDCard";
        private const string Member_CondTypeName_HZ = "Passport";
        private const string Member_CondTypeName_CardNo = "CardNo";
        private const string Member_CondTypeName_HYID = "MemberID";//mobilPhone
        private const string Member_CondTypeName_Phone = "mobilePhone";//mobilPhone  qrcode
        private const string Member_CondTypeName_qrcode = "qrcode";

        internal PosService()
        {

        }

        public void a()
        {

        }


        public LoginConfigInfo GetConfig()
        {
            string sql = " select S.BRANCHID,P.SHOPID,P.CODE SHOPCODE,P.NAME SHOPNAME,G.PID,G.KEY"
                       + " from STATION S, POSO2OWFTCFG G, SHOP P"
                       + " where S.STATIONBH = G.POSNO(+)"
                       + " AND S.SHOPID = P.SHOPID(+)"
                       + $" AND S.STATIONBH = '{employee.PlatformId}'";

            LoginConfigInfo lgi = DbHelper.ExecuteOneObject<LoginConfigInfo>(sql);
            return lgi;

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

            string sqlPay = $"select payid,amount,remarks from {strTable}sale_pay";
            sqlPay += $" where posno='{posNo}' and dealid={filter.dealid}";

            string sqlClerk = $"select sheetid,clerkid from {strTable}sale_clerk";
            sqlClerk += $" where posno='{posNo}' and dealid={filter.dealid}";

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

                sqlarr[i] = "insert into sale_pay(posno,dealid,payid,amount,remarks)";
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
                sqlarr[i] = "insert into sale_clerk(posno,dealid,sheetid,clerkid)";
                sqlarr[i] += $"values('{posNo}',{request.dealid},{request.clerklist[j].sheetid},{request.clerklist[j].clerkid})";
                j++;
            }

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

         /*   if (insertCount != 1 + goodsCount + payCount + clerkCount + goodsCount * payCount)
            {
                throw new Exception("写入数据不完整!");
            } */
        }

        public SaleSummaryResult GetSaleSummary(SaleSummaryFilter filter)
        {
            string sql = $"select s.posno,s.dealid,decode(sign(s.sale_amount),0,0,1,0,-1,1) returnflag,"
                   + "       s.sale_time,p.payid,y.name payname,y.type paytype, p.amount"
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
                  + "       s.sale_time,p.payid,y.name payname,y.type paytype, p.amount"
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



        public CalcAccountsPayableResult CalcAccountsPayable(ReqGetGoods reqMth)
        {
            if (reqMth == null)
            {
                throw new Exception("参数为空");
            }

            CalcAccountsPayableResult payableResult = new CalcAccountsPayableResult();

            MemberCard vipcard = new MemberCard();
            CashCardDetails cashCard = new CashCardDetails();
            List<Goods> GoodsList = new List<Goods>();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();
            List<Payment> DevicePayments = new List<Payment>();

            // int iDataType = UniCode_Json;
            int iHTH = 0;
            string Shop = "0001";
            string posNo = employee.PlatformId, userCode = employee.Code,
                 cardCodeToCheck = "", verifyCode = "", password = "", 
                 CondValue = "", sVIPCode = "", sDeptCode = "";
            int i = 0, j = 0, transId = 0, shopId = 0, deptid = 0,
                  CrmBillId = 0, backType = 0, bulkGoodsType = 0, iVIPID = -1;

            string Operator = employee.Id;
            string sTitle = "SysVer", sVer = "", message ="",msg="";

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


                GetVipCardRequest request = new GetVipCardRequest();

                request.condType = int.Parse(reqMth.validType); 
                request.condValue = reqMth.ValidID;

                GetVipCardResponse res = PosAPI.GetVipCard(request);

                if (!res.GetVipCardResult)
                {
                    throw new Exception(res.msg);
                }

                VipCard vip_card = new VipCard();

                vip_card = res.vipCard;

                if (vip_card != null)
                    AssignLocalToPublic_Member(vip_card, out vipcard);

                //  ProcCRM.ProcCRMFunc.GetMemberInfo(iMemberType, reqMth.ValidID, out vipcard, out msg);//iMemberType Member_CondType_HYK_NO


                //2.3取CZK信息
                /* if (vipcard.id > 0)
                 {
                     cardCodeToCheck = ""; verifyCode = ""; password = ""; CondValue = Convert.ToString(vipcard.id);
                     ProcCRM.ProcCRMFunc.GetCashCardInfo(Device, 1, CondValue, Shop, cardCodeToCheck, verifyCode, password,
                         out cashCard, out msg);

                     int iPayID = -1;
                     string sPayName = "";
                     GetCashCardPayID(Device, vipcard.memberType, ref iPayID, ref sPayName, out msg);
                     if (iPayID > 0)
                     {
                         cashCard.payID = iPayID;
                     }
                 }*/

                //2.2:取商品

             //   CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2> 第二步:查询商品 店:" + shopId);
             //   result = -1;

                bool bRslt = false;
                string ItemCode = "";
             //   ErrorMessage message = new ErrorMessage();

                string sTitle1 = "FullCutType", sType = "";
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

                    if (reqMth.goodsList[i].deptID == null)
                        deptid = 0;
                    else
                        deptid = reqMth.goodsList[i].deptID;

                    //2018.04.23_1:处理负库存标记
                  //  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.1.1> 第二步:查询商品 部门代码:" + sDeptCode +
                  //      " 部门ID:" + deptid);
                  //  bRslt = DoGetGoodsInfo(ItemCode, sDeptCode, deptid, backType, bulkGoodsType, Shop, posNo,
                  //        out goods, out message);

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

                    //goods.DeptCode = reqMth.goodsList[i].
                    if ((goods.Price == 0) && (reqMth.goodsList[i].price != 0))
                        goods.Price = reqMth.goodsList[i].price;

                    if ((goods.SaleMoney == 0) && (goods.Price != 0))
                        goods.SaleMoney = RoundMoney(goods.SaleCount * goods.Price);

                    //2018.04.28 负库存销售标记
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
                  //  result = -1;
                    if (!ComputeVipDiscount(Shop, posNo, vipcard, GoodsList, out msg))
                    {
                     //   result = RsltCode_Wrong_NoDef;
                        msg = "计算商品售价失败:" + msg;
                     //   return result;
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


              //  bool bResult = false;
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

                transId = int.Parse(GetLastDealid().ToString()) + 1 ;

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
              //      bResult = CalculateDecDisc_CRM(posNo, mTotalYQJE, CrmBillId, ref GoodsList, out msg);

              //  CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<2.5.1> 查询返券:会员ID:" + vipcard.id);
                if (vipcard.id > 0)
                {
                    string sCountPwd = "NOCHECKPWD";
                    cardCodeToCheck = ""; verifyCode = ""; password = ""; CondValue = Convert.ToString(vipcard.id);
                    if(GetVipCoupon(1, CondValue, Shop, cardCodeToCheck, verifyCode,
                        CrmBillId,out ListCoupon, out iVIPID, out sVIPCode, out msg))

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

        public int UniCalcAccountsPayable(string posNo, int iCode, int transId, int crmBillId, string sPrompt,
            List<Goods> GoodsList,
             MemberCard vipcard, CashCardDetails cashCard,
            List<CouponDetails> ListCoupon,
            out CalcAccountsPayableResult desc)
        {

        //    CommonUtils.WriteSKTLog(1, posNo, "计算销售价格<3.2.1> ");

            int i = 0, mTotalCanUse = 0;
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

            try
            {

                GetVipCouponToPayRequest req = new GetVipCouponToPayRequest();

                req.condType = iCondType;
                req.condValue = sCondValue;
                req.cardCodeToCheck = sCheck;
                req.verifyCode = sVerify;
                req.storeCode = sStoreCode;
                req.serverBillID = iServerID;

                GetVipCouponToPayResponse rep = PosAPI.GetVipCouponToPay(req);


                //   result = client.GetVipCouponToPay(crmSoapHeader, iCondType, sCondValue, sCheck, sVerify, sStoreCode,
                //       iServerID, out msg, out iVIPID, out sVIPCode, out PayCoupon, out payLimits);

                result = rep.GetVipCouponToPayResult;
                msg = rep.msg;

                if (!result) return result;

                PayCoupon = rep.coupons;
                payLimits = rep.payLimits;

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
                                CouponItem.amount = Convert.ToInt32(CurCoupon.Balance * 100);
                                CouponItem.amountCanUse = Convert.ToInt32(CurLimit.LimitMoney * 100);

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

        public static int RoundMoney(double money)
        {
            if (money > 0)
            {
                return (int)(money + 0.5);
            }
            else if (money == 0)
            {
                return 0;
            }
            else
            {
                return (int)(money - 0.5);
            }
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

            try
            {
              //  ABCSoapHeader crmSoapHeader = new ABCSoapHeader();
              //  crmSoapHeader.UserId = CRMUSer;// "AAA";
              //  crmSoapHeader.Password = CRMPwd; // "123";
              //  PosWebServiceSoapClient client = client = new PosWebServiceSoapClient();

                bool GetVipDiscResult = false;

                //   CommonUtils.WriteSKTLog(11, posNo, "计算会员折扣<2.1> 准备发送折扣请求");

                GetArticleVipDiscRequest req = new GetArticleVipDiscRequest();

                req.vipID = vipcard.id;
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
                            int disc = 0;
                            int mTotalSPZK = GoodsList[i].FrontDiscount + GoodsList[i].BackDiscount;
                            disc = RoundMoney(((GoodsList[i].Price * GoodsList[i].SaleCount) - mTotalSPZK) * (1 - articleVipDisc[i].DiscRate));
                            GoodsList[i].MemberDiscount = disc;
                            GoodsList[i].MemberOffRate = articleVipDisc[i].DiscRate; //付会员折扣
                                                                                     //

                         //   CommonUtils.WriteSKTLog(11, posNo, "计算会员折扣<2.3> [2018.01.20] " +
                         //       "会员折扣=[( 零售 X 数量 ) -  前台折 - 后台折 ] X (1-折扣率)   " +
                         //     disc + " = [( " + GoodsList[i].Price + " X " + GoodsList[i].SaleCount + ") - " +
                         //     GoodsList[i].FrontDiscount + " - " + GoodsList[i].BackDiscount + ") X ( 1 -  " + articleVipDisc[i].DiscRate + "))"
                         //       );

                            int mDisc1 = GoodsList[i].MemberDiscount, mDisc2 = GoodsList[i].MemberDiscount;


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

                        article.DeptCode = GoodsList[i].DeptCode;

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

                int crmBillId;
                double decMoney;
                RSaleBillArticleDecMoney[] articleDecMoneys;
                RSaleBillArticlePromFlag[] articlePromFlags;

                bool saveResult = false;


                //  CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.5>" + " 开始调用上传接口:");

                SaveRSaleBillArticlesRequest req = new SaveRSaleBillArticlesRequest();
                req.billHead = billHead;
                req.billArticles = articles;

                SaveRSaleBillArticlesResponse rep = PosAPI.SaveRSaleBillArticles(req);

                saveResult = rep.SaveRSaleBillArticlesResult;

               // saveResult = client.SaveRSaleBillArticles(crmSoapHeader, billHead, articles, out msg, out crmBillId,
               //     out decMoney, out articleDecMoneys, out articlePromFlags);

              //  CommonUtils.WriteSKTLog(1, posNo, "准备上传商品<5.1.6>" + " 完成调用上传接口:");

                if (saveResult)
                {
                    crmBillId = rep.serverBillID;
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

        /**  public int MUniGetCardPayable(string Shop, string PersonCode, string sInput,
              out GetCardPayableResult desc, out string msg)
          {
              msg = "";
              int result = -1;
              result = UniGetCardPayable(Shop, PersonCode, sInput, out desc, out msg);
              return result;
          }

          public int UniGetCardPayable(string Shop, string Operator, string sInput,
              out GetCardPayableResult desc, out string msg)
          {
              int result = -1;
              GetCardPayableResult payableResult = new GetCardPayableResult();
              result = DoUniGetCardPayable(Shop, Operator, sInput, out desc, out msg);

              return result;
          } **/

        //   public int DoUniGetCardPayable(string Shop,string Operator, string sInput,
        //       out GetCardPayableResult payableResult, out string msg)

        public GetCardPayableResult GetCardPayable(ReqGetCardPayable reqMth)
        {
            string msg = "";
            GetCardPayableResult payableResult = new GetCardPayableResult();

            MemberCard vipcard = new MemberCard();
            CashCardDetails cashCard = new CashCardDetails();
            List<CouponDetails> ListCoupon = new List<CouponDetails>();
            List<Payment> DevicePayments = new List<Payment>();

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


                GetVipCardRequest request = new GetVipCardRequest();

                request.condType = condType;
                request.condValue = conValue;

                GetVipCardResponse res = PosAPI.GetVipCard(request);

                if(!res.GetVipCardResult)
                {
                    throw new Exception(res.msg);
                }

                VipCard vip_card = new VipCard();

                vip_card = res.vipCard;

                if (vip_card != null)
                    AssignLocalToPublic_Member(vip_card, out vipcard);


                //计算券
                if (vipcard.id > 0)
                {

                    GetVipCouponToPayRequest couponRequest = new GetVipCouponToPayRequest();

                    couponRequest.condType = 2;
                    couponRequest.condValue = Convert.ToString(vipcard.memberNo);
                    couponRequest.cardCodeToCheck = "";
                    couponRequest.verifyCode = "";
                    couponRequest.storeCode = "0001";
                    couponRequest.serverBillID = reqMth.crmTranID;

                    GetVipCouponToPayResponse couponRes = PosAPI.GetVipCouponToPay(couponRequest);

                   if (couponRes.GetVipCouponToPayResult)
                        DoGetCouponPayments(DevicePayments, ref ListCoupon, ref msg);
                    else
                    {
                        throw new Exception(couponRes.msg);
                    }  
                    
                    
                }
                else
                {
                    GetVipCouponToPayRequest couponRequest = new GetVipCouponToPayRequest();

                    couponRequest.condType = 1;
                    couponRequest.condValue = Convert.ToString(vipcard.id);
                    couponRequest.cardCodeToCheck = "";
                    couponRequest.verifyCode = "";
                    couponRequest.serverBillID = reqMth.crmTranID;

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
            //  desc.mobilePhone = sour.Mobile;
            desc.sex = "";
            desc.validType = "";
            desc.validID = "";
            desc.typeLevel = 0;


            desc.sex = "";
            // if (sour.SexType == 1)
            //     desc.sex = "Female";
            // else if (sour.SexType == 0)
            //     desc.sex = "Male"; 
            desc.memberTypeName = sour.CardTypeName;

            result = true;
            return result;
        }

        public static int DoGetCouponPayments(List<Payment> DevicePayments, ref List<CouponDetails> ListCoupon, ref string msg)
        {
            int result = -1;
            int i = 0, j = 0;

            if (ListCoupon == null) return -1;
            if (ListCoupon.Count <= 0) return -1;

            if (DevicePayments == null) return -1;
            if (DevicePayments.Count <= 0) return -1;

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

        public static int UniGetCardPayable(int iCode, int transId, int crmBillId, string sPrompt,
           MemberCard vipcard, CashCardDetails cashCard,
           List<CouponDetails> ListCoupon,
           out GetCardPayableResult desc)
        {

            int i = 0, mTotalCanUse = 0;
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



        /**     public VipCard GetVipCard(GetVipCardRequest request)
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

        public ArticleVipDisc[] GetArticleVipDisc(GetArticleVipDiscRequest request)
        {
            GetArticleVipDiscResponse res = PosAPI.GetArticleVipDisc(request);
            if (res.GetArticleVipDiscResult)
            {
                return res.discs;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

        public Coupon[] GetVipCoupon(GetVipCouponRequest request)
        {
            GetVipCouponResponse res = PosAPI.GetVipCoupon(request);
            if (res.GetVipCouponResult)
            {
                return res.coupons;
            }
            else
            {
                throw new Exception(res.msg);
            }
            
        }

        public bool GetVipCouponToPay(GetVipCouponToPayRequest request)
        {
            GetVipCouponToPayResponse res = PosAPI.GetVipCouponToPay(request);
            if(res.GetVipCouponToPayResult)
            {
                return res.GetVipCouponToPayResult;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

        public int PrepareTransCouponPayment(PrepareTransCouponPaymentRequest request)
        {
            PrepareTransCouponPaymentResponse res = PosAPI.PrepareTransCouponPayment(request);
            if (res.PrepareTransCouponPaymentResult)
            {
                return res.transID;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

        public int PrepareTransCouponPayment2(PrepareTransCouponPayment2Request request)
        {
            PrepareTransCouponPayment2Response res = PosAPI.PrepareTransCouponPayment2(request);
            if (res.PrepareTransCouponPayment2Result)
            {
                return res.transID;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

        public bool ConfirmTransCouponPayment(ConfirmTransCouponPaymentRequest request)
        {
            ConfirmTransCouponPaymentResponse res = PosAPI.ConfirmTransCouponPayment(request);
            if (res.ConfirmTransCouponPaymentResult)
            {
                return res.ConfirmTransCouponPaymentResult;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

        public bool CancelTransCouponPayment(CancelTransCouponPaymentRequest request)
        {
            CancelTransCouponPaymentResponse res = PosAPI.CancelTransCouponPayment(request);
            if (res.CancelTransCouponPaymentResult)
            {
                return res.CancelTransCouponPaymentResult;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

      //  public bool CalcRSaleBillCent(CalcRSaleBillCentRequest )
      //  {
      //
      //  }  


        public CashCard GetCashCard(GetCashCardRequest Request)
        {
            GetCashCardResponse res = PosAPI.GetCashCard(Request);
            if(res.GetCashCardResult)
            {
                return res.cashCard;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }

        public int PrepareTransCashCardPayment(PrepareTransCashCardPaymentRequest Request)
        {
            PrepareTransCashCardPaymentResponse res = PosAPI.PrepareTransCashCardPayment(Request);

            if (res.PrepareTransCashCardPaymentResult)
            {
                return res.transID;
            }
            else
            {
                throw new Exception(res.msg);
            }
        }**/


        #endregion
    }
}
