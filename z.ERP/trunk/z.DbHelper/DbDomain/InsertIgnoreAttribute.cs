using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 插入时忽略的键
    /// 一般用于数据库允许的自增序列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        Inherited = true, AllowMultiple = false)]
    public class InsertIgnoreAttribute : Attribute
    {

    }
}
