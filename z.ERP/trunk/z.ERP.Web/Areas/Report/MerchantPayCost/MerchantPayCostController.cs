using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.MerchantPayCost
{
    public class MerchantPayCostController:BaseController
    {
        public ActionResult MerchantPayCost()
        {
            ViewBag.Title = "租赁商户缴费查询";
            return View();
        }
        public string Output(SearchItem item)
        {
            return service.ReportService.MerchantPayCostOutput(item);
        }
    }
}
