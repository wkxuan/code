using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.WLGL.WLSrchStock
{
    public class WLSrchStockController : BaseController
    {
        public ActionResult WLSrchStock()
        {
            ViewBag.Title = "物料库存表";
            return View();
        }
    }
}