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
                return service.ReportService.PayTypeSaleSOutput(item);
            else
                return service.ReportService.PayTypeSaleOutput(item);
        }
    }
}