using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.REGION
{
    public class RegionController: BaseController
    {
        public ActionResult Region()
        {
            ViewBag.Title = "区域信息";
            return View(new DefineRender()
            {
                Permission_Add = "10101501",
                Permission_Mod = "10101502"
            });
        }

        public string Save(REGIONEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.REGIONID.IsEmpty())
            {
                DefineSave.REGIONID = service.CommonService.NewINC("REGION");
            }
            v.IsUnique(a => a.REGIONID);
            v.IsUnique(a => a.CODE);
            v.Require(a => a.NAME);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.ORGID);
            v.Require(a => a.AREA_BUILD);
            v.Require(a => a.STATUS);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(REGIONEntity DefineDelete)
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
        public UIResult GetRegion(REGIONEntity Data)
        {
            var res =  service.DpglService.GetRegion(Data);
            return new UIResult(
                new
                {
                    regionlement = res.Item1
                }
                );
        }
    }
}