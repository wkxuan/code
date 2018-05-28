
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using z.ERP.Entities;
using z.Extensions;
using z.MVC5.Results;
using z.Results;
using z.WebPage;
using z.ERP.Entities.Enum;
using z.ERP.Model.Vue;

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
        public List<SelectItem> branch()
        {
            string sql = $@"SELECT A.ID,A.NAME FROM BRANCH A WHERE 1=1 ORDER BY  A.ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
        }
        public List<SelectItem> floor()
        {
            string sql = $@"SELECT A.ID,A.NAME FROM FLOOR A WHERE 1=1 ORDER BY  A.ID ";
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
        public List<SelectItem> feeRule()
        {
            string sql = $@"SELECT ID,NAME FROM FEERULE   ORDER BY  ID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.ToSelectItem("ID", "NAME");
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
            string sql = " SELECT  A.SHOPID,A.CODE,B.CATEGORYID,B.CATEGORYCODE,B.CATEGORYNAME,A.AREA_BUILD,A.AREA_RENTABLE FROM " +
                "  SHOP A,CATEGORY B " +
                "  WHERE  A.CATEGORYID = B.CATEGORYID ";
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

        public object GetBill(BILLEntity Data)
        {
            string sql = " SELECT  A.BILLID,A.BRANCHID,A.MERCHANTID,A.CONTRACTID,A.TERMID,A.NIANYUE,A.YEARMONTH,A.MUST_MONEY "
                +" ,A.RECEIVE_MONEY,A.RRETURN_MONEY,A.START_DATE, A.END_DATE,A.TYPE,A.STATUS,A.DESCRIPTION "
                       +" FROM BILL A " +
                "  WHERE  1=1 ";
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
                + " ,A.RECEIVE_MONEY,A.RRETURN_MONEY,A.START_DATE, A.END_DATE,A.TYPE,A.STATUS,A.DESCRIPTION "
                       + " FROM BILL A " +
                "  WHERE  1=1 ";
            item.HasKey("BILLID", a => sql += $" and A.BILLID like '%{a}%'");
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID like '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = '{a}'");
            item.HasKey("TYPE", a => sql += $" and A.TYPE = '{a}'");
            item.HasKey("NIANYUE", a => sql += $" and A.NIANYUE = '{a}'");
            item.HasKey("YEARMONTH", a => sql += $" and A.YEARMONTH = '{a}'");
            item.HasKey("REPORTER", a => sql += $" and A.REPORTER = '{a}'");
            item.HasKey("REPORTER_TIME_START", a => sql += $" and A.REPORTER_TIME >= '{a}'");
            item.HasKey("REPORTER_TIME_END", a => sql += $" and A.REPORTER_TIME <= '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        /// <summary>
        /// 树形部门
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public Tuple<dynamic> GetTreeOrg()
        {
            List<ORGEntity> p = DbHelper.SelectList(new ORGEntity()).OrderBy(a => a.ORGCODE).ToList();
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
            string sql = $@"SELECT A.ID,A.NAME FROM BRANCH A WHERE 1=1";
            if (!Data.ID.IsEmpty())
                sql += (" and A.ID= " + Data.ID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt
            };
        }
        public object GetFloor(FLOOREntity Data)
        {
            string sql = $@"SELECT A.ID,A.CODE,A.NAME FROM FLOOR A WHERE 1=1";
            if (!Data.BRANCHID.IsEmpty())
                sql += (" and A.BRANCHID= " + Data.BRANCHID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt
            };
        }
    }
}
