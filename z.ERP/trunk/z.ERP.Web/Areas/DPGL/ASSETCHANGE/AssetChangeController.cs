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
        //    return View();
        //}
        public ActionResult AssetChangeList(string type)
        {
            ViewBag.Title = "资产变更单";
            ViewBag.Type = "1";
            return View();
        }
        public ActionResult Detail(string Id)
        {
            ViewBag.Title = "资产变更单浏览";
            var entity = service.DpglService.GetAssetChangeElement(new ASSETCHANGEEntity(Id));
            ViewBag.assetchange = entity.Item1;
            ViewBag.assetchangeitem = entity.Item2;
            return View(entity);
        }

        public ActionResult AssetChangeEdit(string Id)
        {
            ViewBag.Title = "编辑资产调整单";
            return View("AssetChangeEdit",model:Id);
        }

        public void Delete(List<ASSETCHANGEEntity> DeleteData)
        {
            service.DpglService.DeleteAssetChange(DeleteData);
        }


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
            return new UIResult(service.DpglService.GetShop(Data));
        }

        public void ExecData(ASSETCHANGEEntity Data)
        {
            service.DpglService.ExecData(Data);
        }
    }
}