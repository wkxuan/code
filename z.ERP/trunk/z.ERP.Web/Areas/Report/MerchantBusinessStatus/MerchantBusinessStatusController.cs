using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                //固定
                return service.ReportService.MerchantBusinessStatusGDOutput(item);
            else
                //抽成
                return service.ReportService.MerchantBusinessStatusOutput(item);
        }
    }
}