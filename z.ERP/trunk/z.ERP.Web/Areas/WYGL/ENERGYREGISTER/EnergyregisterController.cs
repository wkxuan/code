using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.WYGL.ENERGYREGISTER
{
    public class EnergyreGisterController: BaseController
    {
        public ActionResult EnergyreGisterList()
        {
            ViewBag.Title = "能源设备登记(抄表)";
            return View();
        }

        public ActionResult EnergyreGisterEdit()
        {
            ViewBag.Title = "编辑能源设备登记(抄表)";
            return View();
        }

        public ActionResult EnergyreGisterDetail()
        {
            ViewBag.Title = "浏览能源设备登记(抄表)";
            return View();
        }

        public string Save(ENERGY_REGISTEREntity SaveData)
        {
            return service.WyglService.SaveEnergyreGister(SaveData);
        }
    }
}