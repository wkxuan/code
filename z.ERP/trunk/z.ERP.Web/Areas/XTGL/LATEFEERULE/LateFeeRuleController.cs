using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.LATEFEERULE
{
    public class LateFeeRuleController:BaseController
    {
        public ActionResult LateFeeRule()
        {
            ViewBag.Title = "滞纳规则信息";
            return View(new DefineRender()
            {
                Permission_Add = "10100801",
                Permission_Mod = "10100802",
                Invisible_Srch = true
            });
        }
        public string Save(LATEFEERULEEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = service.CommonService.NewINC("LATEFEERULE");
            v.Require(a => a.ID);
            v.IsUnique(a => a.ID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.NAME);
            v.Require(a => a.DAYS);
            v.Require(a => a.AMOUNTS);
            DefineSave.RATIO = (DefineSave.RATIO.ToDouble()/100).ToString();
            v.Verify();
            return CommonSave(DefineSave);
        }
        public void Delete(LATEFEERULEEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
    
}