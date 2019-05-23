using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.SPGL.ADJUSTDISCOUNT
{
    public class AdjustDiscountController:BaseController
    {
        public ActionResult AdjustDiscountList()
        {
            ViewBag.Title = "扣率调整单";
            return View(new SearchRender()
            {
                Permission_Browse = "10500700",
                Permission_Add = "10500701",
                Permission_Del = "10500701",
                Permission_Edit = "10500701",
                Permission_Exec = " "
            });
        }
            public ActionResult AdjustDiscountEdit(string Id)
        {
            ViewBag.Title = "编辑扣率调整单";
            return View("AdjustDiscountEdit", model: (EditRender)Id);
        }
        public UIResult ShowOneAdjustDiscountEdit(ADJUSTDISCOUNTEntity Data)
        {
            return new UIResult(service.SpglService.ShowOneAdjustDiscountEdit(Data));
        }
        public string Save(ADJUSTDISCOUNTEntity SaveData)
        {
            return service.SpglService.SaveAdjustDiscount(SaveData);
        }
        public void Delete(List<ADJUSTDISCOUNTEntity> DeleteData)
        {
            service.SpglService.DeleteAdjustDiscount(DeleteData);
        }
    }
    
}