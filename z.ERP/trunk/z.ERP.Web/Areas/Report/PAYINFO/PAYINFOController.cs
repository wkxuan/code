using System.Collections.Generic;
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


        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.PAYINFOOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}