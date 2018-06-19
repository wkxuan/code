using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.FLOOR
{
    public class FloorController: BaseController
    {
        public ActionResult Floor()
        {
            ViewBag.Title = "楼层信息";
            return View();
        }

        public string Save(FLOOREntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("FLOOR");
            }
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.CODE);
            v.Require(a => a.NAME);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.ORGID);
            v.Require(a => a.AREA_BUILD);
            v.Require(a => a.STATUS);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(FLOOREntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
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
    }
}