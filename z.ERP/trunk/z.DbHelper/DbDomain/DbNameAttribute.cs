using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 表的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DbNameAttribute : Attribute
    {
        public DbNameAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string Name
        {
            get;
        }

    }
}
