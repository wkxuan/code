using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
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
        public string Save(STATIONEntity DefineSave, List<STATION_PAYEntity> PaySave)
        {
           return service.XtglService.SaveSataion(DefineSave,PaySave);             
        }

        public STATIONEntityMoldel GetStaionElement(STATIONEntity Staion)
        {            
            return service.XtglService.GetStaionElement(Staion);
        }
    }
}