using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using z.MVC5;
using z.MVC5.Controllers;

namespace z.ERP.Web 
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            new MvcStart().Init(DebugSettings.DefaultPage);
        }
    }
}
