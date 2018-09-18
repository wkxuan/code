using System.Data;
using z.MVC5.Results;
namespace z.ERP.Services
{
    public class ReportService : ServiceBase
    {
        internal ReportService()
        {
        }

        public DataGridResult ContractSale(SearchItem item)
        {
            string sql = $"SELECT C.*,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME ";
            sql += "FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID";
            item.HasKey("CONTRACTID", a => sql += $" and CONTRACTID = '{a}'");
            sql += " ORDER BY CONTRACTID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

    }
}
