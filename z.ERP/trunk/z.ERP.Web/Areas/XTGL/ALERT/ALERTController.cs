using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Define;

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

        public UIResult SearchAlert(DEF_ALERTEntity Data)
        {
            var res = service.XtglService.GetAlertElement(Data);
            return new UIResult(
                new
                {
                    defalert = res.Item1,
                    item = res.Item2
                }
                );
        }

    }
}