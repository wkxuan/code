using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.PPGL.BRAND
{
    public class BrandController : BaseController
    {

        public ActionResult BrandList()
        {
            ViewBag.Title = "品牌列表信息";
            return View();
        }

        public ActionResult BrandEdit()
        {
            ViewBag.Title = "编辑品牌列表信息";
            return View();
        }

        public string Save(BRANDEntity SaveData) {
            return service.XtglService.SaveBrand(SaveData);            
        }

        public  void Delete(List<BRANDEntity> DeleteData)
        {
            foreach(var brand in DeleteData)
            {
                var v = GetVerify(brand);
                v.Require(a => a.ID);
                v.Verify();
                CommenDelete(brand);
            }

        }
    }
}