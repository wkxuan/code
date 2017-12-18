using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.FEERULE
{
    public class FeeRuleController:BaseController
    {
        public ActionResult FeeRule()
        {
            return View();
        }
        public string Save(FEERULEEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = service.CommonService.NewINC("FEERULE");
            v.Require(a => a.ID);
            v.IsUnique(a => a.ID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.NAME);
            v.Require(a => a.PAY_CYCLE);        //缴费周期
            v.Require(a => a.ADVANCE_CYCLE);    //提前周期
            v.Require(a => a.FEE_DAY);          //出单日期
            v.Verify();
            return CommonSave(DefineSave);
        }
        public void Delete(FEERULEEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
    
}