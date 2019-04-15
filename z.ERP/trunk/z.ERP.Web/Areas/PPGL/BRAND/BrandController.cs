using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
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
                //Permission_Browse = "10200200",
                Permission_Add = "10200201",
                // Permission_Del = "10200201",
                Permission_Edit = "10200201",
                // Permission_Exec = "10200202"
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

        public string Save(BRANDEntity SaveData)
        {
            return service.XtglService.SaveBrand(SaveData);
        }

        public void Delete(List<BRANDEntity> DeleteData)
        {
            foreach (var brand in DeleteData)
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