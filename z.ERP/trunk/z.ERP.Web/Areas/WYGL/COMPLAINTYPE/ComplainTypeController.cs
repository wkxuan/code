using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.WYGL.COMPLAINTYPE
{
    public class ComplainTypeController : BaseController
    {
        public ActionResult ComplainType()
        {

            ViewBag.Title = "投诉类型定义";
            return View(new DefineRender()
            {
                Permission_Add = "10300401",
                Permission_Mod = "10300401",
                Invisible_Srch = true
            });
        }

        public string Save(COMPLAINTYPEEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = service.CommonService.NewINC("COMPLAINTYPE");
            v.Require(a => a.NAME);
            v.IsNumber(a => a.ID);
            v.IsUnique(a => a.NAME);
            v.Verify();
            return CommonSave(DefineSave);
        }
        public void Delete(COMPLAINTYPEEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
}