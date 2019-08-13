using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.Extensions;

namespace z.ERP.Web.Areas.XTGL.BRANCH
{
    public class BranchController: BaseController
    {
        public ActionResult Branch()
        {
            ViewBag.Title = "门店信息";
            return View();
        }
        public ActionResult BranchDetail(string Id)
        {
            ViewBag.Title = "门店信息";
            return View("BranchDetail", model: (DefineDetailRender)Id);
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