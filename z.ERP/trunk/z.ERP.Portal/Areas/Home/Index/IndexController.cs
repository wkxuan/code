using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.SSO;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Home.Index
{
    public class IndexController : BaseController
    {
        // GET: Home/Index
        public ActionResult Index()
        {
            return View();
        }

        public UIResult GetMenu(MENUEntity data)
        {
            string host = Request.Url.Host;
            return service.HomeService.GetMenuNew(data, host);
        }
    }
}