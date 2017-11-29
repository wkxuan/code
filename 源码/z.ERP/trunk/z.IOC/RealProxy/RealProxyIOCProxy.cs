using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.IOC.RealProxyIOC
{
    internal class RealProxyIOCProxy : RealProxy
    {
        public RealProxyIOCProxy(Type serverType)
            : base(serverType)
        {
        }
        public override IMessage Invoke(IMessage msg)
        {
            //消息拦截之后，就会执行这里的方法。
            if (msg is IConstructionCallMessage)
            {
                IConstructionReturnMessage constructionReturnMessage = this.InitializeServerObject((IConstructionCallMessage)msg);
                SetStubData(this, constructionReturnMessage.ReturnValue);
                return constructionReturnMessage;
            }
            else if (msg is IMethodCallMessage) //如果是方法调用（属性也是方法调用的一种）
            {
                IMethodCallMessage callMsg = msg as IMethodCallMessage;
                object[] args = callMsg.Args;
                IMessage message;
                try
                {
                    RealProxyIOCAttribute attr = callMsg.MethodBase.GetAttribute<RealProxyIOCAttribute>();
                    //没有特性,原路返回
                    if (attr == null)
                    {
                        object o = callMsg.MethodBase.Invoke(GetUnwrappedServer(), args);
                        message = new ReturnMessage(o, args, args.Length, callMsg.LogicalCallContext, callMsg);
                        return message;
                    }
                    ///基础属性注入
                    else if (attr is PropertyBuilderAttribute)
                    {
                        var m = callMsg.MethodBase as MethodInfo;
                        RealProxyIOC si = GetUnwrappedServer() as RealProxyIOC;
                        Type type = si.OverridePropertyType?.FirstOrDefault(a => a.BaseOn(m.ReturnType)) ?? m.ReturnType;
                        object o = Activator.CreateInstance(
                                  type,
                                  BindingFlags.Instance | BindingFlags.NonPublic,
                                  null,
                                  new object[] { },
                                  null);
                        message = new ReturnMessage(o, args, args.Length, callMsg.LogicalCallContext, callMsg);
                        return message;
                    }
                }
                catch (Exception e)
                {
                    message = new ReturnMessage(e, callMsg);
                }
            }
            return msg;
        }
    }
}
