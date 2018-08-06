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
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.DPGL.ASSETCHANGE
{
    public class AssetChangeController : BaseController
    {
        //public ActionResult AssetTypeChangeList()
        //{
        //    ViewBag.Title = "资产类型变更单";
        //    return View();
        //}
        //public ActionResult AssetAreaChangeList()
        //{
        //    ViewBag.Title = "资产面积变更单";
        //    return View();10400101
        //}
        public ActionResult AssetChangeList(string type)
        {
            ViewBag.Title = "资产面积变更";
            return View(new SearchRender()
            {
                Permission_Add = "10400201",
                Permission_Del = "10400201"
            });
        }
        public ActionResult AssetChangeDetail(string Id)
        {
            ViewBag.Title = "资产面积变更";
            var entity = service.DpglService.GetAssetChangeElement(new ASSETCHANGEEntity(Id));
            ViewBag.assetchange = entity.Item1;
            ViewBag.assetchangeitem = entity.Item2;
            return View(entity);
        }

        public ActionResult AssetChangeEdit(string Id)
        {
            ViewBag.Title = "资产面积变更";
            return View("AssetChangeEdit",model: (EditRender)Id);
        }

        public void Delete(List<ASSETCHANGEEntity> DeleteData)
        {
            service.DpglService.DeleteAssetChange(DeleteData);
        }

        [Permission("10400201")]
        public string Save(ASSETCHANGEEntity SaveData)
        {
            return service.DpglService.SaveAssetChange(SaveData);
        }

        public UIResult SearchAssetChange(ASSETCHANGEEntity Data)
        {
            var res = service.DpglService.GetAssetChangeElement(Data);
            return new UIResult(
                new
                {
                    assetchange = res.Item1,
                    assetchangeitem = res.Item2
                }
                );
        }
        public UIResult GetShop(SHOPEntity Data)
        {
            return new UIResult(service.DpglService.GetOneShop(Data));
        }
        [Permission("10400202")]
        public void ExecData(ASSETCHANGEEntity Data)
        {
            service.DpglService.ExecAssetChange(Data);
        }
    }
}