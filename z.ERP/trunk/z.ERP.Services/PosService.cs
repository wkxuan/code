using System;
using System.Collections.Generic;
using z.ERP.Entities.Service.Pos;
using z.Extensions;
using System.Data;
using System.Linq;

namespace z.ERP.Services
{
    public class PosService : ServiceBase
    {
        internal PosService()
        {

        }

        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            string sql = "select a.goodsid,a.name,a.type,nvl(a.price,0) price,nvl(a.member_price,0) member_price,b.shopid";
            sql += "        from GOODS a,GOODS_SHOP b where a.goodsid=b.goodsid";

            if (filter.shopid.HasValue)
                sql += $"  and b.shopid = {filter.shopid}";
            if (filter.goodsdm.IsNotEmpty())
                sql += $"  and (goodsdm = '{filter.goodsdm}' or barcode = '{filter.goodsdm}')";
            return DbHelper.ExecuteObject<FindGoodsResult>(sql);
        }

        /// <summary>
        /// 最大交易号
        /// </summary>
        /// <returns></returns>
        public long GetLastDealid()
        {
            string sql = $"select nvl(max(dealid),0) from sale where posno = '{employee.PlatformId}'";
            return long.Parse(DbHelper.ExecuteTable(sql).Rows[0][0].ToString()); 
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

            return DbHelper.ExecuteObject<FKFSResult>(sql);
        }

        public SaleRequest GetDeal(GetDealFilter filter)
        {
            string sqlSale = "select posno,dealid,sale_time,account_date,cashierid,sale_amount,change_amount,";
            sqlSale += $" nvl(member_cardid,-1) member_cardid,nvl(crm_recordid,-1) crm_recordid,";
            sqlSale += $" posno_old,nvl(dealid_old,-1) dealid_old from sale";
            sqlSale += $" where posno='{filter.posno}' and dealid={filter.dealid}";


            string sqlGoods = "select sheetid,inx,shopid,goodsid,goodscode,price,quantity,";
            sqlGoods += "  sale_amount,discount_amount,coupon_amount from sale_goods";
            sqlGoods += $" where posno='{filter.posno}' and dealid={filter.dealid}";


            string sqlPay = "select payid,amount from sale_pay";
            sqlPay += $" where posno='{filter.posno}' and dealid={filter.dealid}";

            string sqlClerk = "select sheetid,clerkid from sale_clerk";
            sqlClerk += $" where posno='{filter.posno}' and dealid={filter.dealid}";

            DataTable saleDt = DbHelper.ExecuteTable(sqlSale);
         //   List<SaleRequest> saleList = DbHelper.ExecuteObject<SaleRequest>(sqlSale);

            if(!saleDt.IsNotNull())
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
                member_cardid = saleDt.Rows[0][7].ToString().ToInt(),
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
            decimal goodsSaleAmount = request.goodslist.Sum(a => a.sale_amount);
            decimal payAmount = request.paylist.Sum(a => a.amount);

            if (goodsSaleAmount != payAmount)
            {
                throw new Exception("商品列表中销售金额合计与支付列表中的销售金额合计不相等!");
            }

            int goodsCount = request.goodslist.Count;
            int payCount = request.paylist.Count;
            int clerkCount = request.clerklist.Count;

            string[] sqlarr = new string[1+goodsCount+payCount+clerkCount];

            sqlarr[0] = "insert into sale(posno,dealid,sale_time,account_date,cashierid,sale_amount,";
            sqlarr[0] += "change_amount,member_cardid,crm_recordid,posno_old,dealid_old)";
            sqlarr[0] += $"values('{posNo}',{request.dealid},to_date('{request.sale_time}','yyyy-mm-dd HH24:MI:SS'),";
            if (request.account_date.ToString().IsEmpty())
                sqlarr[0] += $"to_date('{request.sale_time.Date}','yyyy-mm-dd HH24:MI:SS'),";
            else
                sqlarr[0] += $"to_date('{request.account_date.Date}','yyyy-mm-dd HH24:MI:SS'),";
            sqlarr[0] += $"{request.cashierid},{goodsSaleAmount},{request.change_amount},";
            sqlarr[0] += $"{request.member_cardid},{request.crm_recordid},";

            if(request.posno.IsNotEmpty())   
                sqlarr[0] += $"'{request.posno_old}',";
            else
                sqlarr[0] += "null,";

            if(request.dealid_old.HasValue && request.dealid_old>0)
                sqlarr[0] += $"{request.dealid_old})";
            else
                sqlarr[0] += "null)";


            int j;

            for(int i=1;i<=goodsCount;i++)
            {
                j = 0;
                sqlarr[i] = "insert into sale_goods(posno,dealid,sheetid,inx,shopid,goodsid,goodscode,";
                sqlarr[i] += "price,quantity,sale_amount,discount_amount,coupon_amount)";
                sqlarr[i] += $"values('{posNo}',{request.dealid},{request.goodslist[j].sheetid},";
                sqlarr[i] += $"{request.goodslist[j].inx},{request.goodslist[j].shopid},{request.goodslist[j].goodsid},";
                sqlarr[i] += $"'{request.goodslist[j].goodscode}',{request.goodslist[j].price},{request.goodslist[j].quantity},";
                sqlarr[i] += $"{request.goodslist[j].sale_amount},{request.goodslist[j].discount_amount},";
                sqlarr[i] += $"{request.goodslist[j].coupon_amount})";
                j++;
            }

            for (int i = 1 + goodsCount; i <= goodsCount + payCount; i++)
            {
                j = 0;
                sqlarr[i] = "insert into sale_pay(posno,dealid,payid,amount)";
                sqlarr[i] += $"values('{posNo}',{request.dealid},{request.paylist[j].payid},{request.paylist[j].amount})";
                j++;
            }

            for (int i = 1 + goodsCount + payCount; i <= goodsCount + payCount + clerkCount; i++)
            {
                j = 0;
                sqlarr[i] = "insert into sale_clerk(posno,dealid,sheetid,clerkid)";
                sqlarr[i] += $"values('{posNo}',{request.dealid},{request.clerklist[j].sheetid},{request.clerklist[j].clerkid})";
                j++;
            }

            int insertCount = 0;
            try
            {
                 insertCount = DbHelper.ExecuteNonQuery(sqlarr);
            }
            catch (Exception)
            {

                throw new Exception("提交数据库时发生异常!");
            }
           
            if(insertCount != 1+ goodsCount + payCount + clerkCount)
            {
                throw new Exception("写入数据不完整!");
            }
        }
    }
}
