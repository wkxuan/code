using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.ContractInfo
{
    public class ContractInfoController : BaseController
    {
        public ActionResult ContractInfo()
        {
            ViewBag.Title = "合同信息表";
            return View();
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

        public string Output(SearchItem item)
        {
            return service.ReportService.ContractInfoOutput(item);
        }
        public UIResult SearchFEE()
        {
            return new UIResult(service.ReportService.SEARCHFEE());
        }
    }
}