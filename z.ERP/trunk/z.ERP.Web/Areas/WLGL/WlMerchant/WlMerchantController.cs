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

namespace z.ERP.Web.Areas.WLGL.WlMerchant
{
    public class WlMerchantController : BaseController
    {
        public ActionResult WlMerchantList()
        {
            ViewBag.Title = "物料供货商列表信息";
            return View(new SearchRender()
            {
                Permission_Browse = "10900103",
                Permission_Add = "10900101",
                Permission_Del = "10900101",
                Permission_Edit = "10900101",
                Permission_Exec = "10900102"
            });
        }


        public ActionResult WlMerchantMx(string Id)
        {
            ViewBag.Title = "物料供货商信息浏览";
            var entity = service.WyglService.GetWlMerchantElement(new WL_MERCHANTEntity(Id));
            ViewBag.merchant = entity.Item1;
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