using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.MerchantBusinessStatus
{
    public class MerchantBusinessStatusController:BaseController
    {
        public ActionResult MerchantBusinessStatus()
        {
            ViewBag.Title = "商户租金经营状况";
            return View();
        }
        public string Output(SearchItem item)
        {
            return service.ReportService.MerchantBusinessStatusOutput(item);
        }
    }
}