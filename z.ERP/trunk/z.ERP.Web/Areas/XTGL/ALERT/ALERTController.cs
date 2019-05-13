using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;
using z.Extensions;

namespace z.ERP.Web.Areas.XTGL.ALERT
{
    public class ALERTController : BaseController
    {
        public ActionResult ALERT()
        {

            ViewBag.Title = "预警信息定义";
            return View();
        }

        public string Save(DEF_ALERTEntity DefineSave)
        {
            return service.XtglService.SaveAlert(DefineSave);
        }
        public void Delete(DEF_ALERTEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }

    }
}