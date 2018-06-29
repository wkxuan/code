using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.BRANCH
{
    public class BranchController: BaseController
    {
        public ActionResult Branch()
        {
            ViewBag.Title = "分店信息";
            return View(new DefineRender()
            {
                Permission_Add = "10100401",
                Permission_Mod = "10100402",
                Invisible_Srch = true   //设置查询按扭不可见
            });
        }

        public string Save(BRANCHEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("BRANCH");
            }
            v.IsUnique(a => a.ID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.NAME);
            v.Require(a => a.ORGID);
            v.IsUnique(a => a.ORGID);
            v.Require(a => a.AREA_BUILD);
            v.Require(a => a.STATUS);
            v.Verify();
            service.XtglService.Org_Update(DefineSave.ORGID, DefineSave.ID.ToInt());
            return CommonSave(DefineSave);
        }

        public void Delete(BRANCHEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}