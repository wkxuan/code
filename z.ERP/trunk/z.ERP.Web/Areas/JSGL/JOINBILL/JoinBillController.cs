using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace z.ERP.Web.Areas.JSGL.JOINBILL
{
    public class JoinBillController : Controller
    {
        // GET: JSGL/JoinBill
        public ActionResult JoinBillList()
        {
            ViewBag.Title = "联营结算单";
            return View();
        }
    }
}