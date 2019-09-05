using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.BILL_SRC
{
    public class Bill_SrcController : BaseController
    {
        public ActionResult Bill_Src()
        {
            ViewBag.Title = "费用账单查询";
            return View();
        }


        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.Bill_SrcOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }


}