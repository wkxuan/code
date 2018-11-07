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
        public string Output(SearchItem item)
        {
            return service.ReportService.GoodsSaleOutput(item);
        }
    }
}