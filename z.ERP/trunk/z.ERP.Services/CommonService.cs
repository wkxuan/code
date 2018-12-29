
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
using z.DBHelper.DBDomain;
using z.DBHelper.DBDomain;
using z.Exceptions;
using z.Extensions;

namespace z.ERP.Services
{
    public class CommonService : ServiceBase
    {
        internal CommonService()
        {
        }


        public void a()
        {
            /*
             * {{aa}}是参数,这个参数必输,参数名是aa,所以后面添加参数时参数名写aa
             * {{bb}}外面包了层{{@ @}}  说明这个参数是可选的,当bb参数没有传的时候,这段就不拼
             * {{cc}}是数组参数,用in() 到时候传一个数组进来,这个也是可选参数,当数组为空或为null时这段sql不拼
             * 
             * 
             * 
             */

            zParameter[] param = new zParameter[2] {
                           new zParameter("pDATE1",new DateTime (2017,10,1),DbType.Date),
                           new zParameter("pDATE2",new DateTime (2018,10,1),DbType.Date) };
            string sql = "select sum(1) SFJE from WORKITEM where PROC_TIME>={{pDATE1}} and PROC_TIME<={{pDATE2}}";
            DataTable dt = DbHelper.ExecuteTable(sql, param);

            sql = "select 1 from dual where 1=1 and a={{aa}} {{@ and b={{bb}} @}} {{@ and c in ({{cc}}) @}}";
            zParameter[] parameters = new zParameter[] {
                new zParameter ("aa",1),
                new zParameter ("bb",1),
                new zParameter ("cc",new string[] { "cc1","cc2" })
            };
            DbHelper.ExecuteTable(sql, parameters);
        }

        /// <summary>
        /// 通用的单表存储方式
        /// </summary>
        /// <param name="infos"></param>
        public IEnumerable<string> CommonSave(IEnumerable<TableEntityBase> infos)
        {
            List<string> res = new List<string>();
            using (var tran = DbHelper.BeginTransaction())
            {
                infos.ForEach(a => res.Add(CommonSave(a)));
                tran.Commit();
            }
            return res;
        }

        /// <summary>
        /// 通用的单表存储方式
        /// </summary>
        /// <param name="info"></param>
        public string CommonSave(TableEntityBase info)
        {
            string key = "";
            PropertyInfo[] pis = info.GetPrimaryKey();
            if (pis == null || pis.Length != 1)
            {
                throw new LogicException("只有唯一主键的类适用于通用存储方法");
            }
            PropertyInfo pi = pis.FirstOrDefault();
            key = pi.GetValue(info, null)?.ToString();
            if (string.IsNullOrWhiteSpace(key))
            {
                key = NewINC(info);
                pi.SetValue(info, key, null);
            }
            DbHelper.Save(info);
            return key;
        }


        /// <summary>
        /// 通用的单表存储方式
        /// </summary>
        /// <param name="infos"></param>
        public void CommenDelete(IEnumerable<TableEntityBase> infos)
        {
            using (var tran = DbHelper.BeginTransaction())
            {
                infos.ForEach(CommenDelete);
                tran.Commit();
            }
        }

        /// <summary>
        /// 通用单表删除方法
        /// </summary>
        /// <param name="info"></param>
        public void CommenDelete(TableEntityBase info)
        {
            PropertyInfo[] pis = info.GetPrimaryKey();
            if (pis == null || pis.Length != 1)
            {
                throw new LogicException("只有唯一主键的类适用于通用删除方法");
            }
            PropertyInfo pi = pis.FirstOrDefault();
            string key = pi.GetValue(info, null)?.ToString();
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new LogicException("通用单表删除方法要求主键有值");
            }
            DbHelper.Delete(info);
        }

        public T Select<T>(T t) where T : TableEntityBase, new()
        {
            return DbHelper.Select(t);
        }

        public List<T> SelectList<T>(T t) where T : TableEntityBase, new()
        {
            return DbHelper.SelectList(t);
        }
    }
}
