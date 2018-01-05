using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ERP.Entities;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class WyglService:ServiceBase
    {
        internal WyglService()
        {

        }
        public DataGridResult GetEnergyreGister(SearchItem item)
        {
            string sql = $@"select * from ENERGY_REGISTER where 1=1 ";
            item.HasKey("BILLID", a => sql += $" and BILLID = '{a}'");
            item.HasKey("CHECK_DATE_START", a => sql += $" and CHECK_DATE>= '{a}'");
            item.HasKey("CHECK_DATE_END", a => sql += $" and CHECK_DATE<= '{a}'");
            sql += " order by BILLID desc";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


    }
}
