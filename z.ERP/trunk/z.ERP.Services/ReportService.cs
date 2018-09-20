using System.Data;
using z.MVC5.Results;
using z.Extensiont;
namespace z.ERP.Services
{
    public class ReportService : ServiceBase
    {
        internal ReportService()
        {
        }

        public DataGridResult ContractSale(SearchItem item)
        {
            string sql = $"SELECT C.*,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME ";
            sql += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K ";
            sql += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME",a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b =>  b) }%'");
            item.HasKey("BRANDID", a => sql += $" and C.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            

            sql += " ORDER BY  C.RQ,C.MERCHANTID,C.CONTRACTID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GOODSSale(SearchItem item)
        {
            string sql = $"SELECT D.*,M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME ";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and G.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");


            sql += " ORDER BY  D.RQ,G.MERCHANTID,G.CONTRACTID,D.GOODSID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

    }
}
