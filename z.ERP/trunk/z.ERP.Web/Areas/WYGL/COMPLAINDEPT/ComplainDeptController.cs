using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.WYGL.COMPLAINDEPT
{
    public class ComplainDeptController:BaseController
    {
        public ActionResult ComplainDept()
        {

            ViewBag.Title = "投诉部门定义";
            return View(new DefineRender()
            {
                Permission_Add = "10300301",
                Permission_Mod = "10300301",
                Invisible_Srch = true
            });
        }

        public string Save(COMPLAINDEPTEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = service.CommonService.NewINC("COMPLAINDEPT");
            v.Require(a => a.NAME);
            v.IsNumber(a => a.ID);
            v.IsUnique(a => a.NAME);
            v.Verify();
            return CommonSave(DefineSave);
        }
        public void Delete(COMPLAINDEPTEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
}