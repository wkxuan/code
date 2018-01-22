using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;
using static z.ERP.Services.XtglService;

namespace z.ERP.Web.Areas.XTGL.STATION
{
    public class StationController: BaseController
    {
        public ActionResult Station()
        {
            ViewBag.Title = "收银终端信息";
            return View();
        }
        public string Save(STATIONEntity DefineSave)
        {
           return service.XtglService.SaveSataion(DefineSave);             
        }

        public UIResult GetStaionElement(STATIONEntity Staion)
        {            
            return new UIResult(service.XtglService.GetStaionElement(Staion));
        }

        public UIResult GetStaionPayList()
        {
            return new UIResult(service.XtglService.GetStaionPayList());
        }
        public void Delete(STATIONEntity DefineDelete)
        {
            service.XtglService.DeleteStation(DefineDelete);
        }
    }
}