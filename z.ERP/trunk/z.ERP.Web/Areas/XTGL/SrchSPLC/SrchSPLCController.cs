using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.SrchSPLC
{
    public class SrchSPLCController : BaseController
    {
        public ActionResult SrchSPLC()
        {
            ViewBag.Title = "审批流程列表信息";
            return View();
        }
    }
}