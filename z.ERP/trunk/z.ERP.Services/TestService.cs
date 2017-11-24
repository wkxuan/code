
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
using z.MVC5.Results;
using z.WebPage;

namespace z.ERP.Services
{
    public class TestService : ServiceBase
    {
        internal TestService()
        {
        }

        public virtual string a()
        {
            return "TestManager";
        }

        public DataGridResult GetData()
        {
            SearchItem item = SearchItem.GetAllPram();
            string sql = $@"select * from bm where 1=1 ";
            item.HasKey("DEPTID", a => sql += $" and DEPTID = '{a}' ");
            item.HasKey("DEPT_NAME", a => sql += $" and DEPT_NAME = '{a}' ");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }
}
