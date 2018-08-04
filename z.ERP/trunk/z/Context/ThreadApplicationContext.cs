using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace z.Context
{
    public class ThreadApplicationContext : ApplicationContextBase
    {
        [ThreadStatic]
        public static Hashtable dataBag = null;
        private Hashtable DataBag
        {
            get
            {
                if (dataBag == null)
                {
                    dataBag = new Hashtable();
                }
                return dataBag;
            }
        }

        public override T GetData<T>(string name)
        {
            T data = null;
            if (DataBag.Contains(name))
            {
                data = DataBag[name] as T;
            }
            return data;
        }

        public override void SetData<T>(string name, T data)
        {
            DataBag[name] = data;
        }

        public override void RemoveData(string name)
        {
            DataBag.Remove(name);
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
