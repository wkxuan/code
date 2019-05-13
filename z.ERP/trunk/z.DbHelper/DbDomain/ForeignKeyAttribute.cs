using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 外键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        Inherited = true, AllowMultiple = true)]
    public class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(string AllKey)
        {
            ParentKey = AllKey;
            ChildrenKey = AllKey;
        }

        /// <summary>
        /// 外键定义
        /// </summary>
        /// <param name="parentkey">父表的键</param>
        /// <param name="childrenkey">子表的键(带类名)</param>
        public ForeignKeyAttribute(string parentkey, string childrenkey)
        {
            ParentKey = parentkey;
            ChildrenKey = childrenkey;
        }

        public string ParentKey
        {
            get;
            set;
        }
        public string ChildrenKey
        {
            get;
            set;
        }
    }
}
