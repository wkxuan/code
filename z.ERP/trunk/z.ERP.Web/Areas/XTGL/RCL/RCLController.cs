using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.MVC5.Results;

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