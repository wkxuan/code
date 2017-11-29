using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.IOC.RealProxyIOC
{
    /// <summary>
    /// 属性注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PropertyBuilderAttribute : RealProxyIOCAttribute
    {

    }
}
