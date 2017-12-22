using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.OPERATIONRULE
{
    public class OperationruleController:BaseController
    {
        public ActionResult Operationrule()
        {
            return View();
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