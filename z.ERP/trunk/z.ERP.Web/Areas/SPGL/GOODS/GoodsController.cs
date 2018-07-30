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

namespace z.ERP.Web.Areas.SPGL.GOODS
{
    public class GoodsController:BaseController
    {
        public ActionResult GoodsList()
        {
            ViewBag.Title = "商品信息";
            return View(new SearchRender()
            {
                Permission_Add = "10500201",
                Permission_Del = "10500201",
                Permission_Edit = "10500201",
                Permission_Exec = "10500202"
            });
        }
        public ActionResult GoodsEdit(string Id)
        {
            ViewBag.Title = "编辑商品信息";
            return View("GoodsEdit", model: (EditRender)Id);
        }

        public ActionResult GoodsDetail(string Id)
        {
            ViewBag.Title = "浏览商品信息";
            var entity = service.SpglService.GetGoodsDetail(new GOODSEntity(Id));
            ViewBag.goods = entity.Item1;
            ViewBag.goodsshop = entity.Item2;
            return View(entity);
        }
        [Permission("10500201")]
        public string Save(GOODSEntity SaveData)
        {
            return service.SpglService.SaveGoods(SaveData);
        }

        public void Delete(List<GOODSEntity> DeleteData)
        {
            service.SpglService.DeleteGoods(DeleteData);
        }

        public UIResult ShowOneEdit(GOODSEntity Data)
        {
            return new UIResult(service.SpglService.ShowOneEdit(Data));
        }

        public UIResult GetContract(CONTRACTEntity Data)
        {
            return new UIResult(service.SpglService.GetContract(Data));
        }

        [Permission("10500202")]
        public void ExecData(GOODSEntity Data)
        {
            service.SpglService.ExecData(Data);
        }
        public UIResult SearchInit()
        {
            var res = service.SpglService.GetKindInit();
            return new UIResult(
                new
                {
                    treeorg = res.Item1
                }
            );
        }
    }
}