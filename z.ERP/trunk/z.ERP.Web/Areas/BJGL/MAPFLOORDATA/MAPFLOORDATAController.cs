using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.MAPFLOORDATA
{
    public class MAPFLOORDATAController : BaseController
    {
        public ActionResult MAPFLOORDATA()
        {
            ViewBag.Title = "楼层信息";
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
        public UIResult GetFloorMD(MAPFLOORDATAEntity Data)
        {
            return new UIResult(service.DpglService.GetFloorMD(Data));
        }
        public string Save(MAPFLOORDATAEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.FLOORID);
            v.Require(a => a.REGIONID);
            v.Require(a => a.POINTS);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(MAPFLOORDATAEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}