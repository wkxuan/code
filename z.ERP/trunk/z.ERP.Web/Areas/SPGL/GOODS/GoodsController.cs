using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.SPGL.GOODS
{
    public class GoodsController:BaseController
    {
        public ActionResult GoodsList()
        {
            ViewBag.Title = "商品信息";
            return View();
        }
        public ActionResult GoodsEdit(string Id)
        {
            ViewBag.Title = "编辑商品信息";
            return View(model: Id);
        }

        public ActionResult GoodsDetail(string Id)
        {
            ViewBag.Title = "浏览商品信息";
            var entity = service.SpglService.GetGoodsDetail(new GOODSEntity(Id));
            ViewBag.good = entity.Item1;
            return View(entity);
        }

        public string Save(GOODSEntity SaveData)
        {
            return service.SpglService.SaveGoods(SaveData);
        }

        public void Delete(List<GOODSEntity> DeleteData)
        {
            service.SpglService.DeleteGoods(DeleteData);
        }
    }
}