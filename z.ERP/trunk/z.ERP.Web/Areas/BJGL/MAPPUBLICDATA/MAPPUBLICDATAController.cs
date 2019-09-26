using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.MAPPUBLICDATA
{
    public class MAPPUBLICDATAController: BaseController
    {
        public ActionResult MAPPUBLICDATA()
        {
            ViewBag.Title = "布局公共设施信息";
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
        public string Save(MAPPUBLICDATAEntity DefineSave)
        {
            return service.DpglService.SaveMAPPUBLICDATA(DefineSave);
        }

        public void Delete(MAPPUBLICDATAEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}