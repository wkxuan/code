using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.IOC.RealProxyIOC
{
    /// <summary>
    /// 受到SimpleIOC体系管理
    /// </summary>
    [RealProxyIOCBaseProxy]
    public class RealProxyIOC : ContextBoundObject
    {
        /// <summary>
        /// 需要重写的新类型
        /// </summary>
        public virtual Type[] OverridePropertyType
        {
            get;
            set;
        }
    }
}
