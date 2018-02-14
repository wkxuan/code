
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
    }
}
