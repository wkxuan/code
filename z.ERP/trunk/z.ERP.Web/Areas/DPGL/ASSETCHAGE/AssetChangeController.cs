using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.DPGL.ASSETCHANGE
{
    public class AssetChangeController : BaseController
    {
        public ActionResult AssetChangeList()
        {
            ViewBag.Title = "资产变更单";
            return View();
        }

        public ActionResult Detail(string Id)
        {
            ViewBag.Title = "资产变更单浏览";
            ASSETCHANGEEntity entity = Select(new ASSETCHANGEEntity(Id));
            return View(entity);
        }

        public ActionResult AssetChangeEdit()
        {
            ViewBag.Title = "商户信息编辑";
            return View();
        }

        public void Delete(ASSETCHANGEEntity DeleteData)
        {
            service.DpglService.DeleteAssetChange(DeleteData);
        }


        public string Save(ASSETCHANGEEntity SaveData)
        {
            return service.DpglService.SaveAssetChange(SaveData);
        }

        public UIResult SearchElement(ASSETCHANGEEntity Data)
        {
            return new UIResult(service.DpglService.GetAssetChangeElement(Data));
        }
    }
}