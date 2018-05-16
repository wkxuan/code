using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.Services;
using z.Extensiont;
using z.LogFactory;

namespace z.ERP.Portal.svc
{
    public class BaseService
    {
        public BaseService()
        {
            service = new ServiceBase();
        }


        protected ServiceBase service
        {
            get;
            set;
        }

        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Service");
            }
        }

        public T LogRun<T>(Func<T> func, params object[] infos)
        {
            if (func == null)
                return default(T);
            try
            {
                if (infos.IsEmpty())
                    Log.Info("Service", infos);
                return func.Invoke();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

    }
}