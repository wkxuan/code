using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.MVC5.Attributes
{
    /// <summary>
    /// 无需登录特性,此时也不判断权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,
       Inherited = false, AllowMultiple = false)]
    public class IgnoreLoginAttribute : Attribute
    {
    }
}
