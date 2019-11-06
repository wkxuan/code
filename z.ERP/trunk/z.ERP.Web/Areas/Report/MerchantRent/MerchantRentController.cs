using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.MerchantRent
{
    public class MerchantRentController : BaseController
    {
        public ActionResult MerchantRent()
        {
            ViewBag.Title = "商户租金计提表";
            return View();
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.MerchantRentOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}