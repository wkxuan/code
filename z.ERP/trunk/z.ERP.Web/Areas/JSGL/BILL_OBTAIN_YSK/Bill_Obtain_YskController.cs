using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;

namespace z.ERP.Web.Areas.JSGL.BILL_OBTAIN_YSH
{
    public class Bill_Obtain_YskController : BaseController
    {
        public ActionResult Bill_Obtain_YskList()
        {
            ViewBag.Title = "预收款收取单";

            return View(new SearchRender()
            {
                Permission_Browse = "10700400",
                Permission_Add = "10700401",
                Permission_Del = "10700401",
                Permission_Edit = "10700401",
                Permission_Exec = "10700402"
            });
        }
        public ActionResult Bill_Obtain_YskEdit(string Id)
        {
            ViewBag.Title = "预收款收取";
            return View("Bill_Obtain_YskEdit", model: (EditRender)Id); 
        }
        public ActionResult Bill_Obtain_YskDetail(string Id)
        {
            ViewBag.Title = "预收款收取";
            var entity = service.JsglService.GetBillObtainElement(new BILL_OBTAINEntity(Id));
            ViewBag.billObtainYsk = entity.Item1;
            //ViewBag.billObtainYskItem = entity.Item2;
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

        //public UIResult GetBill(BILLEntity Data)
        //{
        //    return new UIResult(service.DataService.GetBill(Data));
        //}
    }
}