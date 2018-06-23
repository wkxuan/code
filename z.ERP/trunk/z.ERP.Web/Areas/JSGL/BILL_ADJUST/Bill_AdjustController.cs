﻿using z.ERP.Web.Areas.Base;
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

namespace z.ERP.Web.Areas.JSGL.BILL_ADJUST
{
    public class Bill_AdjustController : BaseController
    {
        public ActionResult Bill_AdjustList()
        {
            ViewBag.Title = "费用调整单";
            return View(new SearchRender()
            {
                Permission_Add = "10700201",
                Permission_Del = "10700201"
            });
        }
        public ActionResult Bill_AdjustEdit(string Id)
        {
            ViewBag.Title = "费用调整单";
            return View("Bill_AdjustEdit", (EditRender)Id);
        }
        public ActionResult Bill_AdjustDetail(string Id)
        {
            ViewBag.Title = "费用调整单单";
            var entity = service.JsglService.GetBillAdjustElement(new BILL_ADJUSTEntity(Id));
            ViewBag.billAdjust = entity.Item1;
            ViewBag.billAdjustItem = entity.Item2;
            return View(entity);
        }

        public void Delete(List<BILL_ADJUSTEntity> DeleteData)
        {
            service.JsglService.DeleteBillAdjust(DeleteData);
        }

        [Permission("10700201")]
        public string Save(BILL_ADJUSTEntity SaveData)
        {
            return service.JsglService.SaveBillAdjust(SaveData);
        }

        public UIResult SearchBill_Adjust(BILL_ADJUSTEntity Data)
        {
            var res = service.JsglService.GetBillAdjustElement(Data);
            return new UIResult(
                new
                {
                    billAdjust= res.Item1,
                    billAdjustItem = res.Item2
                }
                );
        }
        [Permission("10700202")]
        public void ExecData(BILL_ADJUSTEntity Data)
        {
            service.JsglService.ExecBillAdjust(Data);
        }

        //public UIResult GetBill(BILLEntity Data)
        //{
        //    return new UIResult(service.DataService.GetBill(Data));
        //}
    }
}