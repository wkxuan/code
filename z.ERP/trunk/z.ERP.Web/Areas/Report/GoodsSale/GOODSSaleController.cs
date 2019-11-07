using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.GoodsSale
{
    public class GoodsSaleController : BaseController
    {
        public ActionResult GoodsSale()
        {
            ViewBag.Title = "商品销售查询";
            return View();
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.GoodsSaleOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}