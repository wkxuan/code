using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.MAPSHOW
{
    public class MAPSHOWController : BaseController
    {
        public ActionResult MAPSHOW()
        {
            ViewBag.Title = "布局信息";
            return View();
        }
        public UIResult GetInitMAPDATA(string floorid,string shopstatus) {
            var datas = service.DpglService.GetInitMAPDATA(floorid, shopstatus);
            return new UIResult(
                new { 
                    floorInfo= datas.Item1,
                    labelArray=datas.Item2
                }
                );
        }
        public UIResult GetSHOPINFO(string shopid) {
            var data = service.DpglService.GetSHOPINFO(shopid);
            return new UIResult(
                new
                {
                    shopdata = data.Item1,
                    merchantdata = data.Item2
                });
        }
    }
}