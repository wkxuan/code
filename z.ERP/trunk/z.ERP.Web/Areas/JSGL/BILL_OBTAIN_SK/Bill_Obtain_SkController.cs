using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Results;
using System;
using z.ERP.Entities.Auto;

namespace z.ERP.Web.Areas.JSGL.BILL_OBTAIN_SK
{
    public class Bill_Obtain_SkController : BaseController
    {
        public ActionResult Bill_Obtain_SkList()
        {
            ViewBag.Title = "租赁核销单";
            return View();
        }
        public ActionResult Bill_Obtain_SkEdit(string Id)
        {
            ViewBag.Title = "租赁核销单";
            return View("Bill_Obtain_SkEdit", (EditRender)Id);
        }

        public void Delete(List<BILL_OBTAINEntity> DeleteData)
        {
            service.JsglService.DeleteBillObtain(DeleteData);
        }

        public string Save(BILL_OBTAINEntity SaveData)
        {
            return service.JsglService.SaveBillObtain(SaveData);
        }
        public UIResult GETfee(FEE_ACCOUNTEntity Data)
        {
            return new UIResult(service.DataService.feeAccount(Data));
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
        public UIResult SearchBalance(MERCHANT_ACCOUNTEntity Data)
        {
            return new UIResult(service.DataService.GetBalance(Data));
        }
    }
}