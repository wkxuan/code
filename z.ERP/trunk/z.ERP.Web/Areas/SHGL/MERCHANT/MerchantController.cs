using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Model;

namespace z.ERP.Web.Areas.SHGL.MERCHANT
{
    public class MerchantController : BaseController
    {
        public ActionResult MerchantList()
        {
            ViewBag.Title = "商户列表信息";
            return View();
        }

        public ActionResult Detail(string Id)
        {
            ViewBag.Title = "商户信息浏览";
            MERCHANTEntity entity = Select(new MERCHANTEntity(Id));
            return View(entity);
        }


        public ActionResult MerchantEdit(string Id)
        {
            ViewBag.Title = "商户信息编辑";
            return View(model: Id);
        }

        public void Delete(MERCHANTEntity DeleteData)
        {
            service.ShglService.DeleteMerchant(DeleteData);
        }


        public string Save(MERCHANTEntity SaveData)
        {
            return service.ShglService.SaveMerchant(SaveData);
        }

        public UIResult SearchMerchant(MERCHANTEntity Data)
        {
            return new UIResult(service.ShglService.GetMerchantElement(Data));
        }
    }
}