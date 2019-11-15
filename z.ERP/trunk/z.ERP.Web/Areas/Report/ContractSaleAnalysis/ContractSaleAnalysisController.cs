using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.ContractSaleAnalysis
{
    public class ContractSaleAnalysisController : BaseController
    {
        public ActionResult ContractSaleAnalysis()
        {
            ViewBag.Title = "租约销售对比分析";
            return View();
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.ContractSaleAnalysisDt(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}