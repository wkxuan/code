using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.MerchantPayable
{
    public class MerchantPayableController : BaseController
    {
        public ActionResult MerchantPayable()
        {
            ViewBag.Title = "商户应交已付报表";
            return View();
        }
        //获取收费项目list
        public UIResult GetSfxmList()
        {
            var res = service.ReportService.GetSfxmList();
            return new UIResult(
                new
                {
                  res
                }
            );
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.MerchantPayableOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}