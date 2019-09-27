using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.BJGL.PUBLICDATA
{
    public class PUBLICDATAController: BaseController
    {
        public ActionResult PUBLICDATA()
        {
            ViewBag.Title = "公共设施信息";
            return View();
        }
        public string Save(PUBLICDATAEntity DefineSave)
        {
            return service.DpglService.PUBLICDATASave(DefineSave);
        }

        public void Delete(PUBLICDATAEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}