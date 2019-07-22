
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using z.ERP.Entities;
using z.Extensions;
using z.MVC5.Results;
using z.Results;
using z.ERP.Entities.Enum;
using z.ERP.Model.Vue;
using z.ERP.Entities.Auto;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class DataService : ServiceBase
    {
        internal DataService()
        {
        }

        public List<SelectItem> a()
        {
            return new List<SelectItem>() {
                 new SelectItem ("1","11")
            };
        }

        public List<SelectItem> b()
        {
            return new List<SelectItem>() {
                 new SelectItem ("1","11"),
                 new SelectItem ("2","22"),
                 new SelectItem ("3","33")
            };
        }
        //测试控件SQL语句查询下拉收款方式
        public List<SelectItem> pay()
        {
            string sql = $@"SELECT * FROM PAY WHERE VOID_FLAG=1 ORDER BY  PAYID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("PAYID", "NAME");
        }

        public List<SelectItem> fkfs()
        {
            string sql = $@"SELECT ID,NAME FROM FKFS ORDER BY  ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }

        public List<SelectItem> branch()
        {
            string sql = $@"SELECT A.ID,A.NAME FROM BRANCH A WHERE 1=1"
                           + " and A.ID in ("+GetPermissionSql(PermissionType.Branch)+")"  //门店权限
                         + " ORDER BY  A.ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }
        public List<SelectItem> floor()
        {
            string sql = $@"SELECT A.ID,A.NAME FROM FLOOR A  WHERE 1=1 "
                       + "     and A.BRANCHID IN ("+GetPermissionSql(PermissionType.Branch)+ ")"  //门店权限
                       + "   ORDER BY  A.ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }

        public List<SelectItem> org_hs()
        {
            string sql = $@"SELECT A.ORGID,A.ORGNAME FROM ORG A WHERE  ORG_TYPE=" + ((int)部门类型.核算部门).ToString() + "   ORDER BY  A.ORGID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ORGID", "ORGNAME");
        }

        public List<SelectItem> org_zs()
        {
            string sql = $@"SELECT A.ORGID,A.ORGNAME FROM ORG A WHERE LEVEL_LAST="+ ((int)末级标记.末级).ToString() + " AND  ORG_TYPE=" + ((int)部门类型.招商部门).ToString() + "   ORDER BY  A.ORGID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ORGID", "ORGNAME");
        }

        public List<SelectItem> operrule()
        {
            string sql = $@"SELECT A.ID,A.NAME FROM OPERATIONRULE A WHERE 1=1   ORDER BY  A.ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }

        public List<SelectItem> feesubject()
        {
            string sql = $@"SELECT TRIMID,NAME FROM FeeSubject A  ORDER BY TRIMID";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("TRIMID", "NAME");
        }

        public List<SelectItem> coupon()    //优惠券
        {
            string sql = "select YHQID,YHQMC from YHQDEF where 1=1 order by YHQID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("YHQID", "YHQMC");
        }

        public List<SelectItem> feeRule()
        {
            string sql = $@"SELECT ID,NAME FROM FEERULE   ORDER BY  ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }

        public List<SelectItem> LateFeeRule()
        {
            string sql = $@"SELECT ID,NAME FROM LateFeeRule  ORDER BY  ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }

        public DataGridResult GetJsklGroup(SearchItem item)
        {
            string sql = $@"select * from CONTRACT_GROUP A where 1=1";
            item.HasKey("GROUPNO", a => sql += $" and A.GROUPNO = '{a}'");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasKey("DESCRIPTION", a => sql += $" and A.DESCRIPTION like '%{a}%'");
            sql += " ORDER BY  A.GROUPNO";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }



        public object GetBrand(BRANDEntity Data)
        {
            string sql = " SELECT  A.NAME,B.CATEGORYCODE,B.CATEGORYNAME FROM BRAND A,CATEGORY B " +
                "  WHERE  A.CATEGORYID = B.CATEGORYID ";
            if (!Data.ID.IsEmpty())
                sql += (" and A.ID= " + Data.ID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt.ToOneLine()
            };
        }

        public object GetShop(SHOPEntity Data)
        {
            string sql = " SELECT  A.SHOPID,A.CODE,B.CATEGORYID,B.CATEGORYCODE,B.CATEGORYNAME,A.AREA_BUILD,A.AREA_RENTABLE " +
                          "  FROM SHOP A,CATEGORY B " +
                          " WHERE  A.CATEGORYID = B.CATEGORYID " +
                          "   AND  A.BRANCHID IN ("+GetPermissionSql(PermissionType.Branch)+")";  //门店权限
            if (!Data.CODE.IsEmpty())
                sql += " AND A.CODE='"+ Data.CODE + "'";
            if (!Data.BRANCHID.IsEmpty())
                sql += " AND A.BRANCHID=" + Data.BRANCHID + "";
            
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt.ToOneLine()
            };
        }
        public object GetFeeSubject(FEESUBJECTEntity Data)
        {
            string sql = " SELECT  * FROM " +
                "  FEESUBJECT A" +
                "  WHERE  1 = 1 ";
            if (!Data.TRIMID.IsEmpty())
                sql += " AND A.TRIMID='" + Data.TRIMID + "'";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt.ToOneLine()
            };
        }

        //和下面重名
        public object GetBill_Bak(BILLEntity Data)
        {
            string sql = " SELECT  A.BILLID,A.BRANCHID,A.MERCHANTID,A.CONTRACTID,A.TERMID,A.NIANYUE,A.YEARMONTH,A.MUST_MONEY, "
                       + "         A.RECEIVE_MONEY,A.RRETURN_MONEY,A.START_DATE, A.END_DATE,A.TYPE,A.STATUS,A.DESCRIPTION,B.NAME FEENAME"
                       + "   FROM BILL A,FeeSubject B "
                       + "  WHERE  A.TRIMID= B.TRIMID "
                       + "    AND A.BRANCHID IN ("+GetPermissionSql(PermissionType.Branch)+") ";  //门店权限

            if (!Data.BILLID.IsEmpty())
                sql += " AND A.BILLID='" + Data.BILLID + "'";
            if (!Data.BRANCHID.IsEmpty())
                sql += " AND A.BRANCHID=" + Data.BRANCHID + "";

            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt.ToOneLine()
            };
        }
        /// <summary>
        /// 弹窗选择账单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetBill(SearchItem item)
        {
            string sql = " SELECT  A.BILLID,A.BRANCHID,A.MERCHANTID,A.CONTRACTID,A.TERMID,A.NIANYUE,A.YEARMONTH,A.MUST_MONEY "
                + " ,A.RECEIVE_MONEY,A.RRETURN_MONEY,A.START_DATE, A.END_DATE,A.TYPE,A.STATUS,A.DESCRIPTION,B.NAME BRANCHNAME "
                + " ,A.REPORTER_NAME,A.REPORTER_TIME,F.NAME TERMMC,A.MUST_MONEY - A.RECEIVE_MONEY UNPAID_MONEY"
                + "   FROM BILL A,BRANCH B,FEESUBJECT F "
                + "  WHERE A.BRANCHID=B.ID  and A.TERMID =F.TRIMID"
                + "    and B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = {a}");
            item.HasKey("BILLID", a => sql += $" and A.BILLID = {a}");
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID = {a}");
            item.HasKey("TRIMID", a => sql += $" and A.TERMID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = {a}");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = {a}");
            item.HasKey("TYPE", a => sql += $" and A.TYPE = {a}");
            item.HasKey("NIANYUE", a => sql += $" and A.NIANYUE = {a}");
            item.HasKey("YEARMONTH", a => sql += $" and A.YEARMONTH = {a}");
            item.HasKey("REPORTER", a => sql += $" and REPORTER_NAME  LIKE '%{a}%'");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and A.REPORTER_TIME >= {a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and A.REPORTER_TIME <= {a}");
            item.HasKey("WFDJ", a => sql += $" and A.MUST_MONEY - A.RECEIVE_MONEY<>0");
            item.HasKey("FTYPE",a => sql += $" and F.TYPE = {a}");    //费用项目类型
            item.HasKey("RRETURNFLAG", a => sql += $" and A.RECEIVE_MONEY <> 0");
            item.HasKey("SCFS_TZD", a => sql += $" and F.SCFS_TZD = {a}");
            item.HasKey("FEE_ACCOUNTID", a => sql += $" and F.FEE_ACCOUNTID = {a}");


            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<账单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        /// <summary>
        /// 弹窗选择账单 status 是2和3
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetBillPart(SearchItem item)
        {
            string sql = " SELECT  A.BILLID,A.BRANCHID,A.MERCHANTID,A.CONTRACTID,A.TERMID,A.NIANYUE,A.YEARMONTH,A.MUST_MONEY "
                + " ,A.RECEIVE_MONEY,A.RRETURN_MONEY,A.START_DATE, A.END_DATE,A.TYPE,A.STATUS,A.DESCRIPTION,B.NAME BRANCHNAME "
                + " ,A.REPORTER_NAME,A.REPORTER_TIME,F.NAME TERMMC,A.MUST_MONEY - A.RECEIVE_MONEY UNPAID_MONEY"
                + "   FROM BILL A,BRANCH B,FEESUBJECT F "
                + "  WHERE  A.BRANCHID=B.ID  and A.TERMID =F.TRIMID and A.STATUS IN (2,3) and A.TYPE IN (1,2) "
                + "    AND B.ID IN ("+GetPermissionSql(PermissionType.Branch)+") ";   //门店权限
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = {a}");
            item.HasKey("BILLID", a => sql += $" and A.BILLID = {a}");
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID = {a}");
            item.HasKey("TRIMID", a => sql += $" and A.TERMID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = {a}");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = {a}");
            item.HasKey("TYPE", a => sql += $" and A.TYPE = {a}");
            item.HasKey("NIANYUE", a => sql += $" and A.NIANYUE = {a}");
            item.HasKey("YEARMONTH", a => sql += $" and A.YEARMONTH = {a}");
            item.HasKey("REPORTER", a => sql += $" and REPORTER_NAME  LIKE '%{a}%'");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and A.REPORTER_TIME >= {a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and A.REPORTER_TIME <= {a}");
            item.HasKey("WFDJ", a => sql += $" and A.MUST_MONEY - A.RECEIVE_MONEY<>0");
            item.HasKey("FTYPE", a => sql += $" and F.TYPE = {a}");    //费用项目类型
            item.HasKey("RRETURNFLAG", a => sql += $" and A.RECEIVE_MONEY <> 0");
            item.HasKey("SCFS_TZD", a => sql += $" and F.SCFS_TZD = {a}");
            item.HasKey("FEE_ACCOUNTID", a => sql += $" and F.FEE_ACCOUNTID = {a}");


            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<账单状态Part>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        /// <summary>
        /// 树形部门
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public Tuple<dynamic> GetTreeOrg()
        {
            string sql = "select * from ORG where 1=1 and ORGID in (" + GetPermissionSql(PermissionType.Org) + ")"; //部门权限

            List<ORGEntity> p = DbHelper.ExecuteObject<ORGEntity>(sql);

          //  List<ORGEntity> p = DbHelper.SelectList(new ORGEntity()).OrderBy(a => a.ORGCODE).ToList();
            var treeOrg = new UIResult(TreeModel.Create(p,
                a => a.ORGCODE,
                a => new TreeModel()
                {
                    code = a.ORGCODE,
                    title = a.ORGNAME,
                    value = a.ORGID,
                    label = a.ORGNAME,
                    expand = true
                })?.ToArray());

            return new Tuple<dynamic>(treeOrg);
        }

        public object GetBranch(BRANCHEntity Data)
        {
            string sql = $@"SELECT A.ID,A.NAME FROM BRANCH A WHERE 1=1"
                  + " AND A.ID IN ("+GetPermissionSql(PermissionType.Branch)+")";  //门店权限
            if (!Data.ID.IsEmpty())
                sql += (" and A.ID= " + Data.ID);
            sql += " and STATUS = 1";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt
            };
        }

        public object GetRegion(REGIONEntity Data)
        {
            string sql = $@"SELECT A.REGIONID,A.CODE,A.NAME FROM REGION A WHERE 1=1"
                  + " AND A.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")"  //门店权限
                  + " AND A.REGIONID IN (" + GetPermissionSql(PermissionType.Region) + ")";  //区域权限
            if (!Data.BRANCHID.IsEmpty())
                sql += (" and A.BRANCHID= " + Data.BRANCHID);
            sql += " AND A.STATUS = 1 ORDER BY A.CODE";
            
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt
            };
        }
        public object GetFloor(FLOOREntity Data)
        {
            string sql = $@"SELECT A.ID,A.CODE,A.NAME FROM FLOOR A WHERE 1=1"
                 + " AND A.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")"  //门店权限
                 + " AND A.REGIONID IN (" + GetPermissionSql(PermissionType.Region) + ")"; //区域权限
            if (!Data.BRANCHID.IsEmpty())
                sql += (" and A.BRANCHID= " + Data.BRANCHID);
            if (!Data.REGIONID.IsEmpty())
                sql += (" and A.REGIONID= " + Data.REGIONID);
            sql += " and STATUS = 1 ORDER BY A.CODE";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt
            };
        }
        /// <summary>
        /// 树形业态
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public Tuple<dynamic> GetTreeCategory()
        {
            List<CATEGORYEntity> p = DbHelper.SelectList(new CATEGORYEntity()).OrderBy(a => a.CATEGORYCODE).ToList();
            var treeOrg = new UIResult(TreeModel.Create(p,
                a => a.CATEGORYCODE,
                a => new TreeModel()
                {
                    code = a.CATEGORYCODE,
                    title = a.CATEGORYNAME,
                    value = a.CATEGORYID,
                    label = a.CATEGORYNAME,
                    expand = true
                })?.ToArray());

            return new Tuple<dynamic>(treeOrg);
        }
        /// <summary>
        /// 弹窗选择账单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetContract(SearchItem item)
        {
            string sql = " SELECT  A.*  ,B.NAME MERCHANTNAME,C.NAME BRANCHNAME "
                       + "   FROM CONTRACT A,MERCHANT B,BRANCH C "
                       + "  WHERE A.MERCHANTID=B.MERCHANTID AND A.BRANCHID=C.ID"
                       + "    AND C.ID IN ("+GetPermissionSql(PermissionType.Branch)+")";  //门店权限
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID like '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = '{a}'");
            item.HasKey("REPORTER", a => sql += $" and A.REPORTER = '{a}'");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and A.REPORTER_TIME >= '{a}'");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and A.REPORTER_TIME <= '{a}'");
            item.HasKey("YXHTBJ", a => sql += $" and A.STATUS in (2,3,4)");
            item.HasKey("FREESHOPBJ", a => sql += $" and not exists (select 1 from FREESHOP P where P.CONTRACTID = A.CONTRACTID)");

            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<合同状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetGoodsShopList(SearchItem item)
        {
            string sql = $@" select G.*,M.NAME SHMC,D.NAME BRANDMC,C.CODE KINDDM,C.NAME KINDMC,S.CODE,S.NAME SPMC,P.SHOPID "
              + "   from GOODS G,MERCHANT M,GOODS_KIND C,BRAND D ,GOODS_SHOP P,SHOP S,CONTRACT T"
              + "  where G.MERCHANTID=M.MERCHANTID  AND G.KINDID=C.ID and G.BRANDID =D.ID "
              + "    and G.GOODSID = P.GOODSID  and P.SHOPID=S.SHOPID AND G.CONTRACTID=T.CONTRACTID"
              + "    and T.BRANCHID IN ("+GetPermissionSql(PermissionType.Branch)+")";  //门店权限
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and G.STATUS IN ({a})");
            item.HasKey("NAME", a => sql += $" and G.NAME like '%{a}%'");
            item.HasKey("YYY", a => sql += $" and exists(select 1 from SYSUSER S where P.SHOPID = S.SHOPID and S.USERID = '{a}')");
            
            sql += " ORDER BY  G.GOODSDM";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetGoodsList(SearchItem item)
        {
            string sql = @"SELECT G.*,C.JSKL,B.NAME BRANDMC "
                      + "    FROM GOODS G, CONTRACT_GROUP C ,BRAND B,CONTRACT T "
                      + "   WHERE G.STATUS=2 AND G.CONTRACTID=C.CONTRACTID AND G.JSKL_GROUP=C.GROUPNO "
                      + "     AND G.BRANDID=B.ID AND G.CONTRACTID=T.CONTRACTID "
                      + "     AND T.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("NAME", a => sql += $" and G.NAME like '%{a}%'");
            sql += " ORDER BY  G.GOODSDM";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataTable GetPay()
        {
            string sql = $@"SELECT * FROM PAY WHERE VOID_FLAG=1 ORDER BY  PAYID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }

        public List<SelectItem> comPlainDept()
        {
            string sql = $@"select ID,NAME from COMPLAINDEPT ORDER BY ID";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }
        public List<SelectItem> comPlainType()
        {
            string sql = $@"select ID,NAME from COMPLAINTYPE ORDER BY ID";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }

        public List<SelectItem> feeAccount()    //收费单位
        {
            string sql = "select id,name from fee_account order by id ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("id", "name");
        }
        //商户收费单位余额
        public object GetBalance(MERCHANT_ACCOUNTEntity Data)
        {
            string sql = @" SELECT * FROM MERCHANT_ACCOUNT WHERE 1=1";
            if (!Data.MERCHANTID.IsEmpty())
                sql += " AND MERCHANTID='" + Data.MERCHANTID + "'";
            if (!Data.FEE_ACCOUNT_ID.IsEmpty())
                sql += " AND FEE_ACCOUNT_ID='" + Data.FEE_ACCOUNT_ID + "'";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt.ToOneLine()
            };
        }
        //获取打印地址
        public DataTable GetPrintUrl(string menuid,string printtype)
        {
            string sql = @" SELECT * FROM PRINTDEF WHERE  USED=1 ";
            if (!menuid.IsEmpty())
                sql += " AND MENUID='" + menuid + "'";
            if (!printtype.IsEmpty())
                sql += " AND PRINTTYPE='" + printtype + "'";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }

    }
}
