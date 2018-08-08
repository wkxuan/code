using System;
using System.Collections.Generic;
using z.ERP.Entities.Service.Pos;
using z.Extensions;
using System.Data;

namespace z.ERP.Services
{
    public class PosService : ServiceBase
    {
        internal PosService()
        {

        }

        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            string sql = "select a.goodsid,a.name,a.type,a.price,a.member_price from GOODS a where 1=1";

            if (filter.shopId.HasValue)
                sql += $" and exists(select 1 from GOODS_SHOP b where a.goodsid=b.goodsid and b.shopid = {filter.shopId})";
            if (filter.goodsdm.IsNotEmpty())
                sql += $" and (goodsdm = '{filter.goodsdm}' or barcode = '{filter.goodsdm}')";
            return DbHelper.ExecuteObject<FindGoodsResult>(sql);
        }

        /// <summary>
        /// 最大交易号
        /// </summary>
        /// <returns></returns>
        public string GetLastDealid()
        {
            string sql = $"select nvl(max(dealid),0) from sale where posno = '{employee.PlatformId}'";
            return  DbHelper.ExecuteTable(sql).Rows[0][0].ToString(); 
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

        public DealResult GetDeal()
        {
            return new DealResult();
        }

        public void Sale(SaleRequest request)
        {
        }
    }
}
