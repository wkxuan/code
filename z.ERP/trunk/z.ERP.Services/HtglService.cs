using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;
using System.Linq;
using z.Extensiont;

namespace z.ERP.Services
{
    public class HtglService: ServiceBase
    {
        internal HtglService()
        {
        }

        public DataGridResult GetContract(SearchItem item)
        {
            string sql = $@"SELECT * FROM CONTRACT WHERE 1=1 ";
            item.HasArrayKey("STYLE", a => sql += $" and STYLE in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            sql += " ORDER BY   CONTRACTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
    }
}
