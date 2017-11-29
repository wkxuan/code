
using System;
using System.Collections.Generic;
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
using z.DbHelper.DbDomain;
using z.Exceptions;
using z.Extensiont;

namespace z.ERP.Services
{
    public class CommonService : ServiceBase
    {
        internal CommonService()
        {
        }

        /// <summary>
        /// 通用的单表存储方式
        /// </summary>
        /// <param name="infos"></param>
        public void CommonSave(IEnumerable<EntityBase> infos)
        {
            using (var tran = DbHelper.BeginTransaction())
            {
                infos.ForEach(CommonSave);
                tran.Commit();
            }
        }

        /// <summary>
        /// 通用的单表存储方式
        /// </summary>
        /// <param name="info"></param>
        public void CommonSave(EntityBase info)
        {
            PropertyInfo[] pis = info.GetPrimaryKey();
            if (pis == null || pis.Length != 1)
            {
                throw new LogicException("只有唯一主键的类适用于通用存储方法");
            }
            PropertyInfo pi = pis.FirstOrDefault();
            string key = pi.GetValue(info, null)?.ToString();
            if (string.IsNullOrWhiteSpace(key))
            {
                pi.SetValue(info, NewINC(info), null);
            }
            DbHelper.Save(info);
        }

        /// <summary>
        /// 通用的单表存储方式
        /// </summary>
        /// <param name="infos"></param>
        public void CommenDelete(IEnumerable<EntityBase> infos)
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
        public void CommenDelete(EntityBase info)
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
    }
}
