using System.Web.Mvc;
using z.ERP.Web.Areas.Base;


namespace z.ERP.Web.Areas.Report.SALEGATHER
{
    public class SALEGATHERController : BaseController
    {
        public ActionResult SALEGATHER()
        {
            ViewBag.Title = "销售数据查询";
            return View();
        }
    }
}