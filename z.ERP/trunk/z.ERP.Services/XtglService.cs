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
using z.Extensiont;
using z.MVC5.Results;
using z.WebPage;

namespace z.ERP.Services
{
    public class XtglService : ServiceBase
    {
        internal XtglService()
        {
        }

        public DataGridResult GetBrandData(SearchItem item)
        {
            string sql = $@"SELECT B.*,C.CATEGORYCODE,C.CATEGORYNAME FROM BRAND B,CATEGORY C where B.CATEGORYID=C.CATEGORYID ";
            item.HasKey("NAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("CODE", a => sql += $" and B.CODE = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetPay(SearchItem item)
        {
            string sql = $@"SELECT PAYID,NAME FROM PAY WHERE 1=1 ";
            sql += " ORDER BY  PAYID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetPayElement(SearchItem item)
        {
            string sql = $@"SELECT * FROM PAY WHERE 1=1 ";
            item.HasKey("PAYID", a => sql += $" and PAYID = '{a}'");
            sql += " ORDER BY  PAYID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetFeeSubject(SearchItem item)
        {
            string sql = $@"SELECT TRIMID,NAME FROM FEESUBJECT WHERE 1=1 ";
            item.HasKey("TRIMID", a => sql += $" and TRIMID = '{a}'");
            sql += " ORDER BY  TRIMID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetFkfs(SearchItem item)
        {
            string sql = $@"select ID,NAME from FKFS order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetOperationrule(SearchItem item)
        {
            string sql = $@"select ID,NAME from OPERATIONRULE order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetBranch(SearchItem item)
        {
            string sql = $@"select ID,NAME from BRANCH order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFeeRule(SearchItem item)
        {
            string sql = $@"select ID,NAME from FEERULE order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }

}
