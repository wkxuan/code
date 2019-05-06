using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Portal.Areas.Home.DefaultNew
{
    public class DefaultNewController:BaseController
    {
        public ActionResult DefaultNew() {
            return View();
        }
    }
}