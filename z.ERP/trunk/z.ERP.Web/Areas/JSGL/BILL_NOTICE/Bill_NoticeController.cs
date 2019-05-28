using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using System;

namespace z.ERP.Web.Areas.JSGL.BILL_NOTICE
{
    public class Bill_NoticeController : BaseController
    {
        public ActionResult Bill_NoticeList()
        {
            ViewBag.Title = "商户缴费通知单";
            return View(new SearchRender()
            {
                Permission_Browse = "10700500",
                Permission_Add = "10700501",
                Permission_Del = "10700501",
                Permission_Edit = "10700501",
                Permission_Exec = "10700502"
            });
        }
        public ActionResult Bill_NoticeEdit(string Id)
        {
            ViewBag.Title = "缴费通知单";
            return View("Bill_NoticeEdit", model: (EditRender)Id); 
        }
        public ActionResult Bill_NoticeDetail(string Id)
        {
            ViewBag.Title = "缴费通知单";
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
            //return new UIResult(service.DataService.GetBill(Data));
            return new UIResult();
        }
        public ActionResult Bill_Notice_Print(string Id)
        {
            var entity = service.JsglService.GetBillNoticePrint(new BILL_NOTICEEntity(Id));
            ViewBag.billNotice = entity.Item1;
            ViewBag.billNoticeItem = entity.Item2;
            if (entity.Item3.Rows.Count > 0)
            {
                ViewBag.MERCHANTACCOUNT = Convert.ToDecimal(entity.Item3.Rows[0]["BALANCE"]);
                //应付金额
                var a = Convert.ToDecimal(entity.Item1.NOTICE_MONEY);
                var b = Convert.ToDecimal(entity.Item3.Rows[0]["BALANCE"].ToString());
                if (a > b)
                {
                    ViewBag.payable = a - b;
                }
                else
                {
                    ViewBag.payable = 0;
                }
            }
            else
            {
                ViewBag.MERCHANTACCOUNT = 0;
                ViewBag.payable = Convert.ToDecimal(ViewBag.billNotice.NOTICE_MONEY);
            }
            ViewBag.CurrentDate = System.DateTime.Now;
            return View();
        }

        public ActionResult Bill_Notice_PrintOther(string Id)
        {
            var entity = service.JsglService.GetBillNoticePrint(new BILL_NOTICEEntity(Id));
            ViewBag.billNotice = entity.Item1;
            ViewBag.billNoticeItem = entity.Item2;
            if (entity.Item3.Rows.Count > 0)
            {
                ViewBag.MERCHANTACCOUNT = Convert.ToDecimal(entity.Item3.Rows[0]["BALANCE"]);
                //应付金额
                var a = Convert.ToDecimal(entity.Item1.NOTICE_MONEY);
                var b = Convert.ToDecimal(entity.Item3.Rows[0]["BALANCE"].ToString());
                if (a > b)
                {
                    ViewBag.payable = a - b;
                }
                else
                {
                    ViewBag.payable = 0;
                }
            }
            else
            {
                ViewBag.MERCHANTACCOUNT = 0;
                ViewBag.payable = Convert.ToDecimal(ViewBag.billNotice.NOTICE_MONEY);
            }            
            ViewBag.CurrentDate = System.DateTime.Now;
            return View();
        }
    }
}