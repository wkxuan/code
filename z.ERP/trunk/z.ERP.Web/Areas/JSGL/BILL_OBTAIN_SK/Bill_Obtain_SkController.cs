using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Results;
using System;

namespace z.ERP.Web.Areas.JSGL.BILL_OBTAIN_SK
{
    public class Bill_Obtain_SkController: BaseController
    {
        public ActionResult Bill_Obtain_SkList()
        {
            ViewBag.Title = "租赁核销单";
            return View(new SearchRender()
            {
                Permission_Browse = "10700700",
                Permission_Add = "10700701",
                Permission_Del = "10700701",
                Permission_Edit = "10700701",
                Permission_Exec = "10700702"
            });
        }
        public ActionResult Bill_Obtain_SkEdit(string Id)
        {
            ViewBag.Title = "编辑租赁核销单";            
            return View("Bill_Obtain_SkEdit", (EditRender)Id);
        }
        public ActionResult Bill_Obtain_SkDetail(string Id)
        {
            ViewBag.Title = "浏览租赁核销单";
            var entity = service.JsglService.GetBillObtainElement(new BILL_OBTAINEntity(Id));
            ViewBag.billObtain = entity.Item1;
            ViewBag.billObtainItem = entity.Item2;
            ViewBag.billObtainInvoice = entity.Item3;
            return View(entity);
        }

        public void Delete(List<BILL_OBTAINEntity> DeleteData)
        {
            service.JsglService.DeleteBillObtain(DeleteData);
        }

        public string Save(BILL_OBTAINEntity SaveData)
        {
            return service.JsglService.SaveBillObtain(SaveData);
        }

        public void ExecData(BILL_OBTAINEntity Data)
        {
            service.JsglService.ExecBillObtain(Data);
        }
        public UIResult SearchBill_Obtain(BILL_OBTAINEntity Data)
        {
            var res = service.JsglService.GetBillObtainElement(Data);
            return new UIResult(
                new
                {
                    billObtain = res.Item1,
                    billObtainItem = res.Item2,
                    billObtainInvoice = res.Item3,
        }
                );
        }
        public ActionResult Bill_Obtain_SkPrint(string Id)
        {
              var entity = service.JsglService.GetBillObtainPrint(new BILL_OBTAINEntity(Id));
            ViewBag.billObtain = entity.Item1;
            ViewBag.billObtainItem = entity.Item2;
            ViewBag.CurrentDate = System.DateTime.Now;
            return View();
        }
    }
}