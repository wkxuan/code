using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.DPGL.ASSETSPILT
{
    public class AssetSpiltController : BaseController
    {
        public ActionResult AssetSpiltList(string type)
        {
            ViewBag.Title = "资产拆分单";
            ViewBag.Type = "3";
            return View();
        }

        public ActionResult AssetSpiltEdit(string Id)
        {
            ViewBag.Title = "资产拆分单详情";
            return View("AssetSpiltEdit", model: (EditRender)Id);
        }

        [Permission("10400201")]
        public void Delete(List<ASSETCHANGEEntity> DeleteData)
        {
            service.DpglService.DeleteAssetChange(DeleteData);
        }

        [Permission("10400201")]
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
            return new UIResult(service.DpglService.GetOneShop(Data));
        }

        [Permission("10400202")]
        public void ExecData(ASSETCHANGEEntity Data)
        {
            service.DpglService.ExecAssetSpilt(Data);
        }
    }
}