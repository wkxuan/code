using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.PAY_CHARGES
{
    public class Pay_ChargesController: BaseController
    {
        public ActionResult Pay_Charges()
        {
            ViewBag.Title = "收款方式手续费设置";
            return View();
        }
        public UIResult GetBranch(BRANCHEntity Data)
        {
            return new UIResult(service.DataService.GetBranch(Data));
        }
        public UIResult GetPAY()
        {
            return new UIResult(service.XtglService.GetPAY());
        }
        public string Save(PAY_CHARGESEntity DefineSave)
        {           
            return service.XtglService.PAY_CHARGESSAVE(DefineSave);
        }
        public UIResult GetPay_ChargesOne(PAY_CHARGESEntity data)
        {
            return service.XtglService.GetPay_ChargesOne(data);
        }
    }
}