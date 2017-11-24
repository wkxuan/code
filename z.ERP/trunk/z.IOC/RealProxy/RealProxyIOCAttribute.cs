using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace z.IOC.RealProxyIOC
{
    internal class RealProxyIOCBaseProxyAttribute : ProxyAttribute
    {
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            RealProxyIOCProxy realProxy = new RealProxyIOCProxy(serverType);
            return realProxy.GetTransparentProxy() as MarshalByRefObject;
        }
    }

    public class RealProxyIOCAttribute : Attribute
    {

    }
}
