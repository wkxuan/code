using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DbHelper.DbDomain
{
    /// <summary>
    /// 外键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        Inherited = true, AllowMultiple = false)]
    public class ForeignKeyAttribute : Attribute
    {
    }
}
