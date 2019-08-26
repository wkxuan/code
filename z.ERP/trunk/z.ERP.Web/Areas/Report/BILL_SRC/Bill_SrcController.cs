using System.Web.Mvc;
using z.ERP.Web.Areas.Base;


namespace z.ERP.Web.Areas.Report.BILL_SRC
{
    public class Bill_SrcController : BaseController
    {
        public ActionResult Bill_Src()
        {
            ViewBag.Title = "费用账单查询";
            return View();
        }
    }
}