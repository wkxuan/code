using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.JSGL.BILL_OBTAIN
{
    public class Bill_ObtainController : BaseController
    {
        public ActionResult Bill_ObtainList()
        {
            ViewBag.Title = "保证金收取单";
            return View();
        }
        public ActionResult Bill_ObtainEdit(string Id)
        {
            ViewBag.Title = "保证金收取";
            return View("Bill_ObtainEdit", (EditRender)Id);
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