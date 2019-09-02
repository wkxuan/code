﻿using System;
using System.Data;
using z.ERP.Entities.Enum;
using z.Extensions;
using z.MVC5.Results;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class ReportService : ServiceBase
    {
        internal ReportService()
        {
        }


        #region 租约销售
        public string ContractSaleSqlParm(SearchItem item)
        {
            string sqlParam = " from CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,CATEGORY G,FLOOR F";
            sqlParam += " where C.MERCHANTID=M.MERCHANTID AND C.SHOPID=S.SHOPID AND C.BRANDID=B.ID";
            sqlParam += "  and B.CATEGORYID=G.CATEGORYID and S.FLOORID=F.ID";
            sqlParam += "  and C.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sqlParam += "  and F.ID in (" + GetPermissionSql(PermissionType.Floor) + ")";  //楼层权限
            string SqlyTQx = GetYtQx("G");
            if (SqlyTQx != "")  //业态权限
            {
                sqlParam += " and " + SqlyTQx;
            }

            item.HasKey("CATEGORYCODE", a => sqlParam += $" and G.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sqlParam += $" and F.ID in ({a})");
            item.HasKey("BRANCHID", a => sqlParam += $" and C.BRANCHID = {a}");
            item.HasKey("CONTRACTID", a => sqlParam += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sqlParam += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sqlParam += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sqlParam += $" and C.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sqlParam += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sqlParam += $" and C.BRANDID = {a}");
            item.HasKey("BRANDNAME", a => sqlParam += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sqlParam += $" and C.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sqlParam += $" and C.YEARMONTH <= {a}");
            return sqlParam;
        }

        public DataGridResult ContractSale(SearchItem item)
        {
            string sql = @"select C.RQ,C.CONTRACTID,C.MERCHANTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,
                                  G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE,F.NAME FLOORNAME,
                                  B.NAME BRANDNAME,SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT";

            string sqlsum = $"select nvl(SUM(C.AMOUNT),0) AMOUNT,nvl(SUM(C.COST),0) COST,nvl(SUM(C.DIS_AMOUNT),0) DIS_AMOUNT," +
                              "nvl(SUM(C.PER_AMOUNT),0) PER_AMOUNT ";

            string sqlParam = ContractSaleSqlParm(item);

            sql += sqlParam;
            
            sql += @" GROUP BY C.RQ,C.CONTRACTID,C.MERCHANTID,M.NAME,S.CODE,S.NAME,
                             G.CATEGORYCODE,G.CATEGORYNAME,F.CODE,F.NAME,B.NAME 
                      ORDER BY  C.RQ,C.MERCHANTID,C.CONTRACTID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                sqlsum += sqlParam;

                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                if (dtSum.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["CONTRACTID"] = "合计";
                    dr["AMOUNT"] = dtSum.Rows[0]["AMOUNT"].ToString();
                    dr["COST"] = dtSum.Rows[0]["COST"].ToString();
                    dr["DIS_AMOUNT"] = dtSum.Rows[0]["DIS_AMOUNT"].ToString();
                    dr["PER_AMOUNT"] = dtSum.Rows[0]["PER_AMOUNT"].ToString();
                    dt.Rows.Add(dr);
                }               
            }
            return new DataGridResult(dt, count);
        }
        public DataGridResult ContractSaleM(SearchItem item)
        {
            
            string sql = $"SELECT C.YEARMONTH,SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,"+
                          " SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT,"+
                          " C.MERCHANTID,C.CONTRACTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,"+
                          " B.NAME BRANDNAME,"+
                          " G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE";

            string sqlsum = $"SELECT SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT ";

            string sqlParam = ContractSaleSqlParm(item);

            sql += sqlParam;
            sql += " GROUP BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID,M.NAME,S.CODE,S.NAME,B.NAME,G.CATEGORYCODE,G.CATEGORYNAME,F.CODE";
            sql += " ORDER BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                sqlsum += sqlParam;

                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                if (dtSum.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["CONTRACTID"] = "合计";
                    dr["AMOUNT"] = dtSum.Rows[0]["AMOUNT"].ToString();
                    dr["COST"] = dtSum.Rows[0]["COST"].ToString();
                    dr["DIS_AMOUNT"] = dtSum.Rows[0]["DIS_AMOUNT"].ToString();
                    dr["PER_AMOUNT"] = dtSum.Rows[0]["PER_AMOUNT"].ToString();
                    dt.Rows.Add(dr);
                }
            }
            return new DataGridResult(dt, count);
        }
        public string ContractSaleOutput(SearchItem item)
        {
            string sql;
            if (item.Values["SrchTYPE"] == "2")  //月数据
            {
                sql = $"SELECT C.YEARMONTH RQ,SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT,";
                sql += " C.MERCHANTID,C.CONTRACTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,B.NAME BRANDNAME, ";
                sql += " G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE,F.NAME FLOORNAME";
                sql += ContractSaleSqlParm(item);
                sql += " GROUP BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID,M.NAME,S.CODE,S.NAME,B.NAME,G.CATEGORYCODE,G.CATEGORYNAME,F.CODE,F.NAME";
                sql += " ORDER BY C.YEARMONTH,C.MERCHANTID,C.CONTRACTID ";

            }
            else
            {
                sql = @"select to_char(C.RQ,'yyyy-mm-dd') RQ,C.CONTRACTID,C.MERCHANTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,
                               G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE,F.NAME FLOORNAME,
                               B.NAME BRANDNAME,SUM(C.AMOUNT) AMOUNT,SUM(C.COST) COST,SUM(C.DIS_AMOUNT) DIS_AMOUNT,SUM(C.PER_AMOUNT) PER_AMOUNT";
                sql += ContractSaleSqlParm(item); ;

                sql += @" GROUP BY C.RQ,C.CONTRACTID,C.MERCHANTID,M.NAME,S.CODE,S.NAME,
                             G.CATEGORYCODE,G.CATEGORYNAME,F.CODE,F.NAME,B.NAME 
                      ORDER BY  C.RQ,C.MERCHANTID,C.CONTRACTID ";
            }

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "ContractSale";
            return GetExport("租约销售导出", a =>
            {
                a.SetTable(dt);
            });
        }
        #endregion

        #region 商品销售
        public DataGridResult GoodsSale(SearchItem item)
        {
            string SqlyTQx = GetYtQx("C");

            string sql = $"SELECT D.RQ,D.AMOUNT,D.COST,D.DIS_AMOUNT,D.PER_AMOUNT,M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME ";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K,CATEGORY C ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID and B.CATEGORYID=C.CATEGORYID";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";

            sql += "  and D.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            if (SqlyTQx != "")  //业态权限
            {
                sql += " and " + SqlyTQx;
            }


            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
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
                sqlsum += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K,CATEGORY C ";
                sqlsum += " WHERE G.MERCHANTID=M.MERCHANTID ";
                sqlsum += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
                sqlsum += "  and B.CATEGORYID=C.CATEGORYID";

                sqlsum += "  and D.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

                if (SqlyTQx != "")  //业态权限
                {
                    sqlsum += " and " + SqlyTQx;
                }

                item.HasKey("BRANCHID", a => sqlsum += $" and D.BRANCHID = {a}");
                item.HasKey("GOODSDM", a => sqlsum += $" and G.GOODSDM = '{a}'");
                item.HasKey("GOODSNAME", a => sqlsum += $" and G.NAME LIKE '%{a}%'");
                item.HasKey("CONTRACTID", a => sqlsum += $" and G.CONTRACTID = '{a}'");
                item.HasDateKey("RQ_START", a => sqlsum += $" and D.RQ >= {a}");
                item.HasDateKey("RQ_END", a => sqlsum += $" and D.RQ <= {a}");
                item.HasKey("MERCHANTID", a => sqlsum += $" and G.MERCHANTID LIKE '%{a}%'");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
                item.HasKey("CATEGORYCODE", a => sqlsum += $" and C.CATEGORYCODE LIKE '{a}%'");
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
            string SqlyTQx = GetYtQx("C");
            string sql = $"SELECT D.YEARMONTH,M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME,";
            sql += " sum(D.AMOUNT) AMOUNT,sum(D.COST) COST,sum(D.DIS_AMOUNT) DIS_AMOUNT,sum(D.PER_AMOUNT) PER_AMOUNT";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K,CATEGORY C ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            sql += "  and B.CATEGORYID=C.CATEGORYID";

            sql += "  and D.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            if (SqlyTQx != "")  //业态权限
            {
                sql += " and " + SqlyTQx;
            }
            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
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
                sqlsum += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K,CATEGORY C ";
                sqlsum += " WHERE G.MERCHANTID=M.MERCHANTID ";
                sqlsum += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
                sqlsum += "  and B.CATEGORYID=C.CATEGORYID";

                sqlsum += "  and D.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

                if (SqlyTQx != "") //业态权限
                {
                    sqlsum += " and " + SqlyTQx;
                }
                item.HasKey("BRANCHID", a => sqlsum += $" and D.BRANCHID = {a}");
                item.HasKey("GOODSDM", a => sqlsum += $" and G.GOODSDM = '{a}'");
                item.HasKey("GOODSNAME", a => sqlsum += $" and G.NAME LIKE '%{a}%'");
                item.HasKey("CONTRACTID", a => sqlsum += $" and G.CONTRACTID = '{a}'");
                item.HasDateKey("RQ_START", a => sqlsum += $" and D.RQ >= {a}");
                item.HasDateKey("RQ_END", a => sqlsum += $" and D.RQ <= {a}");
                item.HasKey("MERCHANTID", a => sqlsum += $" and G.MERCHANTID LIKE '%{a}%'");
                item.HasKey("MERCHANTNAME", a => sqlsum += $" and M.NAME LIKE '%{a}%'");
                item.HasKey("CATEGORYCODE", a => sqlsum += $" and C.CATEGORYCODE LIKE '{a}%'");
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
            string SqlyTQx = GetYtQx("C");
            string sql = $"SELECT to_char(D.RQ,'yyyy-mm-dd') RQ,D.AMOUNT,D.COST,D.DIS_AMOUNT,D.PER_AMOUNT,";
            sql += " M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME ";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K,CATEGORY C  ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            sql += "  and B.CATEGORYID=C.CATEGORYID";

            sql += "  and D.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            if (SqlyTQx != "") //业态权限
            {
                sql += " and " + SqlyTQx;
            }

            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
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
            string SqlyTQx = GetYtQx("C");
            string sql = $"SELECT D.YEARMONTH RQ,SUM(D.AMOUNT) AMOUNT,SUM(D.COST) COST,SUM(D.DIS_AMOUNT) DIS_AMOUNT,SUM(D.PER_AMOUNT) PER_AMOUNT,";
            sql += " M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
            sql += " B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME ";
            sql += " FROM GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K,CATEGORY C  ";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID ";
            sql += "   AND D.GOODSID=G.GOODSID  AND G.BRANDID=B.ID AND G.KINDID=K.ID";
            sql += "  and B.CATEGORYID=C.CATEGORYID";

            sql += "  and D.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            if (SqlyTQx != "") //业态权限
            {
                sql += " and " + SqlyTQx;
            }

            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID = {a}");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
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
        #endregion

        #region POS销售
        public DataGridResult SaleRecord(SearchItem item)
        {
            string sql = $"SELECT S.POSNO,S.DEALID,S.SALE_TIME,trunc(S.ACCOUNT_DATE) ACCOUNT_DATE,U.USERNAME CASHIERNAME, U.USERCODE CASHIERCODE,";
            sql += " S.SALE_AMOUNT,S.CHANGE_AMOUNT,S.POSNO_OLD,S.DEALID_OLD ";
            sql += " FROM SALE S,SYSUSER U,STATION T";
            sql += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH";

            sql += "  and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");

            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID in ({a}))");
            item.HasKey("SHOPID", a => sql += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");

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

            sql += "  and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");

            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID in ({a}))");
            item.HasKey("SHOPID", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");

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

                sqlsum += "  and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

                item.HasKey("BRANCHID", a => sqlsum += $" and T.BRANCHID={a}");
                item.HasKey("POSNO", a => sqlsum += $" and S.POSNO='{a}'");

                item.HasKey("MERCHANTID", a => sqlsum += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID in ({a}))");
                item.HasKey("SHOPID", a => sqlsum += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");

                item.HasDateKey("SALE_TIME_START", a => sqlsum += $" and trunc(S.SALE_TIME) >= {a}");
                item.HasDateKey("SALE_TIME_END", a => sqlsum += $" and trunc(S.SALE_TIME) <= {a}");
                item.HasDateKey("ACCOUNT_DATE_START", a => sqlsum += $" and S.ACCOUNT_DATE >= {a}");
                item.HasDateKey("ACCOUNT_DATE_END", a => sqlsum += $" and S.ACCOUNT_DATE <= {a}");
                item.HasKey("CASHIERID", a => sqlsum += $" and S.CASHIERID = {a}");

                sqlsum += " union all ";

                sqlsum += " SELECT nvl(sum(S.SALE_AMOUNT),0) SALE_AMOUNT";
                sqlsum += " FROM HIS_SALE S, SYSUSER U,STATION T";
                sqlsum += " WHERE S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH ";

                sqlsum += "  and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

                item.HasKey("BRANCHID", a => sqlsum += $" and T.BRANCHID={a}");
                item.HasKey("POSNO", a => sqlsum += $" and S.POSNO='{a}'");

                item.HasKey("MERCHANTID", a => sqlsum += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID in ({a}))");
                item.HasKey("SHOPID", a => sqlsum += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");

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

            sql += "  and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");

            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID in ({a}))");
            item.HasKey("SHOPID", a => sql += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");

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

            sql += "  and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID={a}");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");

            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM HIS_SALE_GOODS G,GOODS D WHERE S.POSNO=G.POSNO and S.DEALID=G.DEALID AND G.GOODSID=D.GOODSID AND D.MERCHANTID in ({a}))");
            item.HasKey("SHOPID", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");

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
        #endregion

        public DataGridResult MerchantRent(SearchItem item)
        {
            string SqlyTQx = GetYtQx("C");
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
                            + "   and Y.MERCHANTID = M.MERCHANTID"

                            + "   and Y.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += " and R.ID in (" + GetPermissionSql(PermissionType.Floor) + ")";  //楼层权限

            if (SqlyTQx != "") //业态权限
            {
                sql += " and " + SqlyTQx;
            }


            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and R.ID in ({a})");
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
            string SqlyTQx = GetYtQx("C");
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
                            + "   and Y.MERCHANTID = M.MERCHANTID"
                            + "   and Y.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += " and R.ID in (" + GetPermissionSql(PermissionType.Floor) + ")";  //楼层权限
            if (SqlyTQx != "") //业态权限
            {
                sql += " and " + SqlyTQx;
            }

            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and R.ID in ({a})");
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
        public DataTable SEARCHFEE()
        {
            {
                string sql = $@"SELECT TRIMID,NAME,PYM FROM FeeSubject A WHERE TRIMID<1000 ORDER BY TRIMID";
                DataTable dt = DbHelper.ExecuteTable(sql);
                return dt;
            }
        }

        public DataGridResult ContractInfo(SearchItem item)
        {
            var feedata = SEARCHFEE();
            var sql1 = "";
            if (feedata.Rows.Count > 0)
            {
                foreach (DataRow data in feedata.Rows)
                {
                    sql1 += " , (SELECT MAX(CC.PRICE)  FROM CONTRACT_COST CC"
                           + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = " + data["TRIMID"].ToString() + ") " + data["PYM"].ToString() + " ";
                }
            }
            string SqlyTQx = GetYtQx("Y");
            string sql = " SELECT C.CONTRACTID,F.CODE FLOORCODE,S.CODE SHOPCODE,D.NAME BRANDNAME,"
                       + " M.MERCHANTID,M.NAME MERCHANTNAME, C.AREAR,to_char(C.CONT_START,'YYYY-MM-DD') CONT_START,"
                       + " to_char(C.CONT_END,'YYYY-MM-DD') CONT_END,O.NAME RENTWAY, CR.RENTPRICE,FR.NAME RENTRULE "
                       + sql1
                       + "  FROM CONTRACT C, MERCHANT M,CONTRACT_SHOP CS, CONTRACT_RENTPRICE CR,"
                       + "       SHOP S, FLOOR F,CONTRACT_BRAND CB, OPERATIONRULE O,FEERULE FR,"
                       + "       BRAND D,CATEGORY Y"
                       + " WHERE C.MERCHANTID = M.MERCHANTID AND C.CONTRACTID = CS.CONTRACTID"
                       + "   AND C.CONTRACTID = CR.CONTRACTID AND CS.SHOPID = S.SHOPID"
                       + "   AND S.FLOORID = F.ID AND C.CONTRACTID = CB.CONTRACTID"
                       + "   AND C.OPERATERULE = O.ID AND C.FEERULE_RENT = FR.ID"
                       + "   AND CB.BRANDID=D.ID AND D.CATEGORYID=Y.CATEGORYID"
                       + "   AND C.HTLX=1 "  //AND C.STATUS !=5
                       + "    AND C.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += " and F.ID in (" + GetPermissionSql(PermissionType.Floor) + ")";  //楼层权限
            if (SqlyTQx != "") //业态权限
            {
                sql += " and " + SqlyTQx;
            }

            item.HasKey("CATEGORYCODE", a => sql += $" and Y.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID in ({a})");
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
            string SqlyTQx = GetYtQx("Y");
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
                       + "   AND C.HTLX=1 "  //AND C.STATUS !=5
                       + "    AND C.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += " and F.ID in (" + GetPermissionSql(PermissionType.Floor) + ")";  //楼层权限
            if (SqlyTQx != "") //业态权限
            {
                sql += " and " + SqlyTQx;
            }


            item.HasKey("CATEGORYCODE", a => sql += $" and Y.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sql += $" and F.ID in ({a})");
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

        #region 收款方式销售报表
        /// <summary>
        /// 普通列表
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult PayTypeSale(SearchItem item)
        {
            //历史交易数据
            String sql = @"SELECT MERCHANT.MERCHANTID,MERCHANT.NAME,BRAND.NAME BRANDNAME,HIS_SALE.SALE_TIME,HIS_SALE.DEALID,HIS_SALE.POSNO,PAY.NAME PAYNAME,HIS_SALE_GOODS_PAY.AMOUNT
                         from HIS_SALE,HIS_SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                                     where HIS_SALE.POSNO=HIS_SALE_GOODS_PAY.POSNO AND HIS_SALE.DEALID=HIS_SALE_GOODS_PAY.DEALID 
                                       AND HIS_SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND HIS_SALE_GOODS_PAY.PAYID=PAY.PAYID 
                                       AND CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                                       AND CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");



            //当天交易数据
            String sql1 = @"SELECT  MERCHANT.MERCHANTID,MERCHANT.NAME,BRAND.NAME BRANDNAME,SALE.SALE_TIME,SALE.DEALID,SALE.POSNO,PAY.NAME PAYNAME,SALE_GOODS_PAY.AMOUNT
                         from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO AND SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql1 += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql1 += $" and TRUNC(SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql1 += $" and TRUNC(SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql1 += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql1 += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql1 += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql1 += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql1 += $" and BRAND.NAME LIKE '%{a}%'");

            string sqlunion = "select * from (" + sql + " union all " + sql1 + " ) ORDER BY MERCHANTID,SALE_TIME DESC,PAYNAME";

            int count;
            DataTable dt = DbHelper.ExecuteTable(sqlunion, item.PageInfo, out count);

            if (count > 0)
            {
                //历史交易金额汇总                

                string sqlunions = "SELECT NVL(SUM(AMOUNT),0) AMOUNT FROM ( select * from(" + sql + " union all " + sql1 + "))";

                DataTable dtSum = DbHelper.ExecuteTable(sqlunions);
                decimal stsum = Convert.ToDecimal(dtSum.Rows[0]["AMOUNT"]);
                DataRow dr = dt.NewRow();
                dr["MERCHANTID"] = "合计";
                dr["AMOUNT"] = stsum.ToString();

                dt.Rows.Add(dr);
            }
            return new DataGridResult(dt, count);

        }
        /// <summary>
        /// 按日期汇总
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult PayTypeSaleS(SearchItem item)
        {
            //历史记录
            string sql = @"SELECT MERCHANT.MERCHANTID,MERCHANT.NAME,PAY.NAME PAYNAME,HIS_SALE.POSNO,HIS_SALE_GOODS_PAY.AMOUNT,HIS_SALE.SALE_TIME
                        from HIS_SALE,HIS_SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                                     where HIS_SALE.POSNO=HIS_SALE_GOODS_PAY.POSNO AND HIS_SALE.DEALID=HIS_SALE_GOODS_PAY.DEALID 
                                       AND HIS_SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND HIS_SALE_GOODS_PAY.PAYID=PAY.PAYID 
                                       AND CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                                       AND CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");
            //当天记录
            string sql1 = @"SELECT  MERCHANT.MERCHANTID,MERCHANT.NAME,PAY.NAME PAYNAME,SALE.POSNO,SALE_GOODS_PAY.AMOUNT,SALE.SALE_TIME
                         from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO AND SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql1 += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql1 += $" and TRUNC(SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql1 += $" and TRUNC(SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql1 += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql1 += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql1 += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql1 += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql1 += $" and BRAND.NAME LIKE '%{a}%'");

            string sqlsum = @"select MERCHANTID,NAME,PAYNAME,POSNO,SUM(AMOUNT) AMOUNT from (" + sql + " union all " + sql1 + " ) GROUP BY MERCHANTID,POSNO,NAME,PAYNAME ORDER BY MERCHANTID,PAYNAME";

            int count;
            DataTable dt = DbHelper.ExecuteTable(sqlsum, item.PageInfo, out count);
            //合计

            if (count > 0)
            {

                string sqlunions = "select NVL(SUM(AMOUNT),0) AMOUNT from （" + sqlsum + " )";

                DataTable dtSum = DbHelper.ExecuteTable(sqlunions);
                decimal stsum = Convert.ToDecimal(dtSum.Rows[0]["AMOUNT"]);
                DataRow dr = dt.NewRow();
                dr["MERCHANTID"] = "合计";
                dr["AMOUNT"] = stsum.ToString();

                dt.Rows.Add(dr);
            }

            return new DataGridResult(dt, count);

        }
        /// <summary>
        /// 明细导出
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string PayTypeSaleOutput(SearchItem item)
        {
            //历史交易数据
            String sql = @"SELECT MERCHANT.MERCHANTID,MERCHANT.NAME,BRAND.NAME BRANDNAME,to_char(HIS_SALE.SALE_TIME,'yyyy-mm-dd hh24:mi:ss') SALE_TIME,HIS_SALE.DEALID,HIS_SALE.POSNO,PAY.NAME PAYNAME,HIS_SALE_GOODS_PAY.AMOUNT
                         from HIS_SALE,HIS_SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                                     where HIS_SALE.POSNO=HIS_SALE_GOODS_PAY.POSNO AND HIS_SALE.DEALID=HIS_SALE_GOODS_PAY.DEALID 
                                       AND HIS_SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND HIS_SALE_GOODS_PAY.PAYID=PAY.PAYID 
                                       AND CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                                       AND CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");

            //当天交易数据
            String sql1 = @"SELECT  MERCHANT.MERCHANTID,MERCHANT.NAME,BRAND.NAME BRANDNAME,to_char(SALE.SALE_TIME,'yyyy-mm-dd hh24:mi:ss') SALE_TIME,SALE.DEALID,SALE.POSNO,PAY.NAME PAYNAME,SALE_GOODS_PAY.AMOUNT
                         from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO AND SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql1 += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql1 += $" and TRUNC(SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql1 += $" and TRUNC(SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql1 += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql1 += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql1 += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql1 += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql1 += $" and BRAND.NAME LIKE '%{a}%'");

            string sqlunion = "select * from (" + sql + " union all " + sql1 + " ) ORDER BY MERCHANTID,SALE_TIME DESC,PAYNAME";

            DataTable dt = DbHelper.ExecuteTable(sqlunion);
            dt.TableName = "PayTypeSale";
            return GetExport("收款方式销售导出", a =>
            {
                a.SetTable(dt);
            });
        }
        /// <summary>
        /// 汇总导出
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string PayTypeSaleSOutput(SearchItem item)
        {
            //历史记录
            string sql = @"SELECT  MERCHANT.MERCHANTID,MERCHANT.NAME,PAY.NAME PAYNAME,HIS_SALE.POSNO,HIS_SALE_GOODS_PAY.AMOUNT,HIS_SALE.SALE_TIME
                         from HIS_SALE,HIS_SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                                     where HIS_SALE.POSNO=HIS_SALE_GOODS_PAY.POSNO AND HIS_SALE.DEALID=HIS_SALE_GOODS_PAY.DEALID 
                                       AND HIS_SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND HIS_SALE_GOODS_PAY.PAYID=PAY.PAYID 
                                       AND CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                                       AND CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");
            //当天记录
            string sql1 = @"SELECT MERCHANT.MERCHANTID,MERCHANT.NAME,PAY.NAME PAYNAME,SALE.POSNO,SALE_GOODS_PAY.AMOUNT,SALE.SALE_TIME
                         from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO AND SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql1 += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql1 += $" and TRUNC(SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql1 += $" and TRUNC(SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql1 += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql1 += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("Pay", a => sql1 += $" and PAY.PAYID= {a}");
            item.HasKey("YEARMONTH_START", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql1 += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("BRANDID", a => sql1 += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql1 += $" and BRAND.NAME LIKE '%{a}%'");

            string sqlsum = @"select MERCHANTID,NAME,PAYNAME,POSNO,SUM(AMOUNT) AMOUNT from (" + sql + " union all " + sql1 + " ) GROUP BY MERCHANTID,NAME,PAYNAME ORDER BY MERCHANTID,PAYNAME";

            DataTable dt = DbHelper.ExecuteTable(sqlsum);
            dt.TableName = "PayTypeSale";
            return GetExport("收款方式销售导出", a =>
            {
                a.SetTable(dt);
            });
        }
        #endregion

        #region  商户缴费
        //查询链接字符串（提取公共部分）
        private string MerchantPayCostSQLStr(SearchItem item)
        {
            string sqlparam = @" from MERCHANT M,BILL B,FEESUBJECT F,CONTRACT_BRAND C,BRAND D
                                where M.MERCHANTID = B.MERCHANTID AND F.TRIMID = B.TERMID AND C.CONTRACTID = B.CONTRACTID
                                  and M.MERCHANTID = B.MERCHANTID AND D.ID = C.BRANDID
                                  and B.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlparam += $" and B.BRANCHID in ({a})");
            item.HasKey("TRIMID", a => sqlparam += $" and F.TRIMID in ({a})");
            item.HasKey("MERCHANTID", a => sqlparam += $" and M.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sqlparam += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sqlparam += $" and D.ID = {a}");
            item.HasKey("BRANDNAME", a => sqlparam += $" and D.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sqlparam += $" and B.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sqlparam += $" and B.YEARMONTH <= {a}");
            item.HasKey("NIANYUE_START", a => sqlparam += $" and B.NIANYUE >= {a}");
            item.HasKey("NIANYUE_END", a => sqlparam += $" and B.NIANYUE <= {a}");

            string ISPAYS = "";
            item.HasKey("ISpay", a => ISPAYS = $"{a}");
            if (!string.IsNullOrEmpty(ISPAYS))
            {
                if (ISPAYS == "4")
                {
                    sqlparam += $" and B.STATUS = " + ISPAYS + "";
                }
                else
                {
                    sqlparam += $" and B.STATUS <> " + ISPAYS + "";
                }
            }
            
            return sqlparam;
        }
        /// <summary>
        /// 商户缴费明细
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult MerchantPayCost(SearchItem item)
        {
            string sql = "";
            if (item.Values["SrchTYPE"] == ((int)列表或汇总.普通列表).ToString())
            {
                sql += @"select M.MERCHANTID,M.NAME MERCHANTNAME,B.NIANYUE,B.YEARMONTH,
                                F.NAME TRIMNAME,D.NAME BRANDNAME,
                                B.MUST_MONEY,B.RECEIVE_MONEY,B.MUST_MONEY-B.RECEIVE_MONEY UNPAID_MONEY ";
                sql += MerchantPayCostSQLStr(item);
                sql += @" order by MERCHANTID,NIANYUE,TRIMID ";
            }
            else
            {
                sql += @"select M.MERCHANTID,M.NAME MERCHANTNAME,D.NAME BRANDNAME,F.NAME TRIMNAME,
                                sum(B.MUST_MONEY) MUST_MONEY,sum(B.RECEIVE_MONEY) RECEIVE_MONEY,
                                sum(B.MUST_MONEY)-sum(B.RECEIVE_MONEY) UNPAID_MONEY ";
                sql += MerchantPayCostSQLStr(item);
                sql += @" group by M.MERCHANTID,M.NAME,D.NAME,F.NAME 
                          order by M.MERCHANTID,F.NAME";
            }

            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            if (count > 0)
            {
                string sqlsum = @"select sum(B.MUST_MONEY) MUST_MONEY,sum(B.RECEIVE_MONEY) RECEIVE_MONEY,
                                  sum(B.MUST_MONEY-B.RECEIVE_MONEY) UNPAID_MONEY ";

                sqlsum += MerchantPayCostSQLStr(item);

                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                if (dtSum.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["MERCHANTID"] = "合计";
                    dr["MUST_MONEY"] = dtSum.Rows[0]["MUST_MONEY"].ToString();
                    dr["RECEIVE_MONEY"] = dtSum.Rows[0]["RECEIVE_MONEY"].ToString();
                    dr["UNPAID_MONEY"] = dtSum.Rows[0]["UNPAID_MONEY"].ToString();
                    dt.Rows.Add(dr);
                }
            }
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 商户缴费明细导出
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string MerchantPayCostOutput(SearchItem item)
        {
            string sql = "";
            if (item.Values["SrchTYPE"] == ((int)列表或汇总.普通列表).ToString())
            {
                sql += @"select M.MERCHANTID,M.NAME MERCHANTNAME,B.NIANYUE,B.YEARMONTH,
                                F.NAME TRIMNAME,D.NAME BRANDNAME,
                                B.MUST_MONEY,B.RECEIVE_MONEY,B.MUST_MONEY-B.RECEIVE_MONEY UNPAID_MONEY ";
                sql += MerchantPayCostSQLStr(item);
                sql += @" order by MERCHANTID,NIANYUE,TRIMID ";
            }
            else
            {
                sql += @"select M.MERCHANTID,M.NAME MERCHANTNAME,D.NAME BRANDNAME,F.NAME TRIMNAME,
                                sum(B.MUST_MONEY) MUST_MONEY,sum(B.RECEIVE_MONEY) RECEIVE_MONEY,
                                sum(B.MUST_MONEY)-sum(B.RECEIVE_MONEY) UNPAID_MONEY ";
                sql += MerchantPayCostSQLStr(item);
                sql += @" group by M.MERCHANTID,M.NAME,D.NAME,F.NAME 
                          order by M.MERCHANTID,F.NAME";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "MerchantPayCost";
            return GetExport("租赁商户缴费记录导出", a =>
            {
                a.SetTable(dt);
            });
        }
        #endregion

        #region 商户租金经营状况
        //固定租金查询链接字符串（提取公共部分）
        private string MerchantBusinessStatusSQLStr(SearchItem item)
        {
            string sqlParam = "";
            item.HasKey("BRANCHID", a => sqlParam += $" and b.BRANCHID = {a}");
            item.HasKey("MERCHANTID", a => sqlParam += $" and m.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sqlParam += $" and m.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sqlParam += $" and d.ID = {a}");
            item.HasKey("BRANDNAME", a => sqlParam += $" and d.NAME LIKE '%{a}%'");
            item.HasKey("NIANYUE_START", a => sqlParam += $" and b.NIANYUE >= {a}");
            item.HasKey("NIANYUE_END", a => sqlParam += $" and b.NIANYUE <= {a}");
            item.HasKey("YEARMONTH_START", a => sqlParam += $" and b.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sqlParam += $" and b.YEARMONTH <= {a}");
            
            //固定租金
            string sqlGd= @"SELECT m.MERCHANTID,m.NAME MERCHANTNAME,d.NAME BRANDNAME,b.NIANYUE,
                                   c.AREA_RENTABLE,b.MUST_MONEY,y.AMOUNT,
                                   decode(c.AREA_RENTABLE, 0, 0, ROUND(y.AMOUNT / c.AREA_RENTABLE, 2)) AMOUNT_AREA,
                                   decode(y.AMOUNT, 0, 0, ROUND(b.MUST_MONEY / y.AMOUNT, 2)) AREA_MONEY
                              from BILL b,CONTRACT t,MERCHANT m,CONTRACT_BRAND cb,BRAND d,CONTRACT_SHOPAREA c,CONTRACT_SUMMARY_YM y
                             where t.CONTRACTID = b.CONTRACTID and b.MERCHANTID = m.MERCHANTID 
                               and cb.BRANDID = d.ID and cb.CONTRACTID = b.CONTRACTID
                               and b.MERCHANTID = m.MERCHANTID and c.CONTRACTID = b.CONTRACTID
                               and y.YEARMONTH = b.NIANYUE and y.CONTRACTID = b.CONTRACTID                     
                               and t.OPERATERULE in (select ID from OPERATIONRULE where PROCESSTYPE={0}) 
                               and b.status not in (1,6) 
                               and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";
            //提成租金
            string sqlTc = @"select TC.* ,b1.MUST_MONEY+TC.MUST_MONEY PAID_MONEY,
                                    decode(TC.AREA_RENTABLE,0,0,round(TC.AMOUNT/TC.AREA_RENTABLE,2)) AMOUNT_AREA,
                                    decode(TC.AMOUNT,0,0,round((b1.MUST_MONEY+TC.MUST_MONEY)/TC.AMOUNT,2)) AREA_MONEY  
                               from (
                             select m.MERCHANTID,m.NAME MERCHANTNAME,d.NAME BRANDNAME,
                                    b.CONTRACTID,b.NIANYUE,s.AREA_RENTABLE,b.MUST_MONEY,
                                    t.TCZJ,y.AMOUNT 
                               from BILL b,CONTRACT_TCZJ t,CONTRACT ct,MERCHANT m,CONTRACT_BRAND c,BRAND d,CONTRACT_SHOPAREA s,CONTRACT_SUMMARY_YM y
                              where b.CONTRACTID=t.CONTRACTID and b.CONTRACTID=ct.CONTRACTID 
                                and b.NIANYUE=t.YEARMONTH and c.CONTRACTID=b.CONTRACTID
                                and b.MERCHANTID=m.MERCHANTID and c.BRANDID=d.ID and c.BRANDID=c.BRANDID
                                and b.MERCHANTID=m.MERCHANTID and s.CONTRACTID=b.CONTRACTID 
                                and y.YEARMONTH=b.NIANYUE and y.CONTRACTID=b.CONTRACTID
                                and b.status not in (1,6) 
                                and ct.OPERATERULE not in (select ID from OPERATIONRULE where PROCESSTYPE={0}) 
                                and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ") {1} "+ 
                          "group by m.MERCHANTID,m.NAME,d.NAME,b.CONTRACTID,b.NIANYUE,s.AREA_RENTABLE,b.MUST_MONEY,t.TCZJ,y.AMOUNT"+
                                 ") TC,BILL b1"+
                            " where TC.MERCHANTID=b1.MERCHANTID and TC.NIANYUE=b1.NIANYUE " +
                               "and b1.CONTRACTID=TC.CONTRACTID and b1.TERMID=1000 " +
                               "and b1.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")"; //门店权限
           
            string sql = "";
            if (item.Values["SrchTYPE"] == ((int)租金收取方式.固定租金).ToString())
            {
                sql += string.Format(sqlGd, ((int)合作方式.纯租).ToString()) + sqlParam;
            }
            else
            {
                sql += string.Format(sqlTc, ((int)合作方式.纯租).ToString(), sqlParam);
            }
            return sql;
        }
        //提成租金查询链接字符串（提取公共部分）
        private string MerchantBusinessStatusSQLStrTC(SearchItem item)
        {
            string sql = @"select TC.* ,b1.MUST_MONEY+TC.MUST_MONEY PAID_MONEY,
                                  decode(TC.AREA_RENTABLE,0,0,round(TC.AMOUNT/TC.AREA_RENTABLE,2)) AMOUNT_AREA,
                                  decode(TC.AMOUNT,0,0,round((b1.MUST_MONEY+TC.MUST_MONEY)/TC.AMOUNT,2)) AREA_MONEY  
                             from (
                           select m.MERCHANTID,m.NAME MERCHANTNAME,d.NAME BRANDNAME,
                                  b.CONTRACTID,b.NIANYUE,s.AREA_RENTABLE,b.MUST_MONEY,
                                  t.TCZJ,y.AMOUNT 
                             from BILL b,CONTRACT_TCZJ t,CONTRACT ct,MERCHANT m,CONTRACT_BRAND c,BRAND d,CONTRACT_SHOPAREA s,CONTRACT_SUMMARY_YM y
                            where b.CONTRACTID=t.CONTRACTID and b.CONTRACTID=ct.CONTRACTID and b.NIANYUE=t.YEARMONTH and c.CONTRACTID=b.CONTRACTID
                              and b.MERCHANTID=m.MERCHANTID and c.BRANDID=d.ID and c.BRANDID=c.BRANDID
                              and b.MERCHANTID=m.MERCHANTID and s.CONTRACTID=b.CONTRACTID 
                              and y.YEARMONTH=b.NIANYUE and y.CONTRACTID=b.CONTRACTID
                              and b.status not in (1,6) 
                              and t.OPERATERULE not in (select ID from OPERATIONRULE where PROCESSTYPE={0}) 
                              and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += @" group by m.MERCHANTID,m.NAME,d.NAME,b.CONTRACTID,b.NIANYUE,s.AREA_RENTABLE,b.MUST_MONEY,t.TCZJ,y.AMOUNT
                      ) TC,BILL b1 
                      where TC.MERCHANTID=b1.MERCHANTID and TC.NIANYUE=b1.NIANYUE 
                            and b1.CONTRACTID=TC.CONTRACTID and b1.TERMID=1000 
                            and b1.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")"; //门店权限
            return sql;
        }
        /// <summary>
        /// 商户租金经营状况查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult MerchantBusinessStatus(SearchItem item)
        {
            string sql = MerchantBusinessStatusSQLStr(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 商户租金经营状况导出
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string MerchantBusinessStatusOutput(SearchItem item)
        {          
            string sql = MerchantBusinessStatusSQLStr(item);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "MerchantBusinessStatus";
            return GetExport("商户租金经营状况导出", a =>
            {
                a.SetTable(dt);
            });
        }
        #endregion

        #region 商品销售明细查询
        public DataGridResult GoodsSaleDetail(SearchItem item)
        {
            string sql = @" SELECT *
                              FROM (select HIS_SALE.SALE_TIME,HIS_SALE.POSNO,HIS_SALE.DEALID,BRAND.NAME BRANDNAME,
                                           GOODS.NAME GOODSNAME,PAY.NAME,HIS_SALE_GOODS_PAY.AMOUNT,
                                           NVL(HIS_SALE.POSNO_OLD,' ') POSNO_OLD,NVL(HIS_SALE.DEALID_OLD,0) DEALID_OLD
                                      from HIS_SALE,HIS_SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                                     where HIS_SALE.POSNO=HIS_SALE_GOODS_PAY.POSNO AND HIS_SALE.DEALID=HIS_SALE_GOODS_PAY.DEALID 
                                       AND HIS_SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND HIS_SALE_GOODS_PAY.PAYID=PAY.PAYID 
                                       AND CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                                       AND CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                                       AND CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("GOODSDM", a => sql += $" and GOODS.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and GOODS.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");

            sql += @" UNION ALL ";

            sql += @"select SALE.SALE_TIME,SALE.POSNO,SALE.DEALID,BRAND.NAME BRANDNAME,GOODS.NAME GOODSNAME,
                            PAY.NAME,SALE_GOODS_PAY.AMOUNT, NVL(SALE.POSNO_OLD,' ') POSNO_OLD,
                            NVL(SALE.DEALID_OLD,0) DEALID_OLD
                       from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO AND SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                        and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("GOODSDM", a => sql += $" and GOODS.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and GOODS.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");

            sql += @" ) ORDER BY POSNO,SALE_TIME DESC,DEALID";

            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                //历史交易金额汇总                

                string sqlunions = "SELECT NVL(SUM(AMOUNT),0) AMOUNT FROM ( select * from (" + sql + " ))";

                DataTable dtSum = DbHelper.ExecuteTable(sqlunions);
                decimal stsum = Convert.ToDecimal(dtSum.Rows[0]["AMOUNT"]);
                DataRow dr = dt.NewRow();
                dr["NAME"] = "合计";
                dr["AMOUNT"] = stsum.ToString();

                dt.Rows.Add(dr);
            }

            return new DataGridResult(dt, count);
        }
        public string GoodsSaleDetailOutput(SearchItem item)
        {
            string sql = @" SELECT * 
                              FROM (select HIS_SALE.SALE_TIME,HIS_SALE.POSNO,HIS_SALE.DEALID,BRAND.NAME BRANDNAME,
                                           GOODS.NAME GOODSNAME,PAY.NAME,HIS_SALE_GOODS_PAY.AMOUNT,
                                           NVL(HIS_SALE.POSNO_OLD,' ') POSNO_OLD,NVL(HIS_SALE.DEALID_OLD,0) DEALID_OLD
                                      from HIS_SALE,HIS_SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                                     where HIS_SALE.POSNO=HIS_SALE_GOODS_PAY.POSNO AND HIS_SALE.DEALID=HIS_SALE_GOODS_PAY.DEALID 
                                       and HIS_SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND HIS_SALE_GOODS_PAY.PAYID=PAY.PAYID 
                                       and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                                       and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                                       and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("GOODSDM", a => sql += $" and GOODS.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and GOODS.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");

            sql += @" UNION ALL ";

            sql += @"select SALE.SALE_TIME,SALE.POSNO,SALE.DEALID,BRAND.NAME BRANDNAME,GOODS.NAME GOODSNAME,
                            PAY.NAME,SALE_GOODS_PAY.AMOUNT, NVL(SALE.POSNO_OLD,' ') POSNO_OLD,
                            NVL(SALE.DEALID_OLD,0) DEALID_OLD
                       from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO AND SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                        and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID = {a}");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANT.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("GOODSDM", a => sql += $" and GOODS.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and GOODS.NAME LIKE '%{a}%'");
            item.HasKey("BRANDID", a => sql += $" and BRAND.ID = {a}");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");

            sql += @" ) ORDER BY POSNO,SALE_TIME DESC,DEALID";
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "GoodsSaleDetail";
            return GetExport("商品销售明细导出", a =>
            {
                a.SetTable(dt);
            });
        }
        #endregion

        #region 商户应交已付报表
        /// <summary>
        /// 商户应交已付报表查询
        /// </summary>
        public DataGridResult MerchantPayable(SearchItem item)
        {
            string sqlParam = "";
            string sqlSfxm = " select distinct b.termid,f.name" +
                             "   from bill b,merchant m,feesubject f" +
                             "  where b.MERCHANTID=m.MERCHANTID and b.TERMID=f.TRIMID" +
                             "    and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlParam += $" and b.BRANCHID in ({a})");
            item.HasKey("MERCHANTID", a => sqlParam += $" and b.MERCHANTID ={a}");
            item.HasKey("NIANYUE", a => sqlParam += $" and b.NIANYUE ={a}");
            item.HasKey("YEARMONTH", a => sqlParam += $" and b.YEARMONTH ={a}");
            item.HasKey("SFXMLX", a => sqlParam += $" and f.TYPE in ({a})");
            item.HasKey("SFXM", a => sqlParam += $" and b.TERMID in ({a})");

            sqlSfxm += sqlParam;
            var resData = DbHelper.ExecuteTable(sqlSfxm);

            string sqlMain = " select b.MERCHANTID,b.NIANYUE,b.YEARMONTH,";
            for (var i = 0; i < resData.Rows.Count; i++)
            {
                sqlMain += String.Format(" sum(decode(b.termid, {0}, b.MUST_MONEY, 0))  MUST_MONEY{0}," +
                                         " sum(decode(b.termid, {0}, b.RECEIVE_MONEY, 0))   RECEIVE_MONEY{0},", resData.Rows[i][0].ToString());
            }
            sqlMain += "       sum(b.MUST_MONEY) MUST_MONEYsum,sum(b.RECEIVE_MONEY) RECEIVE_MONEYsum, m.NAME MERCHANTNAME" +
                       "  from bill b,merchant m,feesubject f" +
                       " where b.MERCHANTID=m.MERCHANTID and b.TERMID=f.TRIMID " +
                       "   and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sqlMain += sqlParam;
            sqlMain += " group by b.MERCHANTID,b.NIANYUE,b.YEARMONTH,b.MUST_MONEY,b.RECEIVE_MONEY,m.NAME";
            int count;
            var dt = DbHelper.ExecuteTable(sqlMain, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 商户应交已付报表导出
        /// </summary>
        public string MerchantPayableOutput(SearchItem item)
        {
            string sql = "";
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.TableName = "MerchantPayable";
            return GetExport("商户应交已付报表", a =>
            {
                a.SetTable(dt);
            });
        }
        public DataTable getData(SearchItem item)
        {
            string sqlParam = "";
            string sqlSfxm = " select distinct b.termid,f.name" +
                             "   from bill b,merchant m,feesubject f" +
                             "  where b.MERCHANTID=m.MERCHANTID and b.TERMID=f.TRIMID" +
                             "    and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlParam += $" and b.BRANCHID ={a}");
            item.HasKey("MERCHANTID", a => sqlParam += $" and b.MERCHANTID ={a}");
            item.HasKey("NIANYUE", a => sqlParam += $" and b.NIANYUE ={a}");
            item.HasKey("YEARMONTH", a => sqlParam += $" and b.YEARMONTH ={a}");
            item.HasKey("SFXMLX", a => sqlParam += $" and f.TYPE ={a}");
            item.HasKey("SFXM", a => sqlParam += $" and b.TERMID ={a}");

            sqlSfxm += sqlParam;
            DbHelper.ExecuteTable(sqlSfxm);
            var resData = DbHelper.ExecuteTable(sqlSfxm);

            string sqlMain = " select b.MERCHANTID,b.NIANYUE,b.YEARMONTH,";
            for (var i = 0; i < resData.Rows.Count; i++)
            {
                sqlMain += String.Format(" sum(decode(b.termid, {0}, b.MUST_MONEY, 0))  MUST_MONEY{0}," +
                                         " sum(decode(b.termid, {0}, b.RECEIVE_MONEY, 0))   RECEIVE_MONEY{0},", resData.Rows[i][0].ToString());
            }
            sqlMain += "       sum(b.MUST_MONEY) MUST_MONEYsum,sum(b.RECEIVE_MONEY) RECEIVE_MONEYsum,m.NAME MERCHANTNAME " +
                       "  from bill b,merchant m,feesubject f" +
                       " where b.MERCHANTID=m.MERCHANTID and b.TERMID=f.TRIMID " +
                       "   and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sqlMain += sqlParam;
            sqlMain += " group by b.MERCHANTID,b.NIANYUE,b.YEARMONTH,b.MUST_MONEY,b.RECEIVE_MONEY,m.NAME";
            return DbHelper.ExecuteTable(sqlMain);
        }
        /// <summary>
        ///获取收费项目list
        /// </summary>
        public DataTable GetSfxmList()
        {
            string sql = "select * from feesubject";
            return DbHelper.ExecuteTable(sql);
        }
        #endregion

        #region 销售采集处理记录查询
        public DataGridResult SALEGATHER(SearchItem item)
        {

            string sqlsum = $@"SELECT BRANCH.NAME, SALEGATHER.DEALID, SALEGATHER.SALETIME, SALEGATHER.FLAG,SALEGATHER.CREATE_TIME, SALEGATHER.REASON, STATION.STATIONBH
                    FROM SALEGATHER 
                    INNER JOIN STATION ON STATION.STATIONBH=SALEGATHER.POSNO 
                    INNER JOIN BRANCH ON BRANCH.ID=STATION.BRANCHID";
            sqlsum += "  AND STATION.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sqlsum += "   WHERE TYPE= 3";

            item.HasDateKey("SALETIME", a => sqlsum += $" and SALETIME={a}");
            item.HasKey("DEALID", a => sqlsum += $" and DEALID={a}");
            item.HasDateKey("CREATE_TIME", a => sqlsum += $" and CREATE_TIME={a}");
            item.HasKey("FLAG", a => sqlsum += $" and FLAG={a}");
            item.HasKey("REASON", a => sqlsum += $" and REASON LIKE '%{a}%'");

            item.HasKey("STATIONBH", a => sqlsum += $" and STATIONBH={a}");

            int count;
            DataTable dt = DbHelper.ExecuteTable(sqlsum, item.PageInfo, out count);
            dt.NewEnumColumns<处理标记>("FLAG", "FLAGMC");
            return new DataGridResult(dt, count);
        }

        #endregion

        #region 销售采集处理记录查询
        public string SALEGATHEROutput(SearchItem item)
        {

            string sqlsum = $@"SELECT BRANCH.NAME, SALEGATHER.DEALID, SALEGATHER.SALETIME, SALEGATHER.FLAG,SALEGATHER.CREATE_TIME, SALEGATHER.REASON, STATION.STATIONBH
                    FROM SALEGATHER 
                    INNER JOIN STATION ON STATION.STATIONBH=SALEGATHER.POSNO 
                    INNER JOIN BRANCH ON BRANCH.ID=STATION.BRANCHID";
            sqlsum += "  AND STATION.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sqlsum += "   WHERE TYPE= 3";

            //item.HasDateKey("SALETIME", a => sqlsum += $" and SALETIME={a}");
            item.HasKey("DEALID", a => sqlsum += $" and DEALID={a}");


            item.HasDateKey("SALETIME_START", a => sqlsum += $" and SALETIME >= {a}");
            item.HasDateKey("SALETIME_END", a => sqlsum += $" and SALETIME <= {a}");

            item.HasDateKey("CREATE_TIME_START", a => sqlsum += $" and CREATE_TIME >={a}");
            item.HasDateKey("CREATE_TIME_END", a => sqlsum += $" and CREATE_TIME <={a}");

            item.HasKey("FLAG", a => sqlsum += $" and FLAG={a}");
            item.HasKey("REASON", a => sqlsum += $" and REASON LIKE '%{a}%'");

            item.HasKey("STATIONBH", a => sqlsum += $" and STATIONBH={a}");


            DataTable dt = DbHelper.ExecuteTable(sqlsum);
            dt.NewEnumColumns<处理标记>("FLAG", "FLAGMC");
            dt.TableName = "SALEGATHER";
            return GetExport("销售采集处理记录查询", a =>
            {
                a.SetTable(dt);
            });
        }

        #endregion

        #region 第三方支付记录查询
        public DataGridResult PAYINFO(SearchItem item)
        {

            string sqlsum = $@"SELECT A.OPERTIME,A.POSNO,A.DEALID,B.NAME,A.CARDNO,A.BANK,A.AMOUNT,A.SERIALNO,A.REFNO, D.NAME BRANCHNAME
                                FROM PAYRECORD A 
                                INNER JOIN PAY B ON(A.PAYID= B.PAYID)
                                INNER JOIN STATION C ON(A.POSNO =C.STATIONBH)
                                INNER JOIN BRANCH D ON(D.ID=C.BRANCHID)" +
                             " and c.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            //sqlsum += "  AND STATION.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlsum += $" and C.BRANCHID ={a}");
            item.HasDateKey("START", a => sqlsum += $" and OPERTIME>={a}");
            item.HasDateKey("END", a => sqlsum += $" and OPERTIME<={a}");

            item.HasKey("POSNO", a => sqlsum += $" and POSNO={a}");
            item.HasKey("DEALID", a => sqlsum += $" and DEALID={a}");
            //item.HasKey("INX", a => sqlsum += $" and INX={a}");
            //item.HasKey("NAME", a => sqlsum += $" and NAME={a}");
            //item.HasKey("CARDNO", a => sqlsum += $" and CARDNO={a}");
            //item.HasKey("BANK", a => sqlsum += $" and BANK={a}");
            item.HasKey("AMOUNT", a => sqlsum += $" and AMOUNT={a}");
            // item.HasKey("SERIALNO", a => sqlsum += $" and SERIALNO={a}");
            //item.HasKey("REFNO", a => sqlsum += $" and REFNO={a}");
            //item.HasKey("PAYID", a => sqlsum += $" and B.PAYID={a}");
            item.HasKey("PAYID", a => sqlsum += $" and B.PAYID ={a}");


            int count;
            DataTable dt = DbHelper.ExecuteTable(sqlsum, item.PageInfo, out count);
            //dt.NewEnumColumns<处理标记>("FLAG", "FLAGMC");

            return new DataGridResult(dt, count);
        }

        #endregion

        #region 第三方支付记录查询导出
        public string PAYINFOOutput(SearchItem item)
        {

            string sqlsum = $@"SELECT A.OPERTIME,A.POSNO,A.DEALID,B.NAME,A.CARDNO,A.BANK,A.AMOUNT,A.SERIALNO,A.REFNO, D.NAME BRANCHNAME
                                FROM PAYRECORD A 
                                INNER JOIN PAY B ON(A.PAYID= B.PAYID)
                                INNER JOIN STATION C ON(A.POSNO =C.STATIONBH)
                                INNER JOIN BRANCH D ON(D.ID=C.BRANCHID)" +
                             " and c.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            //sqlsum += "  AND STATION.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlsum += $" and C.BRANCHID ={a}");
            item.HasDateKey("START", a => sqlsum += $" and OPERTIME>={a}");
            item.HasDateKey("END", a => sqlsum += $" and OPERTIME<={a}");

            item.HasKey("POSNO", a => sqlsum += $" and POSNO={a}");
            item.HasKey("DEALID", a => sqlsum += $" and DEALID={a}");
            //item.HasKey("INX", a => sqlsum += $" and INX={a}");
            //item.HasKey("NAME", a => sqlsum += $" and NAME={a}");
            //item.HasKey("CARDNO", a => sqlsum += $" and CARDNO={a}");
            //item.HasKey("BANK", a => sqlsum += $" and BANK={a}");
            item.HasKey("AMOUNT", a => sqlsum += $" and AMOUNT={a}");
            // item.HasKey("SERIALNO", a => sqlsum += $" and SERIALNO={a}");
            //item.HasKey("REFNO", a => sqlsum += $" and REFNO={a}");
            //item.HasKey("PAYID", a => sqlsum += $" and B.PAYID={a}");
            item.HasKey("PAYID", a => sqlsum += $" and B.PAYID ={a}");

            DataTable dt = DbHelper.ExecuteTable(sqlsum);

            dt.TableName = "PAYINFO";
            return GetExport("第三方支付记录查询", a =>
            {
                a.SetTable(dt);
            });

        }

        #endregion

        #region 费用账单查询
        public DataGridResult Bill_Src(SearchItem item)
        {
            string sqlsum = $@"SELECT B.NAME BRANCHNAME, C.MERCHANTID,C.NAME MERCHANTNAME,A.BILLID, D.NAME FEENAME, A.CONTRACTID, 
                                         A.NIANYUE, A.YEARMONTH, A.MUST_MONEY, A.RECEIVE_MONEY,
                                        A.RETURN_MONEY,A.START_DATE,A.END_DATE,A.TYPE,A.STATUS,F.NAME UNITNAME，A.DESCRIPTION
                                        FROM BILL A, BRANCH B, MERCHANT C, FEESUBJECT D, FEESUBJECT_ACCOUNT E,FEE_ACCOUNT F
                                        WHERE A.BRANCHID = B.ID AND
                                        A.MERCHANTID=C.MERCHANTID AND
                                        A.TERMID=D.TRIMID AND 
                                        E.TERMID=D.TRIMID AND 
                                        E.FEE_ACCOUNTID=F.ID AND
                                        A.BRANCHID =E.BRANCHID";

            sqlsum += "  AND A.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlsum += $" and a.BRANCHID ={a}");
            item.HasKey("MERCHANTID", a => sqlsum += $" and c.MERCHANTID ={a}");
            //item.HasKey("MERCHANTNAME", a => sqlsum += $" and c.NAME ={a}");
            item.HasKey("BILLID", a => sqlsum += $" and a.BILLID ={a}");
            item.HasKey("TRIMID", a => sqlsum += $" and d.TRIMID in ({a})");
             item.HasKey("YEARMONTH_START", a => sqlsum += $" and A.YEARMONTH = {a}");
            //item.HasKey("YEARMONTH_END", a => sqlsum += $" and A.YEARMONTH <= {a}");
            item.HasKey("NIANYUE_START", a => sqlsum += $" and A.NIANYUE = {a}");
            //item.HasKey("NIANYUE_END", a => sqlsum += $" and A.NIANYUE <= {a}");
            item.HasKey("TYPE", a => sqlsum += $" and A.TYPE = {a}");
            item.HasKey("STATUS", a => sqlsum += $" and A.STATUS = {a}");
            item.HasKey("CONTRACTID", a => sqlsum += $" and A.CONTRACTID = {a}");
            sqlsum += "     order by nianyue desc";

            int count;
            DataTable dt = DbHelper.ExecuteTable(sqlsum, item.PageInfo, out count);
            dt.NewEnumColumns<账单状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<账单类型>("TYPE", "TYPEMC");

            return new DataGridResult(dt, count);

        }

    #endregion

    #region 费用账单查询导出
    public string Bill_SrcOutput(SearchItem item)
        {
            string sqlsum = $@"SELECT B.NAME BRANCHNAME, C.MERCHANTID,C.NAME MERCHANTNAME, A.BILLID, D.NAME FEENAME, A.CONTRACTID, 
                                         A.NIANYUE, A.YEARMONTH, A.MUST_MONEY, A.RECEIVE_MONEY,
                                        A.RETURN_MONEY,A.START_DATE,A.END_DATE,A.TYPE,A.STATUS,F.NAME UNITNAME，A.DESCRIPTION
                                        FROM BILL A, BRANCH B, MERCHANT C, FEESUBJECT D, FEESUBJECT_ACCOUNT E,FEE_ACCOUNT F
                                        WHERE A.BRANCHID = B.ID AND
                                        A.MERCHANTID=C.MERCHANTID AND
                                        A.TERMID=D.TRIMID AND 
                                        E.TERMID=D.TRIMID AND 
                                        E.FEE_ACCOUNTID=F.ID AND
                                        A.BRANCHID =E.BRANCHID";

            sqlsum += "  AND A.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlsum += $" and a.BRANCHID ={a}");
            item.HasKey("MERCHANTID", a => sqlsum += $" and c.MERCHANTID ={a}");
            //item.HasKey("MERCHANTNAME", a => sqlsum += $" and c.NAME ={a}");
            item.HasKey("BILLID", a => sqlsum += $" and a.BILLID ={a}");
            item.HasKey("TRIMID", a => sqlsum += $" and D.TRIMID in ({a})");      
            item.HasKey("YEARMONTH_START", a => sqlsum += $" and A.YEARMONTH = {a}");
            //item.HasKey("YEARMONTH_END", a => sqlsum += $" and A.YEARMONTH <= {a}");
            item.HasKey("NIANYUE_START", a => sqlsum += $" and A.NIANYUE = {a}");
            //item.HasKey("NIANYUE_END", a => sqlsum += $" and A.NIANYUE <= {a}");  
            item.HasKey("TYPE", a => sqlsum += $" and A.TYPE = {a}");
            item.HasKey("STATUS", a => sqlsum += $" and A.STATUS = {a}");
            item.HasKey("CONTRACTID", a => sqlsum += $" and A.CONTRACTID = {a}");
            sqlsum += "     order by nianyue desc";

            DataTable dt = DbHelper.ExecuteTable(sqlsum);
            dt.NewEnumColumns<账单状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<账单类型>("TYPE", "TYPEMC");


            dt.TableName = "Bill_Src";
            return GetExport("费用账单查询", a =>
            {
                a.SetTable(dt);
            });
        }
        #endregion
    }
}
