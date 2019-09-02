using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.ContractSale
{
    public class ContractSaleController : BaseController
    {
        public ActionResult ContractSale()
        {
            ViewBag.Title = "租约销售查询";
            return View();
        }

        public UIResult SearchKind()
        {
            var res = service.SpglService.GetKindInit();
            return new UIResult(
                new
                {
                    treeorg = res.Item1
                }
            );
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
            return service.ReportService.ContractSaleOutput(item);
        }
    }
}