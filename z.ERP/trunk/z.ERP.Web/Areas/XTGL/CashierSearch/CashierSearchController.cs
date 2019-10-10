using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.CashierSearch
{
    public class CashierSearchController:BaseController
    {
        public ActionResult CashierSearch()
        {
            ViewBag.Title = "收银员查询";
            return View();
        }
    }
}