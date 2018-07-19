using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.FEERULE
{
    public class FeeRuleController:BaseController
    {
        public ActionResult FeeRule()
        {
            ViewBag.Title = "收费规则信息";
            return View(new DefineRender()
            {
                Permission_Add = "10100901",
                Permission_Mod = "10100902",
                Invisible_Srch = true
            });
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
            if ((Convert.ToInt32(DefineSave.FEE_DAY) <= 0 && Convert.ToInt32(DefineSave.FEE_DAY) != -1) || Convert.ToInt32(DefineSave.FEE_DAY) > 28)
            {
                throw new Exception("出单日期不能小于0或者大于28");
            }
            if ((Convert.ToInt32(DefineSave.UP_DATE) <= 0 && Convert.ToInt32(DefineSave.UP_DATE) != -1) || Convert.ToInt32(DefineSave.UP_DATE) > 28)
            {
                throw new Exception("缴费截至日不能小于0或者大于28");
            }
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