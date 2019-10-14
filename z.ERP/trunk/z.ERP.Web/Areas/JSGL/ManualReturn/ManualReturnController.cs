using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.JSGL.ManualReturn
{
    public class ManualReturnController: BaseController
    {
        public ActionResult ManualReturn()
        {
            ViewBag.Title = "手动生成返款单";
            return View();
        }
        public bool ExecReturn(string branchid,string endtime)
        {
            return true;
        }
    }
}