using System;
using System.Collections.Generic;
using z.ERP.Entities.Service.Pos;
using z.Extensions;

namespace z.ERP.Services
{
    public class PosService : ServiceBase
    {
        internal PosService()
        {

        }

        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            string sql = "select a.goodsid,a.Name,a.Type,a.price,a.member_price,0 Shopid from GOODS a where 1=1";
            if (filter.clerkid.HasValue)
                sql += $" and 1=1 ";
            if (filter.goodsdm.IsNotEmpty())
                sql += $" and (goodsdm like '%{filter.goodsdm}%' or barcode like '%{filter.goodsdm}%')";
            return DbHelper.ExecuteObject<FindGoodsResult>(sql);
        }

        /// <summary>
        /// 最大交易号
        /// </summary>
        /// <returns></returns>
        public string GetLastDealid()
        {
            string sql = $"select nvl(max(dealid),0) from sale where posno = {employee.PlatformId}";
            return  DbHelper.ExecuteTable(sql).Rows[0][0].ToString(); 
        }

        public UserYYYResult GetYYY(string code)
        {
            throw new Exception("找不到营业员");
        }

        public List<FKFSResult> GetFKFS()
        {
            return new List<FKFSResult>();
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
