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
    public class DbTableAttribute : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Tablename
        {
            get;
        }

        /// <summary>
        /// 表的中文名字
        /// </summary>
        public string Tabcomments
        {
            get;
            set;
        }

        /// <summary>
        /// 表的属性
        /// </summary>
        /// <param name="tablename"></param>
        public DbTableAttribute(string tablename, string comments = null)
        {
            Tablename = tablename;
            Tabcomments = comments ?? tablename;
        }

        /// <summary>
        /// 是一张表
        /// 名称同类名
        /// </summary>
        public DbTableAttribute()
        {

        }
    }
}
