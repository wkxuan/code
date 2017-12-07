
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
    public class TestService : ServiceBase
    {
        internal TestService()
        {
        }

        public virtual string a()
        {

            P1Entity d1 = DbHelper.Select(new P1Entity("111"));


            return "";
            P1Entity p1 = new P1Entity()
            {
                F1 = "111",
                F2 = "2",
                F3 = "4",
                c1s = new List<C1Entity>() {
                             new C1Entity() {
                                  CF1 ="4",
                                  CF2="222",
                                  CF3 ="4",
                                   cc1s =new CC1Entity[] {
                                        new CC1Entity () {
                                               CCF1 ="4",
                                              CCF2="4",
                                              CCF3 ="5",
                                               CCF4 ="4"
                                        },
                                          new CC1Entity () {
                                               CCF1 ="4",
                                              CCF2="4",
                                              CCF3 ="4",
                                               CCF4 ="4"
                                        },
                                          new CC1Entity () {
                                               CCF1 ="4",
                                              CCF2="4",
                                              CCF3 ="6",
                                               CCF4 ="4"
                                        }
                                   }
                             },new C1Entity() {
                                   CF2="333",
                             }
                        }
            };
            DbHelper.Delete(p1);

            DbHelper.Save(p1);


            return "TestManager";
        }

        public DataGridResult GetData(SearchItem item)
        {
            string sql = $@"select * from ORG where 1=1 ";
            item.HasKey("ORGID", a => sql += $" and ORGID = '{a}' ");
            item.HasKey("ORGCODE", a => sql += $" and ORGCODE = '{a}' ");
            item.HasArrayKey("ORG_TYPE", a => sql += $" and ORG_TYPE in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            item.HasTimeKey("CREATE_TIME", a => sql += $" and CREATE_TIME = to_date('{a}') ");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
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
            string sql = $@"SELECT * FROM PAY";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }
}
