using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.SrchORG
{
    public class SrchORGController : BaseController
    {
        public ActionResult SrchORG()
        {
            ViewBag.Title = "组织结构查询";
            return View();
        }
    }
}