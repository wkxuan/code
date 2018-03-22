﻿
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

        public T Select<T>(T t) where T : TableEntityBase
        {
            return DbHelper.Select(t);
        }

        public List<T> SelectList<T>(T t) where T : TableEntityBase, new()
        {
            return DbHelper.SelectList(t);
        }
    }
}
