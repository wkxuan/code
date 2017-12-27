using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;

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

        public void DeleteMerchant(List<MERCHANTEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                var v = GetVerify(mer);
                //校验
                DbHelper.Delete(mer);
            }
        }
    }
}
