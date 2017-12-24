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
            item.HasKey("PAYID", a => sql += $" and PAYID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME = '{a}'");
            sql += " ORDER BY  PAYID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetPayElement(SearchItem item)
        {
            string sql = $@"SELECT * FROM PAY WHERE 1=1 ";
            item.HasKey("PAYID", a => sql += $" and PAYID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetFeeSubject(SearchItem item)
        {
            string sql = $@"SELECT TRIMID,NAME FROM FEESUBJECT  WHERE 1=1";
            item.HasKey("TRIMID", a => sql += $" and TRIMID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME = '{a}'");
            sql += " ORDER BY  TRIMID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetFeeSubjectElement(SearchItem item)
        {
            string sql = $@"SELECT TRIMID,NAME FROM FEESUBJECT WHERE 1=1 ";
            item.HasKey("TRIMID", a => sql += $" and TRIMID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetBranch(SearchItem item)
        {
            string sql = $@"SELECT ID,NAME FROM BRANCH WHERE 1=1";
            item.HasKey("ID,", a => sql += $" and ID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME = '{a}'");
            sql += " ORDER BY  ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetBranchElement(SearchItem item)
        {
            string sql = $@"select * from BRANCH where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetConfig(SearchItem item)
        {
            string sql = $@"select * from CONFIG where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetFkfs(SearchItem item)
        {
            string sql = $@"select ID,NAME from FKFS where 1=1";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetOperationrule(SearchItem item)
        {
            string sql = $@"select * from OPERATIONRULE where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetFeeRule(SearchItem item)
        {
            string sql = $@"select * from FEERULE where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetLateFeeRule(SearchItem item)
        {
            string sql = $@"select A.ID,A.NAME,A.DAYS,A.AMOUNTS from LATEFEERULE A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.ID = '{a}'");
            sql += "order by A.ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetLateFeeRuleElement(SearchItem item)
        {
            string sql = $@"select A.ID,A.NAME,A.DAYS,A.AMOUNTS from LATEFEERULE A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.ID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFloor(SearchItem item)
        {
            string sql = $@"SELECT A.CODE,A.NAME FROM FLOOR A WHERE 1=1";
            item.HasKey("ID,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            sql += " ORDER BY  A.CODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetFloorElement(SearchItem item)
        {
            string sql = $@"select A.* from FLOOR A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.CODE = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetShop(SearchItem item)
        {
            string sql = $@"SELECT A.CODE,A.NAME FROM SHOP A WHERE 1=1";
            item.HasKey("CODE,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            sql += " ORDER BY  A.CODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetShopElement(SearchItem item)
        {
            string sql = $@"select A.* from SHOP A where 1=1 ";
            item.HasKey("SHOPID", a => sql += $" and A.SHOPID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetEnergyFiles(SearchItem item)
        {
            string sql = $@"SELECT A.FILECODE,A.FILENAME FROM ENERGY_FILES A WHERE 1=1";
            item.HasKey("FILECODE,", a => sql += $" and A.FILECODE = '{a}'");
            item.HasKey("FILENAME", a => sql += $" and A.FILENAME = '{a}'");
            sql += " ORDER BY  A.FILECODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetEnergyFilesElement(SearchItem item)
        {
            string sql = $@"select A.* from ENERGY_FILES A where 1=1 ";
            item.HasKey("FILEID", a => sql += $" and A.FILEID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }

}
