using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public void Save(OPERATIONRULEEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (string.IsNullOrEmpty(DefineSave.ID))
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
            CommonSave(DefineSave);
            
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