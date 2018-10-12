using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.POSO2OWFTCFG
{
    public class PosO2OWftCfgController : BaseController
    {
        public ActionResult PosO2OWftCfg()
        {
            ViewBag.Title = "POS第三方支付配置";
            return View(new DefineRender()
            {
                Permission_Add = "10500501",
                Permission_Mod = "10500502"
            });
        }

        public string Save(POSO2OWFTCFGEntity DefineSave)
        {
            var v = GetVerify(DefineSave);

            v.IsUnique(a => a.POSNO);
            v.Require(a => a.URL);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(POSO2OWFTCFGEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}