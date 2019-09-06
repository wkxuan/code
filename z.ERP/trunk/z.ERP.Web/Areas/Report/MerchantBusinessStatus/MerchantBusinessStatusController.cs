using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.MerchantBusinessStatus
{
    public class MerchantBusinessStatusController:BaseController
    {
        public ActionResult MerchantBusinessStatus()
        {
            ViewBag.Title = "商户租金经营状况";
            return View();
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.MerchantBusinessStatusOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
        public UIResult SearchCate()
        {
            var res = service.DataService.GetTreeCategory();
            return new UIResult(
                new
                {
                    treeOrg = res.Item1
                }
            );
        }
    }
}