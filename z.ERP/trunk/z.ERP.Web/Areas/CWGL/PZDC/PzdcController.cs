using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CWGL.PZDC
{
    public class PzdcController: BaseController
    {
        public ActionResult Pzdc()
        {
            ViewBag.Title = "凭证导出";
            return View();
        }
        //public string ExportPz(string Data)
        //{
        //    return service.CwglService.ExportPz(Data);
        //}
    }

}