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
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = service.CommonService.NewINC("DEF_ALERT");
            v.Require(a => a.MC);
            v.IsNumber(a => a.ID);
            v.IsUnique(a => a.MC);
            v.Verify();
            return CommonSave(DefineSave);
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