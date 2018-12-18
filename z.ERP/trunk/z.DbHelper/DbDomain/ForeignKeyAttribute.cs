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
