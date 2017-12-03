using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Share.Render;
using z.Results;

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
        public ActionResult ServiceDropDownList(ServiceDropDownListRender render)
        {
            var fun = render.ServiceMothod.Compile();
            render.Data = fun(service.DataService);
            return View("BaseDropDownList", render);
        }
    }
}