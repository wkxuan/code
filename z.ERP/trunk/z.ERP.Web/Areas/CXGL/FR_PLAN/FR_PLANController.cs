using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CXGL.FR_PLAN
{
    public class FR_PLANController:BaseController
    {
        public ActionResult FR_PLAN()
        {
            ViewBag.Title = "满减方案";
            return View();
        }
        public string Save(FR_PLANEntity DefineSave)
        {
            return service.CxglService.SaveFRPLAN(DefineSave);
        }
        public UIResult SearchFRPLANInfo(FR_PLANEntity Data)
        {
            var res = service.CxglService.GetFRPLANInfo(Data);
            return new UIResult(
                new
                {
                    FRPLANInfo = res.Item1,
                    Item = res.Item2
                }
            );
        }
    }
}