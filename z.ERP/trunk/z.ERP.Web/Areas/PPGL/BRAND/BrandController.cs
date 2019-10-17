using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
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

        public ActionResult BrandEdit(string Id)
        {
            ViewBag.Title = "编辑品牌列表信息";
            return View("BrandEdit", model: (EditRender)Id);
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
        public override ImportMsg ImportExcelDataHandle(DataTable dt)
        {
            return service.SpglService.SaleBillImport(dt);
        }

    }
}