using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using z.ERP.Web.Areas.Layout.Define;
using z.Exceptions;

namespace z.ERP.Web.Areas.XTGL.FEESUBJECT
{
    public class FeeSubjectController: BaseController
    {
        public ActionResult FeeSubject() {
            ViewBag.Title = "收费项目信息";
            return View(new DefineRender()
            {
                Permission_Add = "10100501",
                Permission_Mod = "10100502"
            });
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
            if (DefineDelete.TRIMID.ToInt() >= 2000) {
                throw new LogicException($"预定义的收费项目不能删除!");
            }
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}