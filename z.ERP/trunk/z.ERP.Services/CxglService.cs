using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class CxglService:ServiceBase
    {
        internal CxglService()
        {

        }
        public DataGridResult SearchPromotion(SearchItem item)
        {
            string sql = $@"SELECT * FROM PROMOTION WHERE 1=1 ";
            item.HasKey("NAME", a => sql += $" and NAME LIKE '%{a}%'");
            item.HasKey("YEAR", a => sql += $" and YEAR= {a}");
            item.HasKey("CONTENT", a => sql += $" and CONTENT LIKE '%{a}%'");
            item.HasDateKey("START_DATE_START", a => sql += $" and START_DATE >= {a}");
            item.HasDateKey("START_DATE_END", a => sql += $" and START_DATE <= {a}");
            item.HasDateKey("END_DATE_START", a => sql += $" and END_DATE >= {a}");
            item.HasDateKey("END_DATE_END", a => sql += $" and END_DATE <= {a}");
            item.HasKey("STATUS", a => sql += $" and STATUS= {a}");
            sql += " ORDER BY ID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public PROMOTIONEntity ShowOneData(PROMOTIONEntity data)
        {
            string sql = $@"SELECT * FROM PROMOTION WHERE ID=" + data.ID;
            var res = DbHelper.ExecuteOneObject<PROMOTIONEntity>(sql);
            return res;
        }
    }
}
