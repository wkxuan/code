using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WLGL.WlMerchant
{
    public class WlMerchantController : BaseController
    {
        public ActionResult WlMerchantList()
        {
            ViewBag.Title = "物料供货商列表信息";
            return View();
        }
        public ActionResult WlMerchantEdit(string Id)
        {
            ViewBag.Title = "物料供货商信息息编辑";

            return View("WlMerchantEdit", model: (EditRender)Id);
        }

        [Permission("10900101")]
        public void Delete(List<WL_MERCHANTEntity> DeleteData)
        {
            service.WyglService.WLDeleteMerchant(DeleteData);
        }

        [Permission("10900101")]
        public string Save(WL_MERCHANTEntity SaveData)
        {
            return service.WyglService.SaveWlMerchant(SaveData);
        }
        public UIResult SearchWlMerchant(WL_MERCHANTEntity Data)
        {
            var res = service.WyglService.GetWlMerchantElement(Data);
            return new UIResult(
                new
                {
                    merchant = res.Item1
                }
            );
        }
        [Permission("10900102")]
        public void ExecData(WL_MERCHANTEntity Data)
        {
            service.WyglService.ExecWLMerchantData(Data);
        }
    }
}