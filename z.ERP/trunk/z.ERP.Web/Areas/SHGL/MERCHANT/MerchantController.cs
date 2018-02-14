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

namespace z.ERP.Web.Areas.SHGL.MERCHANT
{
    public class MerchantController : BaseController
    {
        public ActionResult MerchantList()
        {
            ViewBag.Title = "商户列表信息";
            return View();
        }

        //[权限吗(111222)]
        public ActionResult 属性变更()
        {
            ViewBag.Title = "属性变更";
            ViewBag.type = 2;
            return View("MerchantList");
        }

        //[权限吗(111222)]
        public ActionResult 面积变更()
        {
            ViewBag.Title = "面积变更";
            ViewBag.type = 1;
            return View("MerchantList");
        }

        public ActionResult Detail(string Id)
        {
            ViewBag.Title = "商户信息浏览";

            var entity = service.ShglService.GetMerchantElement(new MERCHANTEntity(Id));
            // MERCHANTEntity entity = Select(new MERCHANTEntity(Id));
            ViewBag.merchant = entity.Item1;
            ViewBag.merchantBrand = entity.Item2;
            return View();
        }


        public ActionResult MerchantEdit(string Id)
        {
            ViewBag.Title = "商户信息编辑";
            return View(model: Id);
        }

        //[权限吗(111222)]
        public void Delete(List<MERCHANTEntity> DeleteData)
        {
            service.ShglService.DeleteMerchant(DeleteData);
        }

        public string Save(MERCHANTEntity SaveData)
        {
            return service.ShglService.SaveMerchant(SaveData);
        }

        public UIResult SearchMerchant(MERCHANTEntity Data)
        {
            var res = service.ShglService.GetMerchantElement(Data);
            return new UIResult(
                new
                {
                    merchant = res.Item1,
                    merchantBrand = res.Item2
                }
                );
            //return new UIResult(service.ShglService.GetMerchantElement(Data));
        }


        public UIResult GetBrand(BRANDEntity Data)
        {
            return new UIResult(service.ShglService.GetBrand(Data));
        }

        public void ExecData(MERCHANTEntity Data)
        {
            service.ShglService.ExecData(Data);
        }
    }
}