using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.Extensions;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;

namespace z.ERP.Web.Areas.PPGL.BRAND
{
    public class BrandController : BaseController
    {

        public ActionResult BrandList()
        {
            ViewBag.Title = "品牌列表信息";
            return View(new SearchRender()
            {
                Permission_Add = "102002",
                Permission_Del = "102002",
                Permission_Edit = "102002",
                Permission_Exec = "102002"
            });
        }

        public ActionResult BrandEdit(string Id)
        {
            ViewBag.Title = "编辑品牌列表信息";
            return View("BrandEdit", model: (EditRender)Id);
        }

        public ActionResult BrandDetail(string Id)
        {
            ViewBag.Title = "浏览品牌列表信息";

            var entity = service.XtglService.GetBrandDetail(new BRANDEntity(Id));            
            ViewBag.brand = entity.Item1;
            return View();
        }

        [Permission("102002")]

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
        public UIResult SearchElement(BRANDEntity Data)
        {
            return new UIResult(service.XtglService.GetBrandElement(Data));
        }
        [Permission("102002")]
        public void ExecData(BRANDEntity Data)
        {
            service.XtglService.BrandExecData(Data);
        }

        public UIResult SearchInit()
        {
            var res = service.DataService.GetTreeCategory();
            return new UIResult(
                new
                {
                    treeOrg = res.Item1
                }
            );
        }

    }
}