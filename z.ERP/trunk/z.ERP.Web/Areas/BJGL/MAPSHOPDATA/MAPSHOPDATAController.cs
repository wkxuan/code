using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.MAPSHOPDATA
{
    public class MAPSHOPDATAController : BaseController
    {
        public ActionResult MAPSHOPDATA()
        {
            ViewBag.Title = "单元信息";
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
        public string Save(MAPSHOPDATAEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            v.Require(a => a.SHOPID);
            v.Require(a => a.TITLEPOINTS);
            v.Require(a => a.POINTS);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(MAPSHOPDATAEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }

}
