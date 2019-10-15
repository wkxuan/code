using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.RCL
{
    public class RCLController : BaseController
    {
        public ActionResult RCL()
        {
            ViewBag.Title = "日处理";
            return View();
        }

        public void Exec(WRITEDATAEntity WRITEDATA)
        {
            //  service.WriteDataService.CanRcl(WRITEDATA);
        }
    }
}