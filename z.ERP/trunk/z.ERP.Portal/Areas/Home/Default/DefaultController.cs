using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.SSO;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Home.Default
{
    public class DefaultController : BaseController
    {
        public ActionResult Default()
        {
            return View();
        }
    }
}