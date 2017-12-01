using System.Data;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class BrandService : ServiceBase
    {
        internal BrandService()
        {

        }

        public DataGridResult GetBrandData()
        {
            SearchItem item = SearchItem.GetAllPram();
            string sql = $@"select * from BRAND where 1=1 ";            
            item.HasKey("NAME", a => sql += $" and NAME LIKE ' '{a}' %'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }
}
