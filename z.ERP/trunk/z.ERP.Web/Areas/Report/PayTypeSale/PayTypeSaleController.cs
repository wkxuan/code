using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.PayTypeSale
{
    public class PayTypeSaleController : BaseController
    {
        public ActionResult PayTypeSale()
        {
            ViewBag.Title = "收款方式销售查询";
            return View();
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.PayTypeSaleOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}