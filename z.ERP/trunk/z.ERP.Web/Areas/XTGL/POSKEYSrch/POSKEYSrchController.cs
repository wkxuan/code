using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.POSKEYSrch
{
    public class POSKEYSrchController : BaseController
    {
        public ActionResult POSKEYSrch()
        {
            ViewBag.Title = "终端密钥查询";
            return View();
        }

        public string Output(SearchItem item)
        {
            return service.XtglService.POSKEYSrchOutput(item);
        }

    }
}