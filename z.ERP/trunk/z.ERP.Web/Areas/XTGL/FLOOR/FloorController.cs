using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.Extensions;
using z.MVC5.Results;
using System.Collections.Generic;

namespace z.ERP.Web.Areas.XTGL.FLOOR
{
    public class FloorController: BaseController
    {
        public ActionResult Floor()
        {
            ViewBag.Title = "楼层信息";
            return View();
        }
        public ActionResult FloorDetail(string Id)
        {
            ViewBag.Title = "楼层信息";
            return View("FloorDetail", model: (DefineDetailRender)Id);
        }
        public string Save(FLOOREntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("FLOOR");
            }
            v.IsUnique(a => a.ID);
            v.Require(a => a.CODE);
            v.Require(a => a.NAME);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.REGIONID);
            v.Require(a => a.ORGID);
            v.Require(a => a.AREA_BUILD);
            v.Require(a => a.STATUS);
            v.Verify();
            return CommonSave(DefineSave);
        }
        public void Delete(List<FLOOREntity> DefineDelete)
        {
            foreach (var con in DefineDelete)
            {
                CommenDelete(con);
            }
        }
        public UIResult SearchInit()
        {
            var res = service.DataService.GetTreeOrg();
            return new UIResult(
                new
                {
                    treeOrg = res.Item1
                }
            );
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
            var res = service.DpglService.GetFloor(Data);
            return new UIResult(
                new
                {
                    floorelement = res.Item1
                }
                );
        }
    }
}