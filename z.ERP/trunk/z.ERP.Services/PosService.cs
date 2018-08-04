using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Entities.Service.Pos;
using z.ERP.Model.Vue;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class PosService : ServiceBase
    {
        internal PosService()
        {

        }

        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            var e = employee;
            string sql = "select a.goodsid,a.Name,a.Type,a.price,a.member_price,0 Shopid from GOODS a where 1=1";
            if (filter.clerkid.HasValue)
                sql += $" and 1=1 ";
            if (filter.goodsdm.IsNotEmpty())
                sql += $" and (goodsdm like '%{filter.goodsdm}%' or barcode like '%{filter.goodsdm}%')";
            return DbHelper.ExecuteObject<FindGoodsResult>(sql);
        }
    }
}
