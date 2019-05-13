using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.MerchantPayCost
{
    public class MerchantPayCostController:BaseController
    {
        public ActionResult MerchantPayCost()
        {
            ViewBag.Title = "租赁商户缴费查询";
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
            if (item.Values["SrchTYPE"] == "2")
                return service.ReportService.MerchantPayCostSOutput(item);
            else
                return service.ReportService.MerchantPayCostOutput(item);
        }
    }
}
