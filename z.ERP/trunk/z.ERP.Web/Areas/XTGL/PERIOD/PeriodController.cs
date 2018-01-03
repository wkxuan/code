using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.PERIOD
{
    public class PeriodController:BaseController
    {
        public ActionResult Period()
        {
            ViewBag.Title = "月账区间信息";
            return View();
        }
        public void Save(List<PERIODEntity> listPeriod)
        {
            foreach(var Period in listPeriod)
            {
                var v = GetVerify(Period);
                CommonSave(Period);
            }

        }

    }
}