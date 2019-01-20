using System;
using System.Collections.Generic;
using z.Extensions;
using System.Data;
using System.Linq;
using z.ServiceHelper;
using System.Diagnostics;

namespace z.POS.Services
{


    public class PosService : ServiceBase
    {

        internal PosService()
        {

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

    }
}
