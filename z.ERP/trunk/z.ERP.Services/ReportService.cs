﻿using System.Data;
using z.MVC5.Results;
using System;
using z.Extensions;

namespace z.ERP.Services
{
    public class ReportService : ServiceBase
    {
        internal ReportService()
        {
        }

        public DataGridResult ContractSale(SearchItem item)
        {
            string sql = $"SELECT C.*,G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME ";
            sql += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K,CATEGORY G,FLOOR F";
            sql += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
            sql += "  and B.CATEGORYID=G.CATEGORYID and S.FLOORID=F.ID";
            item.HasKey("CATEGORYCODE", a => sql += $" and G.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and C.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and C.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and C.YEARMONTH <= {a}");

            sql += " ORDER BY  C.RQ,C.MERCHANTID,C.CONTRACTID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                string sqlsum = $"SELECT SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT ";
                sqlsum += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K,CATEGORY G,FLOOR F";
                sqlsum += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
                sqlsum += "  and B.CATEGORYID=G.CATEGORYID  and S.FLOORID=F.ID";
                item.HasKey("CATEGORYCODE", a => sqlsum += $" and G.CATEGORYCODE LIKE '{a}%'");
                item.HasKey("FLOORID", a => sqlsum += $" and F.ID = {a}");
                item.HasKey("BRANCHID", a => sqlsum += $" and C.BRANCHID = {a}");
                item.HasKey("CONTRACTID", a => sqlsum += $" and C.CONTRACTID = '{a}'");
                item.HasDateKey("RQ_START", a => sqlsum += $" and C.RQ >= {a}");
                item.HasDateKey("RQ_END", a => sqlsum += $" and C.RQ <= {a}");
                item.HasKey("MERCHANTID", a => sqlsum += $" and C.MERCHANTID LIKE '%{a}%'");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
                item.HasArrayKey("KINDID", a => sqlsum += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
                item.HasKey("BRANDID", a => sqlsum += $" and C.BRANDID = {a}");
                item.HasKey("BRANDNAME", a => sqlsum += $" and B.NAME LIKE '%{a}%'");
                item.HasKey("YEARMONTH_START", a => sqlsum += $" and C.YEARMONTH >= {a}");
                item.HasKey("YEARMONTH_END", a => sqlsum += $" and C.YEARMONTH <= {a}");

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

        public DataGridResult ContractSaleM(SearchItem item)
        {
            string sql = $"SELECT C.YEARMONTH,SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT,";
            sql += " C.MERCHANTID,C.CONTRACTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,";
            sql += " G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE";
            sql += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K,CATEGORY G,FLOOR F ";
            sql += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
            sql += "  and B.CATEGORYID=G.CATEGORYID  and S.FLOORID=F.ID";
            item.HasKey("CATEGORYCODE", a => sql += $" and G.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and C.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and C.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and C.YEARMONTH <= {a}");

            sql += " GROUP BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID,M.NAME,S.CODE,S.NAME,B.NAME,K.CODE,K.NAME,G.CATEGORYCODE,G.CATEGORYNAME,F.CODE";
            sql += " ORDER BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                string sqlsum = $"SELECT SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT ";
                sqlsum += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K,CATEGORY G,FLOOR F ";
                sqlsum += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
                sqlsum += "  and B.CATEGORYID=G.CATEGORYID  and S.FLOORID=F.ID";
                item.HasKey("CATEGORYCODE", a => sqlsum += $" and G.CATEGORYCODE LIKE '{a}%'");
                item.HasKey("FLOORID", a => sqlsum += $" and F.ID = {a}");
                item.HasKey("BRANCHID", a => sqlsum += $" and C.BRANCHID = {a}");
                item.HasKey("CONTRACTID", a => sqlsum += $" and C.CONTRACTID = '{a}'");
                item.HasDateKey("RQ_START", a => sqlsum += $" and C.RQ >= {a}");
                item.HasDateKey("RQ_END", a => sqlsum += $" and C.RQ <= {a}");
                item.HasKey("MERCHANTID", a => sqlsum += $" and C.MERCHANTID LIKE '%{a}%'");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
                item.HasArrayKey("KINDID", a => sqlsum += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
                item.HasKey("BRANDID", a => sqlsum += $" and C.BRANDID = {a}");
                item.HasKey("BRANDNAME", a => sqlsum += $" and B.NAME LIKE '%{a}%'");
                item.HasKey("YEARMONTH_START", a => sqlsum += $" and C.YEARMONTH >= {a}");
                item.HasKey("YEARMONTH_END", a => sqlsum += $" and C.YEARMONTH <= {a}");

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
        public string ContractSaleOutput(SearchItem item)
        {
            string sql = $"SELECT  to_char(C.RQ,'yyyy-mm-dd') RQ,C.AMOUNT,C.COST,C.DIS_AMOUNT,C.PER_AMOUNT,C.MERCHANTID,C.CONTRACTID,";
            sql += "  M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME, ";
            sql += " G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE";
            sql += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K,CATEGORY G,FLOOR F  ";
            sql += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
            sql += "  and B.CATEGORYID=G.CATEGORYID  and S.FLOORID=F.ID";
            item.HasKey("CATEGORYCODE", a => sql += $" and G.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and C.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and C.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and C.YEARMONTH <= {a}");

            sql += " ORDER BY  C.RQ,C.MERCHANTID,C.CONTRACTID ";

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "ContractSale";
            return GetExport("租约销售导出", a =>
            {
                a.SetTable(dt);
            });
        }

        public string ContractSaleMOutput(SearchItem item)
        {
            string sql = $"SELECT C.YEARMONTH RQ,SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT,";
            sql += " C.MERCHANTID,C.CONTRACTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME, ";
            sql += " G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE";
            sql += " FROM CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,GOODS_KIND K,CATEGORY G,FLOOR F  ";
            sql += " WHERE C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID AND C.KINDID=K.ID";
            sql += "  and B.CATEGORYID=G.CATEGORYID  and S.FLOORID=F.ID";
            item.HasKey("CATEGORYCODE", a => sql += $" and G.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and C.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and C.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and C.YEARMONTH <= {a}");

            sql += " GROUP BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID,M.NAME,S.CODE,S.NAME,B.NAME,K.CODE,K.NAME,G.CATEGORYCODE,G.CATEGORYNAME,F.CODE";
            sql += " ORDER BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID ";

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "ContractSale";
            return GetExport("租约销售导出", a =>
            {
                a.SetTable(dt);
            });
        }

        public DataGridResult GoodsSale(SearchItem item)
        {
            string sql = $"SELECT D.RQ,D.AMOUNT,D.COST,D.DIS_AMOUNT,D.PER_AMOUNT,M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME ";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and G.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and D.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and D.YEARMONTH <= {a}");


            sql += " ORDER BY  D.RQ,G.MERCHANTID,G.CONTRACTID,G.GOODSDM ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                string sqlsum = $"SELECT SUM(D.AMOUNT) AMOUNT,SUM(D.COST) COST,SUM(D.DIS_AMOUNT) DIS_AMOUNT,SUM(D.PER_AMOUNT) PER_AMOUNT";
                sqlsum += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
                sqlsum += " WHERE G.MERCHANTID=M.MERCHANTID ";
                sqlsum += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
                item.HasKey("BRANCHID", a => sqlsum += $" and D.BRANCHID = {a}");
                item.HasKey("GOODSDM", a => sqlsum += $" and G.GOODSDM = '{a}'");
                item.HasKey("GOODSNAME", a => sqlsum += $" and G.NAME LIKE '%{a}%'");
                item.HasKey("CONTRACTID", a => sqlsum += $" and G.CONTRACTID = '{a}'");
                item.HasDateKey("RQ_START", a => sqlsum += $" and D.RQ >= {a}");
                item.HasDateKey("RQ_END", a => sqlsum += $" and D.RQ <= {a}");
                item.HasKey("MERCHANTID", a => sqlsum += $" and G.MERCHANTID LIKE '%{a}%'");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
                item.HasArrayKey("KINDID", a => sqlsum += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
                item.HasKey("BRANDID", a => sqlsum += $" and G.BRANDID = {a}");
                item.HasKey("BRANDNAME", a => sqlsum += $" and B.NAME LIKE '%{a}%'");
                item.HasKey("YEARMONTH_START", a => sqlsum += $" and D.YEARMONTH >= {a}");
                item.HasKey("YEARMONTH_END", a => sqlsum += $" and D.YEARMONTH <= {a}");

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

        public DataGridResult GoodsSaleM(SearchItem item)
        {
            string sql = $"SELECT D.YEARMONTH,M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME,";
            sql += " sum(D.AMOUNT) AMOUNT,sum(D.COST) COST,sum(D.DIS_AMOUNT) DIS_AMOUNT,sum(D.PER_AMOUNT) PER_AMOUNT";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and G.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and D.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and D.YEARMONTH <= {a}");

            sql += " GROUP BY D.YEARMONTH,M.NAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME,K.CODE,K.NAME,G.GOODSDM,G.BARCODE,G.NAME";

            sql += " ORDER BY D.YEARMONTH,G.MERCHANTID,G.CONTRACTID,G.GOODSDM ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                string sqlsum = $"SELECT SUM(D.AMOUNT) AMOUNT,SUM(D.COST) COST,SUM(D.DIS_AMOUNT) DIS_AMOUNT,SUM(D.PER_AMOUNT) PER_AMOUNT";
                sqlsum += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
                sqlsum += " WHERE G.MERCHANTID=M.MERCHANTID ";
                sqlsum += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
                item.HasKey("BRANCHID", a => sqlsum += $" and D.BRANCHID = {a}");
                item.HasKey("GOODSDM", a => sqlsum += $" and G.GOODSDM = '{a}'");
                item.HasKey("GOODSNAME", a => sqlsum += $" and G.NAME LIKE '%{a}%'");
                item.HasKey("CONTRACTID", a => sqlsum += $" and G.CONTRACTID = '{a}'");
                item.HasDateKey("RQ_START", a => sqlsum += $" and D.RQ >= {a}");
                item.HasDateKey("RQ_END", a => sqlsum += $" and D.RQ <= {a}");
                item.HasKey("MERCHANTID", a => sqlsum += $" and G.MERCHANTID LIKE '%{a}%'");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
                item.HasArrayKey("KINDID", a => sqlsum += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
                item.HasKey("BRANDID", a => sqlsum += $" and G.BRANDID = {a}");
                item.HasKey("BRANDNAME", a => sqlsum += $" and B.NAME LIKE '%{a}%'");
                item.HasKey("YEARMONTH_START", a => sqlsum += $" and D.YEARMONTH >= {a}");
                item.HasKey("YEARMONTH_END", a => sqlsum += $" and D.YEARMONTH <= {a}");

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

        public string GoodsSaleOutput(SearchItem item)
        {
            string sql = $"SELECT to_char(D.RQ,'yyyy-mm-dd') RQ,D.AMOUNT,D.COST,D.DIS_AMOUNT,D.PER_AMOUNT,";
            sql += " M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME ";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and G.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and D.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and D.YEARMONTH <= {a}");

            sql += " ORDER BY  D.RQ,G.MERCHANTID,G.CONTRACTID,D.GOODSID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "GoodsSale";
            return GetExport("商品销售导出", a =>
            {
                a.SetTable(dt);
            });
        }
        public string GoodsSaleMOutput(SearchItem item)
        {
            string sql = $"SELECT D.YEARMONTH RQ,SUM(D.AMOUNT) AMOUNT,SUM(D.COST) COST,SUM(D.DIS_AMOUNT) DIS_AMOUNT,SUM(D.PER_AMOUNT) PER_AMOUNT,";
            sql += " M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME ";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasArrayKey("KINDID", a => sql += $" and K.PKIND_ID LIKE '{ a.SuperJoin(",", b => b) }%'");
            item.HasKey("BRANDID", a => sql += $" and G.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and D.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and D.YEARMONTH <= {a}");

            sql += " GROUP BY D.YEARMONTH,M.NAME,G.CONTRACTID,G.MERCHANTID,B.NAME,K.CODE,K.NAME,G.GOODSDM,G.BARCODE,G.NAME";
            sql += " ORDER BY D.YEARMONTH,G.MERCHANTID,G.CONTRACTID,G.GOODSDM ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "GoodsSale";
            return GetExport("商品销售导出", a =>
            {
                a.SetTable(dt);
            });
        }


        public DataGridResult SaleRecord(SearchItem item)
        {
            string sql = $"SELECT S.POSNO,S.DEALID,S.SALE_TIME,trunc(S.ACCOUNT_DATE) ACCOUNT_DATE,U.USERNAME CASHIERNAME, U.USERCODE CASHIERCODE,";
            sql += " S.SALE_AMOUNT,S.CHANGE_AMOUNT,S.POSNO_OLD,S.DEALID_OLD ";
            sql += " FROM SALE S,SYSUSER U,STATION T";
            sql += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH";
            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");
          //  item.HasKey("SHOPID", a => sql += $" and exists(select 1 from SALE_GOODS G where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.SHOPID={a})");
            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID ='{a}')");
            item.HasKey("MERCHANTNAME", a => sql += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D,MERCHANT M WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID=M.MERCHANTID AND M.NAME LIKE '%{a}%')");
            item.HasKey("SHOPCODE", a => sql += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.CODE LIKE '%{a}%')");
            item.HasKey("SHOPNAME", a => sql += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.NAME LIKE '%{a}%')");
            item.HasDateKey("SALE_TIME_START", a => sql += $" and trunc(S.SALE_TIME) >= {a}");
            item.HasDateKey("SALE_TIME_END", a => sql += $" and trunc(S.SALE_TIME) <= {a}");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and S.ACCOUNT_DATE >= {a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and S.ACCOUNT_DATE <= {a}");
            item.HasKey("CASHIERID", a => sql += $" and S.CASHIERID = {a}");

            sql += " union all ";

            sql += " SELECT S.POSNO,S.DEALID,S.SALE_TIME,trunc(S.ACCOUNT_DATE) ACCOUNT_DATE,U.USERNAME CASHIERNAME, U.USERCODE CASHIERCODE,";
            sql += " S.SALE_AMOUNT,S.CHANGE_AMOUNT,S.POSNO_OLD,S.DEALID_OLD ";
            sql += " FROM HIS_SALE S, SYSUSER U,STATION T";
            sql += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH";
            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID ='{a}')");
            //   item.HasKey("SHOPID", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.SHOPID={a})");
            item.HasKey("MERCHANTNAME", a => sql += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D,MERCHANT M WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID=M.MERCHANTID AND M.NAME LIKE '%{a}%')");
            item.HasKey("SHOPCODE", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.CODE LIKE '%{a}%')");
            item.HasKey("SHOPNAME", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.NAME LIKE '%{a}%')");
            item.HasDateKey("SALE_TIME_START", a => sql += $" and trunc(S.SALE_TIME) >= {a}");
            item.HasDateKey("SALE_TIME_END", a => sql += $" and trunc(S.SALE_TIME) <= {a}");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and S.ACCOUNT_DATE >= {a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and S.ACCOUNT_DATE <= {a}");
            item.HasKey("CASHIERID", a => sql += $" and S.CASHIERID = {a}");

            sql += " ORDER BY  1,2 ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                string sqlsum = $"SELECT nvl(sum(S.SALE_AMOUNT),0) SALE_AMOUNT";
                sqlsum += " FROM SALE S, SYSUSER U,STATION T";
                sqlsum += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH ";
                item.HasKey("BRANCHID", a => sqlsum += $" and T.BRANCHID={a}");
                item.HasKey("POSNO", a => sqlsum += $" and S.POSNO='{a}'");
                item.HasKey("MERCHANTID", a => sqlsum += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID ='{a}')");
                // item.HasKey("SHOPID", a => sqlsum += $" and exists(select 1 from SALE_GOODS G where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.SHOPID={a})");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D,MERCHANT M WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID=M.MERCHANTID AND M.NAME LIKE '%{a}%')");
                item.HasKey("SHOPCODE", a => sqlsum += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.CODE LIKE '%{a}%')");
                item.HasKey("SHOPNAME", a => sqlsum += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.NAME LIKE '%{a}%')");
                item.HasDateKey("SALE_TIME_START", a => sqlsum += $" and trunc(S.SALE_TIME) >= {a}");
                item.HasDateKey("SALE_TIME_END", a => sqlsum += $" and trunc(S.SALE_TIME) <= {a}");
                item.HasDateKey("ACCOUNT_DATE_START", a => sqlsum += $" and S.ACCOUNT_DATE >= {a}");
                item.HasDateKey("ACCOUNT_DATE_END", a => sqlsum += $" and S.ACCOUNT_DATE <= {a}");
                item.HasKey("CASHIERID", a => sqlsum += $" and S.CASHIERID = {a}");

                sqlsum += " union all ";

                sqlsum += " SELECT nvl(sum(S.SALE_AMOUNT),0) SALE_AMOUNT";
                sqlsum += " FROM HIS_SALE S, SYSUSER U,STATION T";
                sqlsum += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH ";
                item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
                item.HasKey("POSNO", a => sqlsum += $" and S.POSNO='{a}'");
                item.HasKey("MERCHANTID", a => sqlsum += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID ='{a}')");
                //item.HasKey("SHOPID", a => sqlsum += $" and exists(select 1 from HIS_SALE_GOODS G where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.SHOPID={a})");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D,MERCHANT M WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID=M.MERCHANTID AND M.NAME LIKE '%{a}%')");
                item.HasKey("SHOPCODE", a => sqlsum += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.CODE LIKE '%{a}%')");
                item.HasKey("SHOPNAME", a => sqlsum += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.NAME LIKE '%{a}%')");
                item.HasDateKey("SALE_TIME_START", a => sqlsum += $" and trunc(S.SALE_TIME) >= {a}");
                item.HasDateKey("SALE_TIME_END", a => sqlsum += $" and trunc(S.SALE_TIME) <= {a}");
                item.HasDateKey("ACCOUNT_DATE_START", a => sqlsum += $" and S.ACCOUNT_DATE >= {a}");
                item.HasDateKey("ACCOUNT_DATE_END", a => sqlsum += $" and S.ACCOUNT_DATE <= {a}");
                item.HasKey("CASHIERID", a => sqlsum += $" and S.CASHIERID = {a}");

                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                DataRow dr = dt.NewRow();
                dr["POSNO"] = "合计";
                dr["SALE_AMOUNT"] = (Convert.ToSingle(dtSum.Rows[0]["SALE_AMOUNT"]) + Convert.ToSingle(dtSum.Rows[1]["SALE_AMOUNT"])).ToString();
                dt.Rows.Add(dr);

            }
            return new DataGridResult(dt, count);

        }

        public string SaleRecordOutput(SearchItem item)
        {
            string sql = $"SELECT S.POSNO,S.DEALID,to_char(S.SALE_TIME,'yy-mm-dd hh24:mi:ss') SALE_TIME,";
            sql += " to_char(trunc(S.ACCOUNT_DATE),'yy-mm-dd') ACCOUNT_DATE,U.USERNAME CASHIERNAME, U.USERCODE CASHIERCODE,";
            sql += " S.SALE_AMOUNT,S.CHANGE_AMOUNT,S.POSNO_OLD,S.DEALID_OLD ";
            sql += " FROM SALE S, SYSUSER U,STATION T";
            sql += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH";
            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID ='{a}')");
            //item.HasKey("SHOPID", a => sql += $" and exists(select 1 from SALE_GOODS G where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.SHOPID={a}");
            item.HasKey("MERCHANTNAME", a => sql += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D,MERCHANT M WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID=M.MERCHANTID AND M.NAME LIKE '%{a}%')");
            item.HasKey("SHOPCODE", a => sql += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.CODE LIKE '%{a}%')");
            item.HasKey("SHOPNAME", a => sql += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.NAME LIKE '%{a}%')");
            item.HasDateKey("SALE_TIME_START", a => sql += $" and trunc(S.SALE_TIME) >= {a}");
            item.HasDateKey("SALE_TIME_END", a => sql += $" and trunc(S.SALE_TIME) <= {a}");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and S.ACCOUNT_DATE >= {a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and S.ACCOUNT_DATE <= {a}");
            item.HasKey("CASHIERID", a => sql += $" and S.CASHIERID = {a}");

            sql += " union all ";

            sql += " SELECT S.POSNO,S.DEALID,to_char(S.SALE_TIME,'yy-mm-dd hh24:mi:ss') SALE_TIME,";
            sql += " to_char(trunc(S.ACCOUNT_DATE),'yy-mm-dd') ACCOUNT_DATE,U.USERNAME CASHIERNAME, U.USERCODE CASHIERCODE,";
            sql += " S.SALE_AMOUNT,S.CHANGE_AMOUNT,S.POSNO_OLD,S.DEALID_OLD ";
            sql += " FROM HIS_SALE S, SYSUSER U,STATION T";
            sql += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH ";
            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID ='{a}')");
            //item.HasKey("SHOPID", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.SHOPID={a}");
            item.HasKey("MERCHANTNAME", a => sql += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D,MERCHANT M WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID=M.MERCHANTID AND M.NAME LIKE '%{a}%')");
            item.HasKey("SHOPCODE", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.CODE LIKE '%{a}%')");
            item.HasKey("SHOPNAME", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.NAME LIKE '%{a}%')");
            item.HasDateKey("SALE_TIME_START", a => sql += $" and trunc(S.SALE_TIME) >= {a}");
            item.HasDateKey("SALE_TIME_END", a => sql += $" and trunc(S.SALE_TIME) <= {a}");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and S.ACCOUNT_DATE >= {a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and S.ACCOUNT_DATE <= {a}");
            item.HasKey("CASHIERID", a => sql += $" and S.CASHIERID = {a}");

            sql += " ORDER BY  1,2 ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "SaleRecord";
            return GetExport("POS销售导出", a =>
            {
                a.SetTable(dt);
            });
        }

        public DataGridResult MerchantRent(SearchItem item)
        {
            string sql = "select Z.*,(Z.TCRENTS-RENTS) CE,(case when (Z.TCRENTS-RENTS)>0 then (Z.TCRENTS-RENTS) else 0  end) YJT from ("
                            + "select Y.YEARMONTH,R.CODE FLOORCODE,P.CODE SHOPCODE,D.NAME BRANDNAME,Y.CONTRACTID,"
                            + "       C.CATEGORYCODE,C.CATEGORYNAME,Y.MERCHANTID,M.NAME MERCHANTNAME,Y.AMOUNT,"
                            + "       (select sum(AREA_RENTABLE) from CONTRACT_SHOP S where S.CONTRACTID = Y.CONTRACTID) AREA,"
                            + "       (select sum(CR.PRICE) from CONTRACT_RENT CR,CONTRACT_RENTITEM CRM"
                            + "         where CR.CONTRACTID = CRM.CONTRACTID and CR.INX = CRM.INX"
                            + "           and CR.CONTRACTID = Y.CONTRACTID and CRM.YEARMONTH = Y.YEARMONTH) RENTPRICE,"
                            + "       (select sum(RENTS) from CONTRACT_RENTITEM CRM"
                            + "         where CRM.CONTRACTID = Y.CONTRACTID and CRM.YEARMONTH = Y.YEARMONTH) RENTS,"
                            + "       (select sum(CT.TCZJ) from CONTRACT_TCZJ CT"
                            + "         where CT.CONTRACTID = Y.CONTRACTID and CT.YEARMONTH = Y.YEARMONTH) TCRENTS,"
                            + "       (select sum(CC.PRICE) from CONTRACT_COST CC,CONTRACT_COSTMX CCM"
                            + "         where CC.CONTRACTID = CCM.CONTRACTID and CC.TERMID = CCM.TERMID and CC.INX = CCM.INX"
                            + "           and CC.CONTRACTID = Y.CONTRACTID and CC.TERMID = 1 and CCM.YEARMONTH = Y.YEARMONTH) WYPRICE,"
                            + "       (select sum(CCM.SFJE) from CONTRACT_COSTMX CCM"
                            + "         where CCM.CONTRACTID = Y.CONTRACTID and CCM.TERMID = 1 and CCM.YEARMONTH = Y.YEARMONTH) WYJE"
                            + "  from CONTRACT_SUMMARY_YM Y, SHOP P,FLOOR R, BRAND D,CATEGORY C, MERCHANT M"
                            + " where Y.SHOPID = P.SHOPID and P.FLOORID = R.ID"
                            + "   and Y.BRANDID = D.ID and D.CATEGORYID = C.CATEGORYID"
                            + "   and Y.MERCHANTID = M.MERCHANTID";

            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and R.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and Y.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and Y.CONTRACTID = '{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and M.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and D.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and D.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and Y.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and Y.YEARMONTH <= {a}");


            sql += ") Z order by Z.YEARMONTH,Z.FLOORCODE,Z.SHOPCODE,Z.MERCHANTID";

            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public string MerchantRentOutput(SearchItem item)
        {
            string sql = "select Z.*,(Z.TCRENTS-RENTS) CE,(case when (Z.TCRENTS-RENTS)>0 then (Z.TCRENTS-RENTS) else 0  end) YJT from ("
                            + "select Y.YEARMONTH,R.CODE FLOORCODE,P.CODE SHOPCODE,D.NAME BRANDNAME,Y.CONTRACTID,"
                            + "       C.CATEGORYCODE,C.CATEGORYNAME,Y.MERCHANTID,M.NAME MERCHANTNAME,Y.AMOUNT,"
                            + "       (select sum(AREA_RENTABLE) from CONTRACT_SHOP S where S.CONTRACTID = Y.CONTRACTID) AREA,"
                            + "       (select sum(CR.PRICE) from CONTRACT_RENT CR,CONTRACT_RENTITEM CRM"
                            + "         where CR.CONTRACTID = CRM.CONTRACTID and CR.INX = CRM.INX"
                            + "           and CR.CONTRACTID = Y.CONTRACTID and CRM.YEARMONTH = Y.YEARMONTH) RENTPRICE,"
                            + "       (select sum(RENTS) from CONTRACT_RENTITEM CRM"
                            + "         where CRM.CONTRACTID = Y.CONTRACTID and CRM.YEARMONTH = Y.YEARMONTH) RENTS,"
                            + "       (select sum(CT.TCZJ) from CONTRACT_TCZJ CT"
                            + "         where CT.CONTRACTID = Y.CONTRACTID and CT.YEARMONTH = Y.YEARMONTH) TCRENTS,"
                            + "       (select sum(CC.PRICE) from CONTRACT_COST CC,CONTRACT_COSTMX CCM"
                            + "         where CC.CONTRACTID = CCM.CONTRACTID and CC.TERMID = CCM.TERMID and CC.INX = CCM.INX"
                            + "           and CC.CONTRACTID = Y.CONTRACTID and CC.TERMID = 1 and CCM.YEARMONTH = Y.YEARMONTH) WYPRICE,"
                            + "       (select sum(CCM.SFJE) from CONTRACT_COSTMX CCM"
                            + "         where CCM.CONTRACTID = Y.CONTRACTID and CCM.TERMID = 1 and CCM.YEARMONTH = Y.YEARMONTH) WYJE"
                            + "  from CONTRACT_SUMMARY_YM Y, SHOP P,FLOOR R, BRAND D,CATEGORY C, MERCHANT M"
                            + " where Y.SHOPID = P.SHOPID and P.FLOORID = R.ID"
                            + "   and Y.BRANDID = D.ID and D.CATEGORYID = C.CATEGORYID"
                            + "   and Y.MERCHANTID = M.MERCHANTID";

            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and R.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and Y.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and Y.CONTRACTID = '{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and M.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and D.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and D.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and Y.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and Y.YEARMONTH <= {a}");


            sql += ") Z order by Z.YEARMONTH,Z.FLOORCODE,Z.SHOPCODE,Z.MERCHANTID";

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "MerchantRent";
            return GetExport("商户租金计提表", a =>
            {
                a.SetTable(dt);
            });
        }

        public DataGridResult ContractInfo(SearchItem item)
        {
            string sql = " SELECT C.CONTRACTID,F.CODE FLOORCODE,S.CODE SHOPCODE,D.NAME BRANDNAME,"
                       + " M.MERCHANTID,M.NAME MERCHANTNAME, C.AREAR,to_char(C.CONT_START,'YYYY-MM-DD') CONT_START,"
                       + " to_char(C.CONT_END,'YYYY-MM-DD') CONT_END,O.NAME RENTWAY, CR.RENTPRICE,FR.NAME RENTRULE,"
                       + " (SELECT MAX(FW.NAME)  FROM CONTRACT_COST CC, FEERULE FW"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID"
                       + "     AND CC.FEERULEID = FW.ID AND CC.TERMID = 1) WYFRULE,"
                       + " (SELECT MAX(CC.PRICE)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 1) WYFPRICE,"
                       + " (SELECT MAX(CC.COST)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 2) LYBZJ,"
                       + " (SELECT MAX(CC.COST)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 3) ZXBZJ,"
                       + " (SELECT MAX(CC.COST)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 4) POSYJ"
                       + "  FROM CONTRACT C, MERCHANT M,CONTRACT_SHOP CS, CONTRACT_RENTPRICE CR,"
                       + "       SHOP S, FLOOR F,CONTRACT_BRAND CB, OPERATIONRULE O,FEERULE FR,"
                       + "       BRAND D,CATEGORY Y"
                       + " WHERE C.MERCHANTID = M.MERCHANTID AND C.CONTRACTID = CS.CONTRACTID"
                       + "   AND C.CONTRACTID = CR.CONTRACTID AND CS.SHOPID = S.SHOPID"
                       + "   AND S.FLOORID = F.ID AND C.CONTRACTID = CB.CONTRACTID"
                       + "   AND C.OPERATERULE = O.ID AND C.FEERULE_RENT = FR.ID"
                       + "   AND CB.BRANDID=D.ID AND D.CATEGORYID=Y.CATEGORYID"
                       + "   AND C.HTLX=1 AND C.STATUS !=5";

            item.HasKey("CATEGORYCODE", a => sql += $" and Y.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and M.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and D.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and D.NAME LIKE '%{a}%'");

            sql += " ORDER BY F.CODE,C.CONTRACTID";


            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public string ContractInfoOutput(SearchItem item)
        {
            string sql = " SELECT C.CONTRACTID,F.CODE FLOORCODE,S.CODE SHOPCODE,D.NAME BRANDNAME,"
                       + " M.MERCHANTID,M.NAME MERCHANTNAME, C.AREAR,to_char(C.CONT_START,'YYYY-MM-DD') CONT_START,"
                       + " to_char(C.CONT_END,'YYYY-MM-DD') CONT_END,O.NAME RENTWAY, CR.RENTPRICE,FR.NAME RENTRULE,"
                       + " (SELECT MAX(FW.NAME)  FROM CONTRACT_COST CC, FEERULE FW"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID"
                       + "     AND CC.FEERULEID = FW.ID AND CC.TERMID = 1) WYFRULE,"
                       + " (SELECT MAX(CC.PRICE)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 1) WYFPRICE,"
                       + " (SELECT MAX(CC.COST)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 2) LYBZJ,"
                       + " (SELECT MAX(CC.COST)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 3) ZXBZJ,"
                       + " (SELECT MAX(CC.COST)  FROM CONTRACT_COST CC"
                       + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = 4) POSYJ"
                       + "  FROM CONTRACT C, MERCHANT M,CONTRACT_SHOP CS, CONTRACT_RENTPRICE CR,"
                       + "       SHOP S, FLOOR F,CONTRACT_BRAND CB, OPERATIONRULE O,FEERULE FR,"
                       + "       BRAND D,CATEGORY Y"
                       + " WHERE C.MERCHANTID = M.MERCHANTID AND C.CONTRACTID = CS.CONTRACTID"
                       + "   AND C.CONTRACTID = CR.CONTRACTID AND CS.SHOPID = S.SHOPID"
                       + "   AND S.FLOORID = F.ID AND C.CONTRACTID = CB.CONTRACTID"
                       + "   AND C.OPERATERULE = O.ID AND C.FEERULE_RENT = FR.ID"
                       + "   AND CB.BRANDID=D.ID AND D.CATEGORYID=Y.CATEGORYID"
                       + "   AND C.HTLX=1 AND C.STATUS !=5";

            item.HasKey("CATEGORYCODE", a => sql += $" and Y.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and M.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and D.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and D.NAME LIKE '%{a}%'");

            sql += " ORDER BY F.CODE,C.CONTRACTID";

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "ContractInfo";
            return GetExport("合同信息表", a =>
            {
                a.SetTable(dt);
            });
        }

    }
}
