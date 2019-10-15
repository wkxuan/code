using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.JSGL.Invoice
{
    public class InvoiceController:BaseController
    {
        public ActionResult InvoiceList() {
            ViewBag.title = "发票管理";
            return View();
        }
        [Permission("10700801")]
        public ActionResult InvoiceEdit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                ViewBag.Title = "发票录入";
            }
            else {
                ViewBag.Title = "发票编辑";
            }
            return View("InvoiceEdit", model: (EditRender)Id);
        }
        [Permission("10700801")]
        public string Save(InvoiceEntity SaveData)
        {
            return service.JsglService.SaveInvoice(SaveData);
        }
        [Permission("10700801")]
        public void Delete(List<InvoiceEntity> DeleteData)
        {
            service.JsglService.DeleteInvoice(DeleteData);
        }
        public UIResult ShowOneInvoiceEdit(InvoiceEntity Data)
        {
            var res = service.JsglService.ShowOneInvoiceEdit(Data);
            return new UIResult(
                new
                {
                    Invoice = res.Item1,
                    Invoices = res.Item2
                }
            );
        }
    }
}