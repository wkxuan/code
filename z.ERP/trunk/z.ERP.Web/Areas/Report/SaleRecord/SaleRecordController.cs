using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.SaleRecord
{
    public class SaleRecordController : BaseController
    {
        public ActionResult SaleRecord()
        {
            ViewBag.Title = "实时销售查询";
            return View();
        }
        //public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        //{
        //    var dtSource = service.ReportService.SaleRecordOutput(item);
        //    return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        //}
    }
}