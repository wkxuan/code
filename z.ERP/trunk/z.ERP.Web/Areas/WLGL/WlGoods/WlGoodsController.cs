using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Model;
using z.ERP.Entities.Enum;
using System.Data;
using z.MathTools;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.WLGL.WlGoods
{
    public class WlGoodsController : BaseController
    {
        public ActionResult WlGoodsList()
        {
            ViewBag.Title = "物料列表信息";
            return View(new SearchRender()
            {
                Permission_Browse = "10900203",
                Permission_Add = "10900201",
                Permission_Del = "10900201",
                Permission_Edit = "10900201",
                Permission_Exec = "10900202"
            });
        }


        public ActionResult WlGoodsMx(string Id)
        {
            ViewBag.Title = "物料信息浏览";
            var entity = service.WyglService.GetWlGoodsElement(new WL_GOODSEntity(Id));
            ViewBag.goods = entity.Item1;
            return View();
        }


        public ActionResult WlGoodsEdit(string Id)
        {
            ViewBag.Title = "物料信息编辑";

            return View("WlGoodsEdit", model: (EditRender)Id);

        }

        [Permission("10900201")]
        public void Delete(List<WL_GOODSEntity> DeleteData)
        {
            service.WyglService.WLDeleteGoods(DeleteData);
        }

        [Permission("10900201")]
        public string Save(WL_GOODSEntity SaveData)
        {
            return service.WyglService.SaveWlGoods(SaveData);
        }
        public UIResult SearchWlGoods(WL_GOODSEntity Data)
        {
            var res = service.WyglService.GetWlGoodsElement(Data);
            return new UIResult(
                new
                {
                    goods = res.Item1
                }
            );
        }
        [Permission("10900202")]
        public void ExecData(WL_GOODSEntity Data)
        {
            service.WyglService.ExecWLGoodsData(Data);
        }
    }
}