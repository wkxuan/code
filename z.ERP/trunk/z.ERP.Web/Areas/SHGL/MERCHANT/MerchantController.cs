using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;

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

        public ActionResult MerchantEdit()
        {
            ViewBag.Title = "商户信息编辑";
            return View();
        }

        public void Delete(List<MERCHANTEntity> DeleteData)
        {
            service.ShglService.DeleteMerchant(DeleteData);
        }


        public string Save(MERCHANTEntity SaveData)
        {
            return service.ShglService.SaveMerchant(SaveData);
        }
    }
}