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

namespace z.ERP.Web.Areas.JSGL.BILL_NOTICE
{
    public class Bill_NoticeController : BaseController
    {
        public ActionResult Bill_NoticeList()
        {
            ViewBag.Title = "租赁结算单";
            return View();
        }
        public ActionResult Bill_NoticeEdit(string Id)
        {
            ViewBag.Title = "租赁结算单";
            return View("Bill_NoticeEdit", model: Id); 
        }
        public ActionResult Bill_NoticeDetail(string Id)
        {
            ViewBag.Title = "租赁结算单";
            var entity = service.JsglService.GetBillNoticeElement(new BILL_NOTICEEntity(Id));
            ViewBag.billNotice = entity.Item1;
            ViewBag.billNoticeItem = entity.Item2;
            return View(entity);
        }

        public void Delete(List<BILL_NOTICEEntity> DeleteData)
        {
            service.JsglService.DeleteBillNotice(DeleteData);
        }


        public string Save(BILL_NOTICEEntity SaveData)
        {
            return service.JsglService.SaveBillNotice(SaveData);
        }

        public UIResult SearchBill_Notice(BILL_NOTICEEntity Data)
        {
            var res = service.JsglService.GetBillNoticeElement(Data);
            return new UIResult(
                new
                {
                    billNotice = res.Item1,
                    billNoticeItem = res.Item2
                }
                );
        }
        public void ExecData(BILL_NOTICEEntity Data)
        {
            service.JsglService.ExecBillNotice(Data);
        }

        public UIResult GetBill(BILLEntity Data)
        {
            return new UIResult(service.DataService.GetBill(Data));
        }
    }
}