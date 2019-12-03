using System;
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
        private string ContractSaleSqlParam(SearchItem item)
        {
            string sqlParam = " from CONTRACT_SUMMARY C,MERCHANT M,SHOP S,BRAND B,CATEGORY G,FLOOR F";
            sqlParam += " where C.MERCHANTID=M.MERCHANTID and C.SHOPID=S.SHOPID and C.BRANDID=B.ID";
            sqlParam += "       and B.CATEGORYID=G.CATEGORYID and S.FLOORID=F.ID";
            sqlParam += "       and C.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sqlParam += "       and F.ID in (" + GetPermissionSql(PermissionType.Floor) + ")";  //楼层权限
            string SqlyTQx = GetYtQx("G");
            if (SqlyTQx != "")  //业态权限
            {
                sqlParam += " and " + SqlyTQx;
            }

            item.HasKey("CATEGORYCODE", a => sqlParam += $" and G.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("FLOORID", a => sqlParam += $" and F.ID in ({a})");
            item.HasKey("BRANCHID", a => sqlParam += $" and C.BRANCHID in ({a})");
            item.HasKey("CONTRACTID", a => sqlParam += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sqlParam += $" and C.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sqlParam += $" and C.RQ <= {a}");
            item.HasKey("MERCHANTNAME", a => sqlParam += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDNAME", a => sqlParam += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sqlParam += $" and C.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sqlParam += $" and C.YEARMONTH <= {a}");
            return sqlParam;
        }
        private string ContractSaleSql(SearchItem item)
        {
            string sql;
            if (item.Values["SrchTYPE"] == ((int)查询类型.日数据).ToString())
            {
                sql = @"select to_char(C.RQ,'yyyy-mm-dd') RQ,C.CONTRACTID,C.MERCHANTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,
                               G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE,F.NAME FLOORNAME,
                               B.NAME BRANDNAME,sum(C.AMOUNT) AMOUNT,sum(C.COST) COST,sum(C.DIS_AMOUNT) DIS_AMOUNT,sum(C.PER_AMOUNT) PER_AMOUNT";
                sql += ContractSaleSqlParam(item);
                sql += @" group by C.RQ,C.CONTRACTID,C.MERCHANTID,M.NAME,S.CODE,S.NAME,G.CATEGORYCODE,G.CATEGORYNAME,F.CODE,F.NAME,B.NAME 
                          order by C.RQ,C.MERCHANTID,C.CONTRACTID ";
            }
            else
            {
                sql = $"select C.YEARMONTH,sum(C.AMOUNT) AMOUNT,sum(C.COST) COST,sum(C.DIS_AMOUNT) DIS_AMOUNT,sum(C.PER_AMOUNT) PER_AMOUNT,";
                sql += "       C.MERCHANTID,C.CONTRACTID,M.NAME MERCHANTNAME,S.CODE SHOPCODE,S.NAME SHOPNAME,B.NAME BRANDNAME, ";
                sql += "       G.CATEGORYCODE,G.CATEGORYNAME,F.CODE FLOORCODE,F.NAME FLOORNAME";
                sql += ContractSaleSqlParam(item);
                sql += " group by C.YEARMONTH,C.MERCHANTID,C.CONTRACTID,M.NAME,S.CODE,S.NAME,B.NAME,G.CATEGORYCODE,G.CATEGORYNAME,F.CODE,F.NAME";
                sql += " order by C.YEARMONTH,C.MERCHANTID,C.CONTRACTID ";
            }
            return sql;
        }
        public DataGridResult ContractSale(SearchItem item)
        {
            string sql = ContractSaleSql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            if (count > 0)
            {
                string sqlsum = $"select nvl(sum(C.AMOUNT),0) AMOUNT,nvl(sum(C.COST),0) COST,nvl(sum(C.DIS_AMOUNT),0) DIS_AMOUNT," +
                                 "       nvl(sum(C.PER_AMOUNT),0) PER_AMOUNT ";

                sqlsum += ContractSaleSqlParam(item);

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
        public DataTable ContractSaleOutput(SearchItem item)
        {
            string sql = ContractSaleSql(item);
            return DbHelper.ExecuteTable(sql);
        }
        #endregion

        #region 商品销售
        private string GoodsSaleSqlParam(SearchItem item)
        {
            string sql = " from GOODS_SUMMARY D,GOODS G,MERCHANT M,BRAND B,GOODS_KIND K,CATEGORY C ";
            sql += "      where G.MERCHANTID=M.MERCHANTID and B.CATEGORYID=C.CATEGORYID";
            sql += "            and D.GOODSID=G.GOODSID  and G.BRANDID=B.ID and G.KINDID=K.ID";
            sql += "            and D.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            string SqlyTQx = GetYtQx("C");
            if (SqlyTQx != "")  //业态权限
            {
                sql += " and " + SqlyTQx;
            }

            item.HasKey("BRANCHID", a => sql += $" and D.BRANCHID in ({a})");
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and G.NAME LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and D.RQ >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and D.RQ <= {a}");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("BRANDNAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and D.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and D.YEARMONTH <= {a}");

            return sql;
        }
        private string GoodsSaleSql(SearchItem item)
        {
            string sql;
            if (item.Values["SrchTYPE"] == ((int)查询类型.日数据).ToString())
            {
                sql = $"select to_char(D.RQ,'yyyy-mm-dd') RQ,M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,";
                sql += "       G.GOODSDM,G.BARCODE,G.NAME GOODSNAME，D.AMOUNT,D.COST,D.DIS_AMOUNT,D.PER_AMOUNT ";
                sql += GoodsSaleSqlParam(item);
                sql += " order by D.RQ,G.MERCHANTID,G.CONTRACTID,G.GOODSDM ";
            }
            else
            {
                sql = $"select D.YEARMONTH,M.NAME MERCHANTNAME,G.CONTRACTID,G.MERCHANTID,";
                sql += "       B.NAME BRANDNAME,K.CODE KINDCODE,K.NAME KINDNAME,G.GOODSDM,G.BARCODE,G.NAME GOODSNAME,";
                sql += "       sum(D.AMOUNT) AMOUNT,sum(D.COST) COST,sum(D.DIS_AMOUNT) DIS_AMOUNT,sum(D.PER_AMOUNT) PER_AMOUNT";
                sql += GoodsSaleSqlParam(item);
                sql += " group by D.YEARMONTH,M.NAME,G.CONTRACTID,G.MERCHANTID,";
                sql += "          B.NAME,K.CODE,K.NAME,G.GOODSDM,G.BARCODE,G.NAME";
                sql += " order by D.YEARMONTH,G.MERCHANTID,G.CONTRACTID,G.GOODSDM ";
            }
            return sql;
        }
        public DataGridResult GoodsSale(SearchItem item)
        {
            string sql = GoodsSaleSql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                string sqlsum = @"select sum(D.AMOUNT) AMOUNT,sum(D.COST) COST,
                                         sum(D.DIS_AMOUNT) DIS_AMOUNT,sum(D.PER_AMOUNT) PER_AMOUNT";
                sqlsum += GoodsSaleSqlParam(item);
                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                if (dtSum.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["GOODSDM"] = "合计";
                    dr["AMOUNT"] = dtSum.Rows[0]["AMOUNT"].ToString();
                    dr["COST"] = dtSum.Rows[0]["COST"].ToString();
                    dr["DIS_AMOUNT"] = dtSum.Rows[0]["DIS_AMOUNT"].ToString();
                    dr["PER_AMOUNT"] = dtSum.Rows[0]["PER_AMOUNT"].ToString();
                    dt.Rows.Add(dr);
                }
            }
            return new DataGridResult(dt, count);
        }
        public DataTable GoodsSaleOutput(SearchItem item)
        {
            string sql = GoodsSaleSql(item);
            return DbHelper.ExecuteTable(sql);
        }
        #endregion

        #region POS销售
        private string SaleRecordParam(SearchItem item)
        {
            string sql = "{0} from SALE S,SYSUSER U,STATION T";
            sql += " where S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH";
            sql += "       and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID in ({a})");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and exists(select 1 from SALE_GOODS G,GOODS D where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.GOODSID=D.GOODSID and D.MERCHANTID in ({a}))");
            item.HasKey("SHOPID", a => sql += $" and exists(select 1 from SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");
            item.HasDateKey("SALE_TIME_START", a => sql += $" and trunc(S.SALE_TIME) >= {a}");
            item.HasDateKey("SALE_TIME_END", a => sql += $" and trunc(S.SALE_TIME) <= {a}");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and S.ACCOUNT_DATE >= {a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and S.ACCOUNT_DATE <= {a}");
            item.HasKey("CASHIERID", a => sql += $" and S.CASHIERID = {a}");

            sql += " union all ";
            sql += " {0} from HIS_SALE S, SYSUSER U,STATION T";
            sql += " where S.CASHIERID = U.USERID and S.POSNO=T.STATIONBH";
            sql += "  and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and T.BRANCHID in ({a})");
            item.HasKey("POSNO", a => sql += $" and S.POSNO='{a}'");
            item.HasKey("MERCHANTID", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,GOODS D where S.POSNO=G.POSNO and S.DEALID=G.DEALID and G.GOODSID=D.GOODSID and D.MERCHANTID in ({a}))");
            item.HasKey("SHOPID", a => sql += $" and exists(select 1 from HIS_SALE_GOODS G,SHOP P where G.SHOPID=P.SHOPID and S.POSNO=G.POSNO and S.DEALID=G.DEALID and P.SHOPID in ({a}))");
            item.HasDateKey("SALE_TIME_START", a => sql += $" and trunc(S.SALE_TIME) >= {a}");
            item.HasDateKey("SALE_TIME_END", a => sql += $" and trunc(S.SALE_TIME) <= {a}");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and S.ACCOUNT_DATE >= {a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and S.ACCOUNT_DATE <= {a}");
            item.HasKey("CASHIERID", a => sql += $" and S.CASHIERID = {a}");

            return sql;
        }
        private string SaleRecordSql(SearchItem item)
        {
            string selSql = @"select S.POSNO,S.DEALID,to_char(S.SALE_TIME,'yyyy-mm-dd hh24:mi:ss') SALE_TIME,
                                     to_char(S.ACCOUNT_DATE,'yyyy-mm-dd') ACCOUNT_DATE,
                                     U.USERNAME CASHIERNAME,U.USERCODE CASHIERCODE,
                                     S.SALE_AMOUNT,S.CHANGE_AMOUNT,S.POSNO_OLD,S.DEALID_OLD ";
            string sql = string.Format(SaleRecordParam(item), selSql);
            sql += " order by 1,2 ";
            return sql;
        }
        public DataGridResult SaleRecord(SearchItem item)
        {
            string sql = SaleRecordSql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            if (count > 0)
            {
                string sqlsum = $"select nvl(sum(S.SALE_AMOUNT),0) SALE_AMOUNT";
                sqlsum = string.Format(SaleRecordParam(item), sqlsum);
                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                DataRow dr = dt.NewRow();
                dr["POSNO"] = "合计";
                dr["SALE_AMOUNT"] = (Convert.ToSingle(dtSum.Rows[0]["SALE_AMOUNT"]) + Convert.ToSingle(dtSum.Rows[1]["SALE_AMOUNT"])).ToString();
                dt.Rows.Add(dr);
            }
            return new DataGridResult(dt, count);

        }
        public DataTable SaleRecordOutput(SearchItem item)
        {
            string sql = SaleRecordSql(item);
            return DbHelper.ExecuteTable(sql);
        }
        #endregion

        #region 商户租金计提表
        public string MerchantRentsql(SearchItem item)
        {
            string SqlyTQx = GetYtQx("C");
            string sql = "select Z.*,(Z.TCRENTS-RENTS) CE,(case when (Z.TCRENTS-RENTS)>0 then (Z.TCRENTS-RENTS) else 0  end) YJT from ("
                            + "select Y.YEARMONTH,R.CODE FLOORCODE,R.NAME FLOORNAME,P.CODE SHOPCODE,D.NAME BRANDNAME,Y.CONTRACTID,"
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
            item.HasKey("BRANCHID", a => sql += $" and Y.BRANCHID in ({a})");
            item.HasKey("CONTRACTID", a => sql += $" and Y.CONTRACTID = '{a}'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDNAME", a => sql += $" and D.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and Y.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and Y.YEARMONTH <= {a}");
            sql += ") Z order by Z.YEARMONTH,Z.FLOORCODE,Z.SHOPCODE,Z.MERCHANTID";
            return sql;
        }
        public DataGridResult MerchantRent(SearchItem item)
        {
            var sql = MerchantRentsql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataTable MerchantRentOutput(SearchItem item)
        {
            var sql = MerchantRentsql(item);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        #endregion

        #region 合同信息表ContractInfo
        public DataTable SEARCHFEE()
        {
            string sql = $@"SELECT TRIMID,NAME,PYM FROM FeeSubject A WHERE TRIMID<1000 ORDER BY TRIMID";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public string ContractInfoSql(SearchItem item)
        {
            var feedata = SEARCHFEE();
            var sql1 = "";
            if (feedata.Rows.Count > 0)
            {
                foreach (DataRow data in feedata.Rows)
                {
                    sql1 += ", (SELECT MIN(CC.COST)  FROM CONTRACT_COST CC"
                           + "   WHERE CC.CONTRACTID = C.CONTRACTID AND CC.TERMID = " + data["TRIMID"].ToString() + ") " + data["PYM"].ToString() + " ";
                }
            }
            string SqlyTQx = GetYtQx("Y");
            string sql = " SELECT C.CONTRACTID,CI.SHOPCODESTR SHOPCODE,CI.BRANDNAMESTR BRANDNAME,CI.FLOORCODESTR FLOORCODE,"
                       + "        M.MERCHANTID,M.NAME MERCHANTNAME, C.AREAR,to_char(C.CONT_START,'YYYY-MM-DD') CONT_START,"
                       + "        to_char(C.CONT_END,'YYYY-MM-DD') CONT_END,O.NAME RENTWAY, CI.RENTPRICESTR RENTPRICE,FR.NAME RENTRULE,C.STATUS "
                       + sql1
                       + "  FROM CONTRACT C, MERCHANT M,CONTRACT_INFO CI,"
                       + "       OPERATIONRULE O,FEERULE FR"
                       + " WHERE C.MERCHANTID = M.MERCHANTID"
                       + "   AND C.CONTRACTID = CI.CONTRACTID(+) "
                       + "   AND C.OPERATERULE = O.ID AND C.FEERULE_RENT = FR.ID"
                       + "   AND C.HTLX=1 "  //AND C.STATUS !=5
                       + "   AND C.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += @"  and exists(select 1 from CONTRACT_SHOP CP, SHOP S where C.CONTRACTID = CP.CONTRACTID 
                                    and CP.SHOPID = S.SHOPID AND S.FLOORID in (" + GetPermissionSql(PermissionType.Floor) + ")) ";  //楼层权限
            if (SqlyTQx != "") //业态权限
            {
                sql += @" and exists(select 1 from CONTRACT_SHOP CD,CATEGORY Y 
                                      where CD.CONTRACTID=C.CONTRACTID and CD.CATEGORYID=Y.CATEGORYID AND " + SqlyTQx + ") ";
            }
            item.HasKey("CATEGORYCODE", a => sql += $" and exists(select 1 from CONTRACT_SHOP CD,CATEGORY Y where CD.CONTRACTID = C.CONTRACTID and  CD.CATEGORYID = Y.CATEGORYID and Y.CATEGORYCODE LIKE '{a}%') ");
            item.HasKey("FLOORID", a => sql += $" and exists(select 1 from CONTRACT_SHOP CP,SHOP S where C.CONTRACTID = CP.CONTRACTID and CP.SHOPID = S.SHOPID AND S.FLOORID in ({a})) ");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID in ({a})");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and C.STATUS = '{a}'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDNAME", a => sql += $" and exists(select 1 from CONTRACT_BRAND CD,BRAND B where C.CONTRACTID=CD.CONTRACTID and CD.BRANDID=B.ID and B.NAME LIKE '%{a}%') ");

            sql += " ORDER BY CI.FLOORCODESTR,C.CONTRACTID ";

            return sql;
        }
        public DataGridResult ContractInfo(SearchItem item)
        {
            string sql = ContractInfoSql(item);

            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<合同状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public DataTable ContractInfoOutput(SearchItem item)
        {
            string sql = ContractInfoSql(item);

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<合同状态>("STATUS", "STATUSMC");
            return dt;
        }

        #endregion

        #region 收款方式销售报表
        public string PayTypeSalesql(SearchItem item)
        {
            string sql = "";
            if (item.Values["SrchTYPE"] == ((int)查询类型.日数据).ToString())
            {
                sql += @"select * from (";
                sql += @" SELECT MERCHANT.MERCHANTID,MERCHANT.NAME,BRAND.NAME BRANDNAME,to_char(HIS_SALE.SALE_TIME,'yyyy-mm-dd hh24:mi:ss') SALE_TIME,HIS_SALE.DEALID,HIS_SALE.POSNO,PAY.NAME PAYNAME,HIS_SALE_GOODS_PAY.AMOUNT " + PayTypeSalesqlParam(1, item);
                sql += @" union all ";
                sql += @" SELECT  MERCHANT.MERCHANTID,MERCHANT.NAME,BRAND.NAME BRANDNAME,to_char(SALE.SALE_TIME,'yyyy-mm-dd hh24:mi:ss') SALE_TIME,SALE.DEALID,SALE.POSNO,PAY.NAME PAYNAME,SALE_GOODS_PAY.AMOUNT " + PayTypeSalesqlParam(2, item);
                sql += @" ) ORDER BY MERCHANTID,SALE_TIME DESC, PAYNAME";

            }
            else
            {
                sql += @"select MERCHANTID, NAME, PAYNAME, POSNO, SUM(AMOUNT)AMOUNT from(";
                sql += @" SELECT MERCHANT.MERCHANTID,MERCHANT.NAME,PAY.NAME PAYNAME,HIS_SALE.POSNO,HIS_SALE_GOODS_PAY.AMOUNT,HIS_SALE.SALE_TIME " + PayTypeSalesqlParam(1, item);
                sql += @" union all ";
                sql += @" SELECT  MERCHANT.MERCHANTID,MERCHANT.NAME,PAY.NAME PAYNAME,SALE.POSNO,SALE_GOODS_PAY.AMOUNT,SALE.SALE_TIME " + PayTypeSalesqlParam(2, item);
                sql += @") GROUP BY MERCHANTID, POSNO, NAME, PAYNAME ORDER BY MERCHANTID, PAYNAME";

            }
            return sql;
        }
        public string PayTypeSalesqlParam(int TYPE, SearchItem item)
        {
            string sql = "";
            if (TYPE == 1)
            {
                sql += @" from HIS_SALE, HIS_SALE_GOODS_PAY, GOODS, PAY, CONTRACT, BRANCH, MERCHANT, BRAND
                                     where HIS_SALE.POSNO = HIS_SALE_GOODS_PAY.POSNO AND HIS_SALE.DEALID = HIS_SALE_GOODS_PAY.DEALID
                                       AND HIS_SALE_GOODS_PAY.GOODSID = GOODS.GOODSID AND HIS_SALE_GOODS_PAY.PAYID = PAY.PAYID
                                       AND CONTRACT.CONTRACTID = GOODS.CONTRACTID AND CONTRACT.BRANCHID = BRANCH.ID
                                       AND CONTRACT.MERCHANTID = MERCHANT.MERCHANTID AND GOODS.BRANDID = BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
                item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID  in ({a})");
                item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
                item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
                item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
                item.HasKey("Pay", a => sql += $" and PAY.PAYID in ({a})");
                item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
                item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
                item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");
            }
            else
            {
                sql += @" from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO AND SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID AND SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID AND CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID AND GOODS.BRANDID=BRAND.ID
                          and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
                item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID in ({a})");
                item.HasDateKey("RQ_START", a => sql += $" and TRUNC(SALE.SALE_TIME) >= {a}");
                item.HasDateKey("RQ_END", a => sql += $" and TRUNC(SALE.SALE_TIME) <= {a}");
                item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
                item.HasKey("Pay", a => sql += $" and PAY.PAYID in ({a})");
                item.HasKey("YEARMONTH_START", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
                item.HasKey("YEARMONTH_END", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
                item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");
            }
            return sql;
        }
        public DataGridResult PayTypeSale(SearchItem item)
        {
            var sqlunion = PayTypeSalesql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sqlunion, item.PageInfo, out count);

            if (count > 0)
            {
                var sqlsum = "";
                sqlsum = "SELECT NVL(SUM(AMOUNT),0) AMOUNT FROM ( " + sqlunion + ")";
                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                //历史交易金额汇总                
                decimal stsum = Convert.ToDecimal(dtSum.Rows[0]["AMOUNT"]);
                DataRow dr = dt.NewRow();
                dr["MERCHANTID"] = "合计";
                dr["AMOUNT"] = stsum.ToString();

                dt.Rows.Add(dr);
            }
            return new DataGridResult(dt, count);
        }
        public DataTable PayTypeSaleOutput(SearchItem item)
        {
            string sqlunion = PayTypeSalesql(item);
            DataTable dt = DbHelper.ExecuteTable(sqlunion);
            return dt;
        }
        #endregion

        #region  商户缴费
        //查询链接字符串（提取公共部分）
        private string MerchantPayCostSQLStr(SearchItem item)
        {
            string sqlparam = @" from MERCHANT M,BILL B,FEESUBJECT F,CONTRACT_BRAND C,BRAND D
                                where M.MERCHANTID = B.MERCHANTID AND F.TRIMID = B.TERMID AND C.CONTRACTID = B.CONTRACTID
                                  and M.MERCHANTID = B.MERCHANTID AND D.ID = C.BRANDID
                                  and B.STATUS IN (2,3,4)
                                  and B.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlparam += $" and B.BRANCHID in ({a})");
            item.HasKey("TRIMID", a => sqlparam += $" and F.TRIMID in ({a})");
            item.HasKey("MERCHANTNAME", a => sqlparam += $" and M.NAME LIKE '%{a}%'");
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
                    sqlparam += $" and B.STATUS in (2,3)";
                }
                else
                {
                    sqlparam += $" and B.STATUS in (4) ";
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
        public DataTable MerchantPayCostOutput(SearchItem item)
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
            return dt;
        }
        #endregion

        #region 商户租金经营状况
        //固定租金查询链接字符串（提取公共部分）
        private string MerchantBusinessStatusSqlParam(SearchItem item)
        {
            string sqlParam = "";
            item.HasKey("BRANCHID", a => sqlParam += $" and C.BRANCHID in ({a})");
            item.HasKey("MERCHANTNAME", a => sqlParam += $" and M.NAME LIKE '%{a}%'");
            item.HasKey("BRANDNAME", a => sqlParam += $" and exists(select 1 from CONTRACT_BRAND CB,BRAND B where C.CONTRACTID = CB.CONTRACTID and CB.BRANDID=B.ID and B.NAME LIKE '%{a}%')");
            item.HasKey("NIANYUE_START", a => sqlParam += $" and P.YEARMONTH >= {a}");
            item.HasKey("NIANYUE_END", a => sqlParam += $" and P.YEARMONTH <= {a}");
            string SqlyTQx = GetYtQx("Y");
            if (SqlyTQx != "") //业态权限
            {
                sqlParam += @" and exists(select 1 from CONTRACT_BRAND CD,BRAND D,CATEGORY Y 
                                      where CD.CONTRACTID=C.CONTRACTID and CD.BRANDID=D.ID and D.CATEGORYID=Y.CATEGORYID AND " + SqlyTQx + ") ";
            }
            item.HasKey("CATEGORYCODE", a => sqlParam += $@" and exists(select 1 from CONTRACT_BRAND CD,BRAND D,CATEGORY Y 
                                                                       where CD.CONTRACTID = C.CONTRACTID and CD.BRANDID = D.ID 
                                                                       and D.CATEGORYID = Y.CATEGORYID and Y.CATEGORYCODE LIKE '{a}%') ");

            item.HasKey("FLOORID", a => sqlParam += $@" and exists(select 1 from CONTRACT_SHOP CP,SHOP S 
                                                                   where C.CONTRACTID = CP.CONTRACTID and CP.SHOPID = S.SHOPID AND S.FLOORID in ({a})) ");

            string sqlstr = @"select M.MERCHANTID,M.NAME MERCHANTNAME,CI.BRANDNAMESTR BRANDNAME,P.YEARMONTH,C.AREAR AREA,
                                     (select nvl(sum(B.MUST_MONEY),0) from BILL B where B.CONTRACTID = C.CONTRACTID and B.NIANYUE = P.YEARMONTH 
                                                                           and B.TERMID = 1000 and B.STATUS IN (2,3,4)) JCZJ,
                                     (select nvl(sum(B.MUST_MONEY),0) from BILL B where B.CONTRACTID = C.CONTRACTID 
                                                                           and B.NIANYUE = P.YEARMONTH and B.TERMID = 1001 
                                                                           and B.STATUS IN (2,3,4)) JCZJTZ,
                                     (select nvl(sum(CT.TCZJ),0) from CONTRACT_TCZJ CT where CT.CONTRACTID = C.CONTRACTID 
                                                                      and CT.YEARMONTH = P.YEARMONTH) TCZJ,
                                     (select nvl(sum(CY.AMOUNT),0) from CONTRACT_SUMMARY_YM CY where CY.CONTRACTID = C.CONTRACTID 
                                                                        and CY.YEARMONTH = P.YEARMONTH) AMOUNT
                                from CONTRACT C,MERCHANT M,CONTRACT_INFO CI,PERIOD P   
                               where C.MERCHANTID = M.MERCHANTID 
                                     and C.CONTRACTID = CI.CONTRACTID
                                     and P.YEARMONTH in (select B.NIANYUE from BILL B where B.CONTRACTID = C.CONTRACTID and B.STATUS in (2,3,4))
                                     and C.HTLX = 1 ";

            if (item.Values["SrchTYPE"] == ((int)租金收取方式.固定租金).ToString())
                sqlstr += " and C.OPERATERULE in (select ID from OPERATIONRULE where PROCESSTYPE = 2)";
            else
                sqlstr += " and C.OPERATERULE not in (select ID from OPERATIONRULE where PROCESSTYPE = 2)";

            sqlstr += " and C.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";
            sqlstr += sqlParam;

            return sqlstr;
        }
        private string MerchantBusinessStatusSql(SearchItem item)
        {
            string sql = @"select Z.MERCHANTID,Z.MERCHANTNAME,Z.BRANDNAME,Z.YEARMONTH,SUM(Z.AREA) AREA,
                                  sum(Z.JCZJ) JCZJ,sum(Z.TCZJ) TCZJ,
                                  sum(Z.JCZJ + Z.JCZJTZ) SJZJ,sum(Z.AMOUNT) AMOUNT,
                                  decode(sum(Z.AREA),0,0,round(sum(Z.AMOUNT) / sum(Z.AREA),2)) BX,
                                  decode(sum(Z.AMOUNT),0,0,round(sum(Z.JCZJ + Z.JCZJTZ) / sum(Z.AMOUNT),2)) ZSB from (";
            sql += MerchantBusinessStatusSqlParam(item);
            sql += " ) Z  group by Z.MERCHANTID,Z.MERCHANTNAME,Z.BRANDNAME,Z.YEARMONTH order by Z.MERCHANTID,Z.YEARMONTH";
            return sql;
        }
        /// <summary>
        /// 商户租金经营状况查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult MerchantBusinessStatus(SearchItem item)
        {
            int count;
            string sql = MerchantBusinessStatusSql(item);
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            if (dt.Rows.Count > 0)
            {
                string sqlsum = @"select sum(Z.AREA) AREA,sum(Z.JCZJ) JCZJ,sum(Z.TCZJ) TCZJ,
                                         sum(Z.JCZJ + Z.JCZJTZ) SJZJ,sum(Z.AMOUNT) AMOUNT,
                                         decode(sum(Z.AREA),0,0,round(sum(Z.AMOUNT) / sum(Z.AREA),2)) BX,
                                         decode(sum(Z.AMOUNT),0,0,round(sum(Z.JCZJ + Z.JCZJTZ) / sum(Z.AMOUNT),2)) ZSB from (";
                sqlsum += MerchantBusinessStatusSqlParam(item) + ") Z";
                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                DataRow dr = dt.NewRow();
                dr["MERCHANTID"] = "合计";
                dr["AREA"] = dtSum.Rows[0]["AREA"].ToString();
                dr["JCZJ"] = dtSum.Rows[0]["JCZJ"].ToString();
                dr["TCZJ"] = dtSum.Rows[0]["TCZJ"].ToString();
                dr["SJZJ"] = dtSum.Rows[0]["SJZJ"].ToString();
                dr["AMOUNT"] = dtSum.Rows[0]["AMOUNT"].ToString();
                dr["BX"] = dtSum.Rows[0]["BX"].ToString();
                dr["ZSB"] = dtSum.Rows[0]["ZSB"].ToString();
                dt.Rows.Add(dr);
            }
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 商户租金经营状况导出
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataTable MerchantBusinessStatusOutput(SearchItem item)
        {
            string sql = MerchantBusinessStatusSql(item);
            return DbHelper.ExecuteTable(sql);
        }
        #endregion

        #region 商品销售明细查询
        public string GoodsSaleDetailSql(SearchItem item)
        {
            string sql = @" select *
                              from (select to_char(HIS_SALE.SALE_TIME,'yyyy-mm-dd hh24:mi:ss') SALE_TIME,
                                           HIS_SALE.POSNO,HIS_SALE.DEALID,BRAND.NAME BRANDNAME,
                                           GOODS.NAME GOODSNAME,PAY.NAME,HIS_SALE_GOODS_PAY.AMOUNT,
                                           NVL(HIS_SALE.POSNO_OLD,' ') POSNO_OLD,NVL(HIS_SALE.DEALID_OLD,0) DEALID_OLD
                                      from HIS_SALE,HIS_SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                                     where HIS_SALE.POSNO=HIS_SALE_GOODS_PAY.POSNO and HIS_SALE.DEALID=HIS_SALE_GOODS_PAY.DEALID 
                                       and HIS_SALE_GOODS_PAY.GOODSID=GOODS.GOODSID and HIS_SALE_GOODS_PAY.PAYID=PAY.PAYID 
                                       and CONTRACT.CONTRACTID=GOODS.CONTRACTID and CONTRACT.BRANCHID=BRANCH.ID 
                                       and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID and GOODS.BRANDID=BRAND.ID
                                       and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID in ({a})");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(HIS_SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(HIS_SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("GOODSDM", a => sql += $" and GOODS.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and GOODS.NAME LIKE '%{a}%'");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");

            sql += @" UNION ALL ";

            sql += @"select to_char(SALE.SALE_TIME,'yyyy-mm-dd hh24:mi:ss') SALE_TIME,
                            SALE.POSNO,SALE.DEALID,BRAND.NAME BRANDNAME,GOODS.NAME GOODSNAME,
                            PAY.NAME,SALE_GOODS_PAY.AMOUNT, NVL(SALE.POSNO_OLD,' ') POSNO_OLD,
                            NVL(SALE.DEALID_OLD,0) DEALID_OLD
                       from SALE,SALE_GOODS_PAY,GOODS,PAY,CONTRACT,BRANCH,MERCHANT,BRAND
                      where SALE.POSNO=SALE_GOODS_PAY.POSNO and SALE.DEALID=SALE_GOODS_PAY.DEALID 
                        and SALE_GOODS_PAY.GOODSID=GOODS.GOODSID and SALE_GOODS_PAY.PAYID=PAY.PAYID 
                        and CONTRACT.CONTRACTID=GOODS.CONTRACTID and CONTRACT.BRANCHID=BRANCH.ID 
                        and CONTRACT.MERCHANTID=MERCHANT.MERCHANTID and GOODS.BRANDID=BRAND.ID
                        and CONTRACT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and CONTRACT.BRANCHID in ({a})");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(SALE.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(SALE.SALE_TIME) <= {a}");
            item.HasKey("MERCHANTNAME", a => sql += $" and MERCHANT.NAME LIKE '%{a}%'");
            item.HasKey("YEARMONTH_START", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and to_char(SALE.SALE_TIME,'yyyyMM') <= {a}");
            item.HasKey("GOODSDM", a => sql += $" and GOODS.GOODSDM = '{a}'");
            item.HasKey("GOODSNAME", a => sql += $" and GOODS.NAME LIKE '%{a}%'");
            item.HasKey("BRANDNAME", a => sql += $" and BRAND.NAME LIKE '%{a}%'");

            sql += @" ) ORDER BY POSNO,SALE_TIME DESC,DEALID";
            return sql;
        }
        public DataGridResult GoodsSaleDetail(SearchItem item)
        {
            string sql = GoodsSaleDetailSql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);

            if (count > 0)
            {
                //历史交易金额汇总                
                string sqlunions = "select nvl(sum(AMOUNT),0) AMOUNT from ( select * from (" + sql + " ))";

                DataTable dtSum = DbHelper.ExecuteTable(sqlunions);
                decimal stsum = Convert.ToDecimal(dtSum.Rows[0]["AMOUNT"]);
                DataRow dr = dt.NewRow();
                dr["NAME"] = "合计";
                dr["AMOUNT"] = stsum.ToString();
                dt.Rows.Add(dr);
            }

            return new DataGridResult(dt, count);
        }
        public DataTable GoodsSaleDetailOutput(SearchItem item)
        {
            string sql = GoodsSaleDetailSql(item);
            return DbHelper.ExecuteTable(sql);
        }
        #endregion

        #region 商户应交已付报表
        public DataTable GetFeesubject(SearchItem item)
        {
            string sql = " select distinct b.termid,f.name" +
                         "   from bill b,merchant m,feesubject f" +
                         "  where b.MERCHANTID=m.MERCHANTID and b.TERMID=f.TRIMID" +
                         "        and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sql += $" and b.BRANCHID in ({a})");
            item.HasKey("MERCHANTNAME", a => sql += $" and m.NAME like '%{a}%'");
            item.HasKey("SFXMLX", a => sql += $" and f.TYPE in ({a})");
            item.HasKey("SFXM", a => sql += $" and b.TERMID in ({a})");
            item.HasKey("NIANYUE_START", a => sql += $" and b.NIANYUE >= {a}");
            item.HasKey("NIANYUE_END", a => sql += $" and b.NIANYUE <= {a}");
            item.HasKey("YEARMONTH_START", a => sql += $" and b.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and b.YEARMONTH <= {a}");

            return DbHelper.ExecuteTable(sql);
        }
        private string MerchantPayableSqlParam(SearchItem item)
        {
            string sql = "  from bill b,merchant m,feesubject f" +
                        "  where b.MERCHANTID=m.MERCHANTID and b.TERMID=f.TRIMID" +
                        "        and b.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and b.BRANCHID in ({a})");
            item.HasKey("MERCHANTNAME", a => sql += $" and m.NAME like '%{a}%'");
            item.HasKey("SFXMLX", a => sql += $" and f.TYPE in ({a})");
            item.HasKey("SFXM", a => sql += $" and b.TERMID in ({a})");
            item.HasKey("NIANYUE_START", a => sql += $" and b.NIANYUE >= {a}");
            item.HasKey("NIANYUE_END", a => sql += $" and b.NIANYUE <= {a}");
            item.HasKey("YEARMONTH_START", a => sql += $" and b.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sql += $" and b.YEARMONTH <= {a}");
            return sql;
        }
        private string MerchantPayableSql(SearchItem item)
        {
            string sql = " select b.MERCHANTID,m.NAME MERCHANTNAME,b.NIANYUE,b.YEARMONTH,";
            var resData = GetFeesubject(item);
            for (var i = 0; i < resData.Rows.Count; i++)
            {
                sql += String.Format(" sum(decode(b.termid, {0}, b.MUST_MONEY, 0)) MUST_MONEY{0}," +
                                     " sum(decode(b.termid, {0}, b.RECEIVE_MONEY, 0)) RECEIVE_MONEY{0},", resData.Rows[i][0].ToString());
            }
            sql += " sum(b.MUST_MONEY) MUST_MONEYSUM,sum(b.RECEIVE_MONEY) RECEIVE_MONEYSUM";
            sql += MerchantPayableSqlParam(item);
            sql += " group by b.MERCHANTID,m.NAME,b.NIANYUE,b.YEARMONTH,b.MUST_MONEY,b.RECEIVE_MONEY";
            return sql;
        }
        /// <summary>
        /// 商户应交已付报表查询
        /// </summary>
        public DataGridResult MerchantPayable(SearchItem item)
        {
            string sql = MerchantPayableSql(item);
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            if (count > 0)
            {
                string sqlsum = "select ";
                var resData = GetFeesubject(item);
                for (var i = 0; i < resData.Rows.Count; i++)
                {
                    sqlsum += String.Format(" nvl(sum(MUST_MONEY{0}),0) MUST_MONEY{0}," +
                                            " nvl(sum(RECEIVE_MONEY{0}),0) RECEIVE_MONEY{0},", resData.Rows[i][0].ToString());
                }
                sqlsum += " nvl(sum(MUST_MONEYSUM),0) MUST_MONEYSUM,nvl(sum(RECEIVE_MONEYSUM),0) RECEIVE_MONEYSUM";
                sqlsum += " from (" + sql + ")";

                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                if (dtSum.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["MERCHANTID"] = "合计";
                    for (var i = 0; i < resData.Rows.Count; i++)
                    {
                        var inx = resData.Rows[i][0].ToString();
                        dr["MUST_MONEY" + inx] = dtSum.Rows[0]["MUST_MONEY" + inx].ToString();
                        dr["RECEIVE_MONEY" + inx] = dtSum.Rows[0]["RECEIVE_MONEY" + inx].ToString();
                    }
                    dr["MUST_MONEYSUM"] = dtSum.Rows[0]["MUST_MONEYSUM"].ToString();
                    dr["RECEIVE_MONEYSUM"] = dtSum.Rows[0]["RECEIVE_MONEYSUM"].ToString();
                    dt.Rows.Add(dr);
                }
            }
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 商户应交已付报表导出
        /// </summary>
        public DataTable MerchantPayableOutput(SearchItem item)
        {
            string sql = MerchantPayableSql(item);
            return DbHelper.ExecuteTable(sql);
        }
        #endregion

        #region 销售采集处理记录查询
        public string SALEGATHERsql(SearchItem item)
        {
            string sqlsum = $@"SELECT BRANCH.NAME, SALEGATHER.DEALID, to_char(SALEGATHER.SALETIME,'yyyy-mm-dd hh24:mi:ss')SALETIME_START, to_char(SALEGATHER.SALETIME,'yyyy-mm-dd hh24:mi:ss')SALETIME_END,SALEGATHER.FLAG, to_char(SALEGATHER.CREATE_TIME,'yyyy-mm-dd hh24:mi:ss')CREATE_TIME_START, to_char(SALEGATHER.CREATE_TIME,'yyyy-mm-dd hh24:mi:ss')CREATE_TIME_END,SALEGATHER.REASON, STATION.STATIONBH
                    FROM SALEGATHER 
                    INNER JOIN STATION ON STATION.STATIONBH=SALEGATHER.POSNO 
                    INNER JOIN BRANCH ON BRANCH.ID=STATION.BRANCHID";
            sqlsum += "  AND STATION.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sqlsum += "   WHERE TYPE= 3";
            item.HasKey("BRANCHID", a => sqlsum += $" and STATION.BRANCHID in ({a})");
            item.HasDateKey("SALETIME_START", a => sqlsum += $" and TRUNC(SALETIME) >= {a}");
            item.HasDateKey("SALETIME_END", a => sqlsum += $" and TRUNC(SALETIME) <= {a}");
            item.HasKey("DEALID", a => sqlsum += $" and DEALID={a}");
            item.HasDateKey("CREATE_TIME_START", a => sqlsum += $" and TRUNC(CREATE_TIME) >={a}");
            item.HasDateKey("CREATE_TIME_END", a => sqlsum += $" and TRUNC(CREATE_TIME) <={a}");
            item.HasKey("FLAG", a => sqlsum += $" and FLAG IN ({a})");
            item.HasKey("REASON", a => sqlsum += $" and REASON LIKE '%{a}%'");

            item.HasKey("STATIONBH", a => sqlsum += $" and STATIONBH={a}");

            return sqlsum;
        }

        public DataGridResult SALEGATHER(SearchItem item)
        {
            string sql = SALEGATHERsql(item);

            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<处理标记>("FLAG", "FLAGMC");
            return new DataGridResult(dt, count);
        }

        #endregion

        #region 销售采集处理记录查询
        public DataTable SALEGATHEROutput(SearchItem item)
        {

            string sql = SALEGATHERsql(item);

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<处理标记>("FLAG", "FLAGMC");
            return dt;
        }

        #endregion

        #region 第三方支付记录查询
        private string PAYINFOsql(SearchItem item)
        {
            string sqlsum = $@"SELECT to_char(A.OPERTIME,'yyyy-mm-dd hh24:mi:ss') OPERTIME,
                                A.POSNO,A.DEALID,B.NAME,A.AMOUNT,A.SERIALNO,A.REFNO, D.NAME BRANCHNAME
                                FROM PAYRECORD A 
                                INNER JOIN PAY B ON(A.PAYID= B.PAYID)
                                INNER JOIN STATION C ON(A.POSNO =C.STATIONBH)
                                INNER JOIN BRANCH D ON(D.ID=C.BRANCHID)" +
                            " and c.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("BRANCHID", a => sqlsum += $" and C.BRANCHID in ({a})");
            item.HasDateKey("START", a => sqlsum += $" and A.OPERTIME>={a}");
            item.HasDateKey("END", a => sqlsum += $" and A.OPERTIME<={a}");
            item.HasKey("POSNO", a => sqlsum += $" and A.POSNO='{a}'");
            item.HasKey("DEALID", a => sqlsum += $" and A.DEALID='{a}'");
            item.HasKey("AMOUNT", a => sqlsum += $" and A.AMOUNT={a}");
            item.HasKey("PAYID", a => sqlsum += $" and B.PAYID  in ({a})");

            return sqlsum;
        }
        public DataGridResult PAYINFO(SearchItem item)
        {
            string sql = PAYINFOsql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);

        }

        public DataTable PAYINFOOutput(SearchItem item)
        {
            string sql = PAYINFOsql(item);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;

        }

        #endregion

        #region 费用账单查询
        private string Bill_Srcsql(SearchItem item)
        {
            string sqlsum = $@"SELECT A.BILLID, A.CONTRACTID, A.NIANYUE, A.YEARMONTH, A.MUST_MONEY, A.RECEIVE_MONEY, A.RETURN_MONEY, 
                                A.START_DATE,A.END_DATE, A.TYPE, A.STATUS,A.DESCRIPTION, B.NAME BRANCHNAME,C.NAME MERCHANTNAME,
                                F.NAME UNITNAME, D.NAME FEENAME
                                FROM BILL A 
                                LEFT JOIN BRANCH B ON A.BRANCHID=B.ID
                                LEFT JOIN MERCHANT C ON A.MERCHANTID=C.MERCHANTID
                                LEFT JOIN FEESUBJECT_ACCOUNT E ON A.TERMID=E.TERMID AND A.BRANCHID=E.BRANCHID
                                LEFT JOIN FEESUBJECT D ON E.TERMID=D.TRIMID
                                LEFT JOIN FEE_ACCOUNT F ON E.FEE_ACCOUNTID=F.ID AND F.BRANCHID=A.BRANCHID";

            sqlsum += "  WHERE A.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限     
            sqlsum += @"  and exists(select 1 from SHOP S, CONTRACT_SHOP CS where A.BRANCHID = S.BRANCHID AND S.SHOPID=CS.SHOPID AND S.FLOORID  in (" + GetPermissionSql(PermissionType.Floor) + ")) ";  //楼层权限
            string SqlyTQx = GetYtQx("Y");
            if (SqlyTQx != "") //业态权限
            {
                sqlsum += @" and exists(select 1 from CONTRACT_SHOP CS,CATEGORY Y, BILL A where CS.CATEGORYID = Y.CATEGORYID AND CS.CONTRACTID = A.CONTRACTID AND " + SqlyTQx + ") ";
            }

            item.HasKey("BRANCHID", a => sqlsum += $" and a.BRANCHID in ({a})");
            item.HasKey("MERCHANTNAME", a => sqlsum += $" and c.NAME LIKE '%{a}%'");
            item.HasKey("BILLID", a => sqlsum += $" and a.BILLID ={a}");
            item.HasKey("TRIMID", a => sqlsum += $" and d.TRIMID in ({a})");
            item.HasKey("YEARMONTH_START", a => sqlsum += $" and A.YEARMONTH >= {a}");
            item.HasKey("YEARMONTH_END", a => sqlsum += $" and A.YEARMONTH <= {a}");
            item.HasKey("NIANYUE_START", a => sqlsum += $" and A.NIANYUE >= {a}");
            item.HasKey("NIANYUE_END", a => sqlsum += $" and A.NIANYUE <= {a}");
            item.HasKey("TYPE", a => sqlsum += $" and A.TYPE in ({a})");
            item.HasKey("STATUS", a => sqlsum += $" and A.STATUS in ({a})");
            item.HasKey("CONTRACTID", a => sqlsum += $" and A.CONTRACTID = {a}");
            item.HasKey("CATEGORYCODE", a => sqlsum += $" and exists(select 1 from CONTRACT_SHOP CS,CATEGORY Y where A.CONTRACTID = CS.CONTRACTID AND CS.CATEGORYID = Y.CATEGORYID AND Y.CATEGORYCODE LIKE '{a}%') ");
            item.HasKey("FLOORID", a => sqlsum += $" and exists(select 1 from SHOP S, CONTRACT_SHOP CS where A.BRANCHID = S.BRANCHID AND S.SHOPID = CS.SHOPID AND S.FLOORID in （{a}))");
            return sqlsum;
        }
        public DataGridResult Bill_Src(SearchItem item)
        {
            string sql = Bill_Srcsql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<账单状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<账单类型>("TYPE", "TYPEMC");
            return new DataGridResult(dt, count);

        }

        public DataTable Bill_SrcOutput(SearchItem item)
        {

            string sql = Bill_Srcsql(item);

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<账单状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<账单类型>("TYPE", "TYPEMC");
            return dt;
        }
        #endregion

        #region 商户销售汇总
        public DataTable GetPayData(SearchItem item)
        {
            var id = "";
            item.HasKey("Pay", a => id = a);
            string sql = $@"SELECT * FROM PAY WHERE VOID_FLAG=1 ";
            if (!string.IsNullOrEmpty(id)) {
                sql += $" AND PAYID in ({id})";
            }
            sql+=" ORDER BY  PAYID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public string MerchantSaleCollectSQL(SearchItem item) {           
            var paydata = GetPayData(item);
            var sqlpay = "";           
            foreach (DataRow pay in paydata.Rows) {
                var payid = pay["PAYID"].ToString();
                sqlpay += $@"SUM(DECODE(A.PAYID,{payid},A.AMOUNT,0)) PAYID{payid},";
            }
            var sql = $@"SELECT {sqlpay} SUM(A.AMOUNT) SUMPAY,G.MERCHANTID,M.NAME MERCHANTNAME 
                        FROM ALLSALE_GOODS_PAY A,GOODS G,MERCHANT M ,CONTRACT C,BRANCH B,BRAND BD
                        WHERE A.GOODSID=G.GOODSID AND G.MERCHANTID=M.MERCHANTID AND G.CONTRACTID=C.CONTRACTID AND C.BRANCHID=B.ID AND G.BRANDID=BD.ID
                        AND C.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ") ";  //门店权限";
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID in ({a})");
            item.HasKey("Pay", a => sql += $" and A.PAYID in ({a})");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME = '{a}'");
            item.HasKey("BRANDNAME", a => sql += $" and BD.NAME = '{a}'");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID = '{a}'");
            item.HasDateKey("RQ_START", a => sql += $" and trunc(A.ACCOUNT_DATE) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and trunc(A.ACCOUNT_DATE) <= {a}");

            sql += " GROUP BY G.MERCHANTID,M.NAME ORDER BY G.MERCHANTID";
            return sql;
        }
        public DataGridResult MerchantSaleCollect(SearchItem item) {
            string sql = MerchantSaleCollectSQL(item);

            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            if (count > 0)
            {
                var paydata = GetPayData(item);
                var sqlpay = "";
                foreach (DataRow pay in paydata.Rows)
                {
                    var payid = pay["PAYID"].ToString();
                    sqlpay += $@"SUM(PAYID{payid}) SUMPAY{payid},";
                }
                var sqlsum = $"SELECT {sqlpay} SUM(SUMPAY) SSUMPAY FROM ({sql})";
                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                DataRow dr = dt.NewRow();
                dr["MERCHANTID"] = "合计";
                foreach (DataRow pay in paydata.Rows)
                {
                    var payid = pay["PAYID"].ToString();
                    dr["PAYID"+ payid] =dtSum.Rows[0]["SUMPAY"+ payid].ToString();
                }
                dr["SUMPAY"] = dtSum.Rows[0]["SSUMPAY"].ToString();
                dt.Rows.Add(dr);
            }
            return new DataGridResult(dt, count);
        }
        public DataTable MerchantSaleCollectOutput(SearchItem item)
        {
            string sql = MerchantSaleCollectSQL(item);

            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        #endregion

        #region 租约销售对比分析
        public string ContractSaleAnalysisSql(SearchItem item)
        {
            string table = "";
            string field = "";
            if (item.Values["SrchTYPE"] == ((int)查询类型.日数据).ToString())
            {
                field = " to_char(R.CP,'yyyy-MM-dd') CP,to_char(R.PP,'yyyy-MM-dd') PP,to_char(R.SP,'yyyy-MM-dd') SP,";
                table = "RF_DAILY_CONTRACT_SALE";
            }
            else
            {
                field = " R.CP,R.PP,R.SP,";
                table = "RF_MONTHLY_CONTRACT_SALE";
            }
            string sqlstr = $@"select{field}R.CP_SALE,round(decode(nvl(C.AREAR,0),0,0,R.CP_SALE/C.AREAR),2) BQPX,
                                   R.PP_SALE,round(decode(nvl(C.AREAR,0),0,0,R.PP_SALE/C.AREAR),2) SQPX,
                                   round(decode(nvl(R.PP_SALE,0),0,0,(R.CP_SALE/R.PP_SALE-1)*100),2) HB,
                                   round(decode(nvl(R.PP_SALE,0),0,0,(R.CP_SALE/R.PP_SALE-1)*100),2) PXHB,
                                   R.SP_SALE,round(decode(nvl(C.AREAR,0),0,0,R.SP_SALE/C.AREAR),2) SNTQPX,
                                   round(decode(nvl(R.SP_SALE,0),0,0,(R.CP_SALE/R.SP_SALE-1)*100),2) TB,
                                   round(decode(nvl(R.SP_SALE,0),0,0,(R.CP_SALE/R.SP_SALE-1)*100),2) PXTB,
                                   C.CONTRACTID,C.AREAR,M.NAME MERCHANTNAME,S.NAME SHOPNAME,F.CODE,F.NAME FLOORNAME,B.NAME BRANDNAME                                                           
                              from CONTRACT C,{table} R,MERCHANT M,SHOP S,BRAND B,FLOOR F
                             where R.CONTRACTID = C.CONTRACTID and R.SHOPID = S.SHOPID and 
                                   R.MERCHANTID=M.MERCHANTID and R.BRANDID = B.ID and S.FLOORID = F.ID";

            item.HasKey("BRANCHID", a => sqlstr += $" and C.BRANCHID in ({a})");
            item.HasKey("FLOORID", a => sqlstr += $" and F.ID in ({a})");
            item.HasKey("CONTRACTID", a => sqlstr += $" and C.CONTRACTID ={a}");
            item.HasKey("MERCHANTNAME", a => sqlstr += $" and M.NAME like '%{a}%'");
            item.HasKey("BRANDNAME", a => sqlstr += $" and B.NAME like '%{a}%'");

            if (item.Values["SrchTYPE"] == ((int)查询类型.日数据).ToString())
            {
                item.HasDateKey("CP", a => sqlstr += $" and TRUNC(R.CP)= {a}");
            }
            else
            {
                item.HasKey("CP", a => sqlstr += $" and R.CP ={a}");
            }
            sqlstr += " order by S.NAME ASC";
            return sqlstr;
        }
        public DataTable ContractSaleAnalysisDt(SearchItem item)
        {
            var sql = ContractSaleAnalysisSql(item);

            DataTable dt = new DataTable();

            var sqlSum = $@"select z.CODE,z.FLOORNAME,sum(z.CP_SALE) CP_SALE,sum(z.PP_SALE) PP_SALE,sum(z.SP_SALE) SP_SALE,
                                   round(decode(nvl(sum(z.PP_SALE),0),0,0,(sum(z.CP_SALE)/sum(z.PP_SALE)-1)*100),2) HB,
                                   round(decode(nvl(sum(z.SP_SALE),0),0,0,(sum(z.CP_SALE)/sum(z.SP_SALE)-1)*100),2) TB
                              from (" + sql + ") Z where 1=1 group by z.CODE,z.FLOORNAME";

            dt = DbHelper.ExecuteTable(sql);
            if (dt.Rows.Count == 0)
            {
                return dt;
            }
            DataTable dtSum = DbHelper.ExecuteTable(sqlSum);
            for (var i = 0; i < dtSum.Rows.Count; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["CODE"] = dtSum.Rows[i]["CODE"].ToString();
                newRow["FLOORNAME"] = dtSum.Rows[i]["FLOORNAME"].ToString() + "楼层小计";
                newRow["CP_SALE"] = dtSum.Rows[i]["CP_SALE"].ToString();
                newRow["PP_SALE"] = dtSum.Rows[i]["PP_SALE"].ToString();
                newRow["SP_SALE"] = dtSum.Rows[i]["SP_SALE"].ToString();
                newRow["HB"] = dtSum.Rows[i]["HB"].ToString();
                newRow["TB"] = dtSum.Rows[i]["TB"].ToString();
                dt.Rows.Add(newRow);
            }
            dt.DefaultView.Sort = "CODE ASC";
            dt = dt.DefaultView.ToTable();

            DataRow newRowAll = dt.NewRow();
            double cps = 0;
            double pps = 0;
            double sps = 0;
            for (var i = 0; i < dtSum.Rows.Count; i++)
            {
                cps += Convert.ToDouble(dtSum.Rows[i]["CP_SALE"].ToString());
                pps += Convert.ToDouble(dtSum.Rows[i]["PP_SALE"].ToString());
                sps += Convert.ToDouble(dtSum.Rows[i]["SP_SALE"].ToString());
            }
            newRowAll["FLOORNAME"] = "合计";
            newRowAll["CP_SALE"] = cps;
            newRowAll["PP_SALE"] = pps;
            newRowAll["SP_SALE"] = sps;
            newRowAll["HB"] = pps == 0 ? 0 : Math.Round((cps / pps - 1) * 100, 2);
            newRowAll["TB"] = sps == 0 ? 0 : Math.Round((cps / sps - 1) * 100, 2);
            dt.Rows.Add(newRowAll);

            return dt;
        }
        public DataGridResult ContractSaleAnalysis(SearchItem item)
        {
            var dt = ContractSaleAnalysisDt(item);
            return new DataGridResult(dt, dt.Rows.Count);
        }
        #endregion
    }
}
