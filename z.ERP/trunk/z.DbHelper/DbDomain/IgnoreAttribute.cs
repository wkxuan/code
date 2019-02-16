using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 忽略的键
    /// 不参与增删查改
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        Inherited = true, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute
    {
    }
}
