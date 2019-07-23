using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.HTGL.FREESHOP
{
    public class FreeShopController: BaseController
    {
        public ActionResult FreeShopList() {
            ViewBag.Title = "退铺列表信息";
            return View(new SearchRender()
            {
                Permission_Browse = "10600300",
                Permission_Add = "10600301",
                Permission_Del = "10600301",
                Permission_Edit = "10600301",
                Permission_Exec = "10600302"
            });
        }
        public ActionResult FreeShopEdit(string Id)
        {
            ViewBag.Title = "编辑退铺信息";
            return View("FreeShopEdit", model: (EditRender)Id);
        }
        [Permission("10600301")]
        public string Save(FREESHOPEntity SaveData)
        {
            return service.HtglService.SaveFreeShop(SaveData);
        }
        public void Delete(List<FREESHOPEntity> DeleteData)
        {
            service.HtglService.DeleteFreeShop(DeleteData);
        }
        public UIResult GetContractList(CONTRACTEntity Data)
        {
            return new UIResult(service.HtglService.GetContractList(Data));
        }
        public UIResult ShowOneEdit(FREESHOPEntity Data)
        {
            return new UIResult(service.HtglService.ShowOneFreeShopEdit(Data));
        }
        public void ExecData(FREESHOPEntity Data)
        {
            service.HtglService.ExecFreeShop(Data);
        }
        public void StopData(FREESHOPEntity Data)
        {
            service.HtglService.StopFreeShop(Data);
        }
    }
}