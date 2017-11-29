using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using z.MVC5.Controllers;
using z.MVC5.Models;

namespace z.MVC5
{
    public class MvcStart
    {
        public MvcStart()
        {
        }

        public void Init(LoaclePage localepage)
        {
            //AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new ActionProcessAttribute());
            GlobalFilters.Filters.Add(new ExceptionLogAttribute()
            {
                View = "Error"
            });

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: LoaclePage.UrlModel,
                defaults: localepage
            ); //.DataTokens.Add("Area", localepage.area);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BaseRazorViewEngine());

        }

    }
}
