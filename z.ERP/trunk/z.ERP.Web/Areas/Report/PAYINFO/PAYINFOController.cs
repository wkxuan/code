using System.Web.Mvc;
using z.ERP.Web.Areas.Base;


namespace z.ERP.Web.Areas.Report.PAYINFO
{
    public class PAYINFOController : BaseController
    {
        public ActionResult PAYINFO()
        {
            ViewBag.Title = "第三方支付信息查询";
            return View();
        }
    }
}