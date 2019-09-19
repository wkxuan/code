using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.CXGL.PROMOBILL_FG
{
    public class Promobill_FGController: BaseController
    {
        public ActionResult Promobill_FGList()
        {
            ViewBag.Title = "促销赠品单";
            return View();
        }
        public ActionResult Promobill_FGEdit(string Id)
        {
            ViewBag.Title = "促销赠品单";
            return View("Promobill_FGEdit", model: (EditRender)Id);
        }
    }
}