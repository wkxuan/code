using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ERP.Entities.Enum;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class JsglService:ServiceBase
    {
        internal JsglService()
        {

        }

        public DataGridResult GetJoinBillList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,T.NAME MERCHANTNAME " +
                " FROM JOIN_BILL L,BRANCH B ,MERCHANT T " +
                "  WHERE L.BRANCHID = B.ID AND L.MERCHANTID = T.MERCHANTID";
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and L.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and L.MERCHANTID={a}");
            item.HasKey("NIANYUE_START", a => sql += $" and L.NIANYUE>={a}");
            item.HasKey("NIANYUE_END", a => sql += $" and L.NIANYUE<={a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");            
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");            
            item.HasKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<结算单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

    }
}
