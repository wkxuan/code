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

namespace z.ERP.Web.Areas.DPGL.ASSETSPILT
{
    public class AssetSpiltController : BaseController
    {
        public ActionResult AssetSpiltList(string type)
        {
            ViewBag.Title = "店铺拆分处理";
            ViewBag.Type = "3";
            return View();
        }
        public ActionResult Detail(string Id)
        {
            ViewBag.Title = "店铺拆分浏览";
            var entity = service.DpglService.GetAssetChangeElement(new ASSETCHANGEEntity(Id));
            ViewBag.assetchange = entity.Item1;
            ViewBag.assetchangeitem = entity.Item2;
            ViewBag.assetchangeitem2 = entity.Item3;
            return View(entity);
        }

        public ActionResult AssetSpiltEdit(string Id)
        {
            ViewBag.Title = "编辑店铺拆分单";
            return View(model:Id);
        }

        public void Delete(List<ASSETCHANGEEntity> DeleteData)
        {
            service.DpglService.DeleteAssetChange(DeleteData);
        }


        public string Save(ASSETCHANGEEntity SaveData)
        {
            return service.DpglService.SaveAssetChange(SaveData);
        }

        public UIResult SearchAssetSpilt(ASSETCHANGEEntity Data)
        {
            var res = service.DpglService.GetAssetChangeElement(Data);
            return new UIResult(
                new
                {
                    assetSpilt = res.Item1,
                    assetSpiltitem = res.Item2,
                    assetSpiltitem2 = res.Item3
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