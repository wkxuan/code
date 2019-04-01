using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Layout.Define;
using z.Extensions;

namespace z.ERP.Web.Areas.XTGL.POSO2OWFTCFG
{
    public class FeeAccountController : BaseController
    {
        public ActionResult FeeAccount()
        {
            ViewBag.Title = "收费单位定义";
            return View(new DefineRender()
            {
                Permission_Add = "10500501",
                Permission_Mod = "10500502"
            });
        }

        public string Save(FEE_ACCOUNTEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("FEEACCOUNT");
            }
            v.IsUnique(a => a.ID);
            v.Require(a => a.NAME);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(FEE_ACCOUNTEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}