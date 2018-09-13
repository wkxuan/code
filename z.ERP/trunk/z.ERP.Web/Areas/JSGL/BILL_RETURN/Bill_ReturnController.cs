using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;

namespace z.ERP.Web.Areas.JSGL.BILL_RETURN
{
    public class Bill_ReturnController : BaseController
    {
        public ActionResult Bill_ReturnList()
        {
            ViewBag.Title = "保证金返还单";

            return View(new SearchRender()
            {
                Permission_Browse = "10700100",
                Permission_Add = "10700101",
                Permission_Del = "10700101",
                Permission_Edit = "10700101",
                Permission_Exec = "10700102"
            });
        }
        public ActionResult Bill_ReturnEdit(string Id)
        {
            ViewBag.Title = "保证金返还单";
            return View("Bill_ReturnEdit",model: (EditRender)Id); 
        }
        public ActionResult Bill_ReturnDetail(string Id)
        {
            ViewBag.Title = "保证金返还单";
            var entity = service.JsglService.GetBillReturnElement(new BILL_RETURNEntity(Id));
            ViewBag.billReturn = entity.Item1;
            ViewBag.billReturnItem = entity.Item2;
            return View(entity);
        }

        public void Delete(List<BILL_RETURNEntity> DeleteData)
        {
            service.JsglService.DeleteBillReturn(DeleteData);
        }


        public string Save(BILL_RETURNEntity SaveData)
        {
            return service.JsglService.SaveBillReturn(SaveData);
        }

        public UIResult SearchBill_Return(BILL_RETURNEntity Data)
        {
            var res = service.JsglService.GetBillReturnElement(Data);
            return new UIResult(
                new
                {
                    billReturn = res.Item1,
                    billReturnItem = res.Item2
                }
                );
        }
        public void ExecData(BILL_RETURNEntity Data)
        {
            service.JsglService.ExecBillReturn(Data);
        }

        //public UIResult GetBill(BILLEntity Data)
        //{
        //    return new UIResult(service.DataService.GetBill(Data));
        //}
    }
}