using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.SPLC
{
    public class SPLCController : BaseController
    {
        public ActionResult SPLC()
        {

            ViewBag.Title = "审批流程定义";
            return View();
        }

        public string Save(DEF_ALERTEntity DefineSave)
        {
            return null;
        }
        public void Delete(DEF_ALERTEntity DefineDelete)
        {
        }
    }
}