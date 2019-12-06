using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.PPGL.RedemptionRules
{
    public class RedemptionRulesController : BaseController
    {
        public ActionResult RedemptionRules()
        {
            ViewBag.Title = "积分抵现规则";
            return View();
        }
        public ActionResult RedemptionRulesDetail(string Id)
        {
            ViewBag.Title = "积分抵现规则设置";
            return View("RedemptionRulesDetail", model: (DefineDetailRender)Id);
        }
        public string Save(REDEMPTIONRULESEntity DefineSave)
        {
            return service.XtglService.RedemptionRulesSave(DefineSave);
        }
        public void Delete(List<REDEMPTIONRULESEntity> DeleteData)
        {
            service.XtglService.RedemptionRulesDelete(DeleteData);
        }
        public string Begin(REDEMPTIONRULESEntity Data)
        {
            return service.XtglService.RedemptionRulesBegin(Data);
        }
        public string Stop(REDEMPTIONRULESEntity Data)
        {
            return service.XtglService.RedemptionRulesStop(Data);
        }
        public UIResult SearchRedemptionRules(REDEMPTIONRULESEntity Data)
        {
            var res = service.XtglService.SearchRedemptionRulesOne(Data);
            return new UIResult(new { res });
        }
    }
}