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
        public UIResult GetBranch(BRANCHEntity Data)
        {
            return new UIResult(service.DataService.GetBranch(Data));
        }
        public UIResult GetRegion(REGIONEntity Data)
        {
            return new UIResult(service.DataService.GetRegion(Data));
        }
        public UIResult GetFloor(FLOOREntity Data)
        {
            return new UIResult(service.DataService.GetFloor(Data));
        }
        public UIResult GetInitMAPDATA(MAPFLOORINFOEntity data) {
            var datas = service.DpglService.GetInitMAPDATA(data);
            return new UIResult(
                new { 
                    floorInfo= datas.Item1,
                    labelArray=datas.Item2
                }
                );
        }
    }
}