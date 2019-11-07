using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.GoodsSaleDetail
{
    public class GoodsSaleDetailController:BaseController
    {
        public ActionResult GoodsSaleDetail()
        {
            ViewBag.Title = "商品销售明细查询";
            return View();
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.GoodsSaleDetailOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}