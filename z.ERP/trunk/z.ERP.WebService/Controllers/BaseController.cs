using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.Services;
using z.IOC.Simple;
using z.LogFactory;
using z.SSO;
using z.SSO.Model;
using z.WebServiceBase.Controllers;

namespace z.ERP.WebService.Controllers
{
    public class BaseController : ServiceBaseController
    {
        internal BaseController() : base()
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