using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.STATIONMONITOR
{
    public class STATIONMONITORController:BaseController
    {
        public ActionResult STATIONMONITOR()
        {
            ViewBag.Title = "收银终端监控";
            return View();
        }
        public UIResult GetInitData()
        {
            return new UIResult(service.XtglService.GetSTATIONMONITOR());
        }
        public UIResult GetSTATIONSALE(string stationid) {
            var data = service.XtglService.GetSTATIONSALE(stationid);
            return new UIResult(
                new {
                    BRANCHAMOUNT= data.Item1,
                    STATIONPAYDATA= data.Item2
                }
                );
        }
    }
}