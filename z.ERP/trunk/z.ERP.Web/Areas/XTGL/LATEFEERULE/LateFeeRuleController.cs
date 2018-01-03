using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.LATEFEERULE
{
    public class LateFeeRuleController:BaseController
    {
        public ActionResult LateFeeRule()
        {
            ViewBag.Title = "滞纳规则信息";
            return View();
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