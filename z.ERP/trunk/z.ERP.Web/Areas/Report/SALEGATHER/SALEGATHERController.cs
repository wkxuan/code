using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.SALEGATHER
{
    public class SALEGATHERController : BaseController
    {
        public ActionResult SALEGATHER()
        {
            ViewBag.Title = "销售采集处理记录查询";
            return View();
        }

        public string Output(SearchItem item)
        {
            return service.ReportService.SALEGATHEROutput(item);
        }

    }
}