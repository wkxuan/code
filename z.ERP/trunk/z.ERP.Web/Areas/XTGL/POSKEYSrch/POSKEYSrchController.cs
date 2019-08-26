using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.POSKEYSrch
{
    public class POSKEYSrchController : BaseController
    {
        public ActionResult POSKEYSrch()
        {
            ViewBag.Title = "终端密钥查询";
            return View();
        }
    }
}