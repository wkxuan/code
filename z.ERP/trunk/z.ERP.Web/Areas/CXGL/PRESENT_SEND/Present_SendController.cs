using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CXGL.PRESENT_SEND
{
    public class Present_SendController:BaseController
    {
        public ActionResult Present_SendList()
        {
            ViewBag.Title = "赠品发放单";
            return View();
        }
        public ActionResult Present_SendEdit(string Id)
        {
            ViewBag.Title = "赠品发放单";
            return View("Present_SendEdit", model: (EditRender)Id);
        }
        public UIResult GetSaleTicket(string BRANCHID,string POSNO,string DEALID) {
            return new UIResult(service.CxglService.GetSaleTicket(BRANCHID, POSNO, DEALID));
        }
    }
}