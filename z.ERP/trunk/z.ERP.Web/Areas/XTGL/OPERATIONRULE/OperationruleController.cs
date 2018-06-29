using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.OPERATIONRULE
{
    public class OperationruleController:BaseController
    {
        public ActionResult Operationrule()
        {
            ViewBag.Title = "租赁收费规则信息";
            return View(new DefineRender()
            {
                Permission_Add = "10101101",
                Permission_Mod = "10101102",
                Invisible_Srch = true
             });
        }
        public string Save(OPERATIONRULEEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("OPERATIONRULE");
            }
            v.Require(a => a.ID);
            v.Require(a => a.NAME);
            v.Require(a => a.WYSIGN);
            v.Require(a => a.PROCESSTYPE);
            v.Require(a => a.LADDERSIGN);
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.NAME);
            v.Verify();
            return CommonSave(DefineSave);            
        }
        public void Delete(OPERATIONRULEEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }


    }
}