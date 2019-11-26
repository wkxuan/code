using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Model.Tree;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.SYSFUNCCONFIG
{
    public class SysFuncConfigController : BaseController
    {
        public ActionResult SysFuncConfig()
        {
            ViewBag.Title = "系统功能配置";
            return View();
        }
        public UIResult Add(List<MENUMODULEEntity> Data)
        {
            var res = service.XtglService.AddUserModule(Data);
            return new UIResult(new { res });
        }
        public string Edit(MENUMODULEEntity Data)
        {
            return service.XtglService.EditUserModule(Data);
        }
        public string Delete(MENUMODULEEntity Data)
        {
            return service.XtglService.DeleteUserModule(Data);
        }
        public string RoundUpAndDown(List<MENUMODULEEntity> Data)
        {
            return service.XtglService.UpAndDownUserModule(Data);
        }
        public UIResult GetMenuModule(MENUMODULEEntity Data)
        {
            var module = service.XtglService.GetMenuModule(Data);
            return new UIResult(new { module });
        }
    }
}