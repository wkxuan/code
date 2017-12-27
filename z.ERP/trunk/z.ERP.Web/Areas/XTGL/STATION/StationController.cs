using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.STATION
{
    public class StationController: BaseController
    {
        public ActionResult Station()
        {
            return View();
        }
    }
}