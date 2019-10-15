using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.JSGL.BILL_OBTAIN_YSH
{
    public class Bill_Obtain_YskController : BaseController
    {
        public ActionResult Bill_Obtain_YskList()
        {
            ViewBag.Title = "预收款收取单";

            return View();
        }
        public ActionResult Bill_Obtain_YskEdit(string Id)
        {
            ViewBag.Title = "预收款收取";
            return View("Bill_Obtain_YskEdit", model: (EditRender)Id); 
        }

        public void Delete(List<BILL_OBTAINEntity> DeleteData)
        {
            service.JsglService.DeleteBillObtain(DeleteData);
        }
        public string Save(BILL_OBTAINEntity SaveData)
        {
            return service.JsglService.SaveBillObtain(SaveData);
        }

        public UIResult SearchBill_Obtain_Ysk(BILL_OBTAINEntity Data)
        {
            var res = service.JsglService.GetBillObtainElement(Data);
            return new UIResult(
                new
                {
                    billObtainYsk = res.Item1,
                    //billObtainYskItem = res.Item2
                }
                );
        }
        public void ExecData(BILL_OBTAINEntity Data)
        {
            service.JsglService.ExecBillObtain(Data);
        }

        public UIResult GETfee(FEE_ACCOUNTEntity Data)
        {
            return new UIResult(service.DataService.feeAccount(Data));
        }
    }
}