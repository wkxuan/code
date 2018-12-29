using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.BJT
{
    /// <summary>
    /// 布局图
    /// </summary>
    public class BJTController : BaseController
    {
        public ActionResult BJTDef()
        {
            ViewBag.Title = "编辑布局图";
            return View();
        }
    }

}