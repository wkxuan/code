using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.LogFactory;
using z.DGS.Services;
using z.SSO;
using z.SSO.Model;
using z.WebServiceBase.Controllers;

namespace z.DGS.WebService.Controllers
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