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
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.JSGL.BILL_OBTAIN
{
    public class Bill_ObtainController : BaseController
    {
        public ActionResult Bill_ObtainList()
        {
            ViewBag.Title = "保证金收取";
            return View(new SearchRender()
            {
                Permission_Add = "10700201",
                Permission_Del = "10700201"
            });
        }
        public ActionResult Bill_ObtainEdit(string Id)
        {
            ViewBag.Title = "保证金收取";
            return View("Bill_ObtainEdit", (EditRender)Id);
        }
        public ActionResult Bill_ObtainDetail(string Id)
        {
            ViewBag.Title = "保证金收取";
            var entity = service.JsglService.GetBillObtainElement(new BILL_OBTAINEntity(Id));
            ViewBag.billObtain = entity.Item1;
            ViewBag.billObtainItem = entity.Item2;
            return View(entity);
        }

        public void Delete(List<BILL_OBTAINEntity> DeleteData)
        {
            service.JsglService.DeleteBillObtain(DeleteData);
        }

        [Permission("10700301")]
        public string Save(BILL_OBTAINEntity SaveData)
        {
            return service.JsglService.SaveBillObtain(SaveData);
        }

        public UIResult SearchBill_Obtain(BILL_OBTAINEntity Data)
        {
            var res = service.JsglService.GetBillObtainElement(Data);
            return new UIResult(
                new
                {
                    billObtain = res.Item1,
                    billObtainItem = res.Item2
                }
                );
        }
        [Permission("10700302")]
        public void ExecData(BILL_OBTAINEntity Data)
        {
            service.JsglService.ExecBillObtain(Data);
        }

        //public UIResult GetBill(BILLEntity Data)
        //{
        //    return new UIResult(service.DataService.GetBill(Data));
        //}
    }
}