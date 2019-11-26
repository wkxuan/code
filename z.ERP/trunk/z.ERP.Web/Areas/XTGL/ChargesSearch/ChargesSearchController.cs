using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.ChargesSearch
{
    public class ChargesSearchController: BaseController
    {
        public ActionResult ChargesSearch()
        {
            ViewBag.Title = "手续费对账查询";
            return View();
        }
    }
}