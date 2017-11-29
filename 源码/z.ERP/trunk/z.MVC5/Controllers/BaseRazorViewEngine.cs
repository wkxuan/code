using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace z.MVC5.Controllers
{
    public class BaseRazorViewEngine : RazorViewEngine
    {
        public BaseRazorViewEngine()
        {
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewLocationFormats = new string[]
           {
                "~/Areas/"+controllerContext.RouteData.Values["area"]+"/{1}/{0}.cshtml"
           };
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }
    }
}
