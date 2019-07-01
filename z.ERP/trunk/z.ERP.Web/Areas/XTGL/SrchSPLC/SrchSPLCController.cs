using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using System.Collections.Generic;
using z.MVC5.Results;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.Search;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Entities.Enum;

namespace z.ERP.Web.Areas.XTGL.SrchSPLC
{
    public class SrchSPLCController : BaseController
    {
        public ActionResult SrchSPLC()
        {
            ViewBag.Title = "审批流程列表信息";
            return View(new SearchRender());
        }
    }
}