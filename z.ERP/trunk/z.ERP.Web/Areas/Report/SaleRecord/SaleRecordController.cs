using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.SaleRecord
{
    public class SaleRecordController : BaseController
    {
        public ActionResult SaleRecord()
        {
            ViewBag.Title = "POS销售查询";
            return View();
        }
    }
}