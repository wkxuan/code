using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.FEESUBJECT_ACCOUNT
{
    public class FEESUBJECT_ACCOUNTController:BaseController
    {
        public ActionResult FEESUBJECT_ACCOUNT()
        {
            ViewBag.Title = "费用项目门店配置";
            return View(new DefineRender()
            {
                //Permission_Add = "10100501",
                //Permission_Mod = "10100502"
            });
        }
        public string Save(FEESUBJECT_ACCOUNTEntity DefineSave)
        {           
            return service.XtglService.SAVEFEESUBJECT_ACCOUNT(DefineSave);
        }
        public UIResult GetBranch(BRANCHEntity Data)
        {
            return new UIResult(service.DataService.GetBranch(Data));
        }
        public UIResult GetFEE_ACCOUNTDATA(FEE_ACCOUNTEntity Data)
        {
            return new UIResult(service.DataService.feeAccount(Data));
        }
        
    }
}