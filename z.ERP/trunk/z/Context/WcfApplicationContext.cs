using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Web;

namespace z.Context
{
    public class WcfApplicationContext : ApplicationContextBase
    {
        public override T GetData<T>(string name)
        {
            T data = null;
            OpContext<T>.Current.ContextSet.TryGetValue(name, out data);
            return data;
        }

        public override void SetData<T>(string name, T data)
        {
            OpContext<T>.Current.ContextSet[name] = data;
        }

        public override void RemoveData(string name)
        {
            OpContext<object>.Current.ContextSet.Remove(name);
        }
        
        class OpContext<T> : IExtension<OperationContext>
        {
            private OpContext()
            {
                ContextSet = new Dictionary<string, T>();
            }

            public static OpContext<T> Current
            {
                get
                {
                    OpContext<T> opCtx = OperationContext.Current.Extensions.Find<OpContext<T>>();
                    if (opCtx == null)
                    {
                        opCtx = new OpContext<T>();
                        OperationContext.Current.Extensions.Add(opCtx);
                    }
                    return opCtx;
                }
            }
            public IDictionary<string, T> ContextSet
            {
                get;
                private set;
            }

            public void Attach(OperationContext owner)
            {
            }

            public void Detach(OperationContext owner)
            {
            }
        }

        public override IPrincipal principal
        {
            get
            {
                return Thread.CurrentPrincipal;
            }
            set
            {
                Thread.CurrentPrincipal = value;
            }
        }
    }
}
