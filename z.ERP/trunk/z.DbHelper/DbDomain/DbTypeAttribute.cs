using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 字段类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DbTypeAttribute : Attribute
    {
        /// <summary>
        /// 类型
        /// </summary>
        public DbType DbType
        {
            get;
        }

        /// <summary>
        /// 类型参数
        /// </summary>
        public object[] Param
        {
            get;
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        /// <param name="dbtype"></param>
        /// <param name="param"></param>
        public DbTypeAttribute(DbType dbtype, object[] param = null)
        {
            DbType = dbtype;
            Param = param;
        }
    }
}
