using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.ContractSale
{
    public class ContractSaleController : BaseController
    {
        public ActionResult ContractSale()
        {
            ViewBag.Title = "租约销售查询";
            return View();
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.ContractSaleOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}