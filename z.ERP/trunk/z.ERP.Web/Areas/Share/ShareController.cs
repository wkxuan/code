using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Share.Render;

namespace z.ERP.Web.Areas.Share
{
    public class ShareController : BaseController
    {
        public ActionResult Undefine(UndefineRender render)
        {
            return View(render);
        }
        public ActionResult TextBox(TextBoxRender render)
        {
            return View(render);
        }
        public ActionResult BaseDropDownList(DropDownListRender render)
        {
            return View(render);
        }
    }
}