using System.Data;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class ShglService: ServiceBase
    {
        internal ShglService()
        {
        }
        public DataGridResult GetMerchant(SearchItem item)
        {
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1 ";
            sql += " ORDER BY  MERCHANTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }
}
