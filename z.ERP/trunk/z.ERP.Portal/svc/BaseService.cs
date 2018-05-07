using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.Services;

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

    }
}