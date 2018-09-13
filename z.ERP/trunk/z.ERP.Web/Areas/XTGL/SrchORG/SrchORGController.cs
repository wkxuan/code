using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

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