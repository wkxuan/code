using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WYGL.ENERGYREGISTER
{
    public class EnergyreGisterController : BaseController
    {
        public ActionResult EnergyreGisterList()
        {
            ViewBag.Title = "能源设备登记(抄表)";
            return View();
        }

        public ActionResult EnergyreGisterEdit(string Id)
        {
            ViewBag.Title = "编辑能源设备登记(抄表)";
            return View(model: (EditRender)Id);
        }

        public ActionResult EnergyreGisterDetail(string Id)
        {
            ViewBag.Title = "浏览能源设备登记(抄表)";
            var entity = service.WyglService.GetRegisterDetail(new ENERGY_REGISTEREntity(Id));
            ViewBag.regist = entity.Item1;
            ViewBag.item = entity.Item2;
            return View(entity);
        }

        public string Save(ENERGY_REGISTEREntity SaveData)
        {
            return service.WyglService.SaveEnergyreGister(SaveData);
        }

        [Permission("1")]
        public UIResult SearchElement(ENERGY_REGISTEREntity Data)
        {
            return new UIResult(service.WyglService.GetEnergyreGisterElement(Data));
        }

        public void Delete(List<ENERGY_REGISTEREntity> DeleteData)
        {
            service.WyglService.DeleteEnergyreGister(DeleteData);
        }

        public UIResult GetRegister(ENERGY_FILESEntity Data)
        {
            return new UIResult(service.WyglService.GetRegister(Data));
        }
        public void ExecData(ENERGY_REGISTEREntity Data)
        {
            service.WyglService.ExecData(Data);
        }
    }
}