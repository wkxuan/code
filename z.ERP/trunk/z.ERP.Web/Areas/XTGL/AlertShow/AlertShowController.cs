using System.Collections.Generic;
using System.Data;
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
            ViewBag.id = Id;
            return View();
        }

        public UIResult SearchAlert(DEF_ALERTEntity Data)
        {
            //返回预警的列表给col,返回预警的结果给data
            var res = service.XtglService.GetAlertSql(Data);
            return new UIResult(
                new
                {
                    alertData = res.Item1,
                    alertCol = res.Item2
                }
            );
        }
        public string Output(Dictionary<string, string> Cols, DEF_ALERTEntity Data)
        {
            var res = service.XtglService.GetAlertSql(Data);
            return NPOIHelper.ExportExcel(res.Item1, res.Item3, Cols);
        }
    }
}