﻿using System.Data;
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

            if (count > 0)
            {

 
            string sqlsum = $"SELECT SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT ";
            sqlsum += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K ";
            sqlsum += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sqlsum += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sqlsum += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sqlsum += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sqlsum += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sqlsum += $" and C.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sqlsum += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sqlsum += $" and C.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sqlsum += $" and B.NAME LIKE '%{a}%'");


            sql += " ORDER BY  C.RQ,C.MERCHANTID,C.CONTRACTID ";

            DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
            DataRow dr = dt.NewRow();
            dr["CONTRACTID"] = "合计";
            dr["AMOUNT"] = dtSum.Rows[0]["AMOUNT"].ToString();
            dr["COST"] = dtSum.Rows[0]["COST"].ToString();
            dr["DIS_AMOUNT"] = dtSum.Rows[0]["DIS_AMOUNT"].ToString();
            dr["PER_AMOUNT"] = dtSum.Rows[0]["PER_AMOUNT"].ToString();
            dt.Rows.Add(dr);
            }
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

            if(count > 0)
            {
            string sqlsum = $"SELECT SUM(D.AMOUNT) AMOUNT,SUM(D.COST) COST,SUM(D.DIS_AMOUNT) DIS_AMOUNT,SUM(D.PER_AMOUNT) PER_AMOUNT";
            sqlsum += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
            sqlsum += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sqlsum += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sqlsum += $" and D.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sqlsum += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sqlsum += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sqlsum += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sqlsum += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sqlsum += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sqlsum += $" and G.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sqlsum += $" and B.NAME LIKE '%{a}%'");

            DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
            DataRow dr = dt.NewRow();
            dr["GOODSDM"] = "合计";
            dr["AMOUNT"] = dtSum.Rows[0]["AMOUNT"].ToString();
            dr["COST"] = dtSum.Rows[0]["COST"].ToString();
            dr["DIS_AMOUNT"] = dtSum.Rows[0]["DIS_AMOUNT"].ToString();
            dr["PER_AMOUNT"] = dtSum.Rows[0]["PER_AMOUNT"].ToString();
            dt.Rows.Add(dr);
            }
            return new DataGridResult(dt, count);
        }


        public DataGridResult SaleRecord(SearchItem item)
        {
            string sql = $"SELECT S.POSNO,S.DEALID,S.SALE_TIME,trunc(S.ACCOUNT_DATE) ACCOUNT_DATE,U.USERNAME CASHIERNAME, U.USERCODE CASHIERCODE,";
            sql += " S.SALE_AMOUNT,S.CHANGE_AMOUNT,S.POSNO_OLD,S.DEALID_OLD ";
            sql += " FROM SALE S, SYSUSER U ";
            sql += " WHERE S.CASHIERID = U.USERID ";
            item.HasDateKey("SALE_TIME_START", a => sql += $" and trunc(S.SALE_TIME) >= {a}");
            item.HasDateKey("SALE_TIME_END", a => sql += $" and trunc(S.SALE_TIME) <= {a}");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and S.ACCOUNT_DATE >= {a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and S.ACCOUNT_DATE <= {a}");
            item.HasKey("CASHIERID", a => sql += $" and S.CASHIERID = {a}");

            sql += " ORDER BY  S.POSNO,S.DEALID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                string sqlsum = $"SELECT sum(S.SALE_AMOUNT) SALE_AMOUNT";
                sqlsum += " FROM SALE S, SYSUSER U ";
                sqlsum += " WHERE S.CASHIERID = U.USERID ";
                item.HasDateKey("SALE_TIME_START", a => sqlsum += $" and trunc(S.SALE_TIME) >= {a}");
                item.HasDateKey("SALE_TIME_END", a => sqlsum += $" and trunc(S.SALE_TIME) <= {a}");
                item.HasDateKey("ACCOUNT_DATE_START", a => sqlsum += $" and S.ACCOUNT_DATE >= {a}");
                item.HasDateKey("ACCOUNT_DATE_END", a => sqlsum += $" and S.ACCOUNT_DATE <= {a}");
                item.HasKey("CASHIERID", a => sqlsum += $" and S.CASHIERID = {a}");

                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                DataRow dr = dt.NewRow();
                dr["POSNO"] = "合计";
                dr["SALE_AMOUNT"] = dtSum.Rows[0]["SALE_AMOUNT"].ToString();
                dt.Rows.Add(dr);
                
            }
            return new DataGridResult(dt, count);

        }

    }
}