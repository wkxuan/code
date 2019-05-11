using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.AlertShow
{
    public class AlertShowController : BaseController
    {
        public ActionResult AlertShow(string Id)
        {
            ViewBag.Title = "预警结果浏览";
            ViewBag.id = Id;
            return View();
        }

        public UIResult SearchAlert(DEF_ALERTEntity Data)
        {
            var res = service.XtglService.GetAlertSql(Data);
            return new UIResult(
                new
                {
                    alert = res.Item1
                }
            );
        }
    }
}