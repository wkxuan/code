using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.PAYINFO
{
    public class PAYINFOController : BaseController
    {
        public ActionResult PAYINFO()
        {
            ViewBag.Title = "第三方支付记录查询";
            return View();
        }

        public string Output(SearchItem item)
        {
            return service.ReportService.PAYINFOOutput(item);
        }
    }
}