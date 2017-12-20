using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;

namespace z.ERP.Web.Areas.XTGL.FEESUBJECT
{
    public class FeeSubjectController: BaseController
    {
        public ActionResult FeeSubject() {
            return View();
        }

        public string Save(FEESUBJECTEntity DefineSave)
        {
            var v = GetVerify(DefineSave);

            if (DefineSave.TRIMID.IsEmpty())
            {
                DefineSave.TRIMID = service.CommonService.NewINC("FEESUBJECT");

            }


            v.IsUnique(a => a.TRIMID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.NAME);
            v.Require(a => a.PYM);
            v.Require(a => a.TYPE);
            v.Require(a => a.ACCOUNT);
            v.Require(a => a.DEDUCTION);
            v.Require(a => a.VOID_FLAG);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(FEESUBJECTEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}