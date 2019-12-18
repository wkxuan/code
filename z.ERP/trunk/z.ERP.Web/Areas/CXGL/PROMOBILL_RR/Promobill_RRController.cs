using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CXGL.PROMOBILL_RR
{
    public class Promobill_RRController : BaseController
    {
        public ActionResult Promobill_RRList()
        {
            ViewBag.Title = "随机立减活动定义单";
            return View("Promobill_RRList");
        }
        public ActionResult Promobill_RREdit(string Id)
        {
            ViewBag.Title = "随机立减活动定义单";
            return View("Promobill_RREdit", model: (EditRender)Id);
        }
        public string Save(PROMOBILL_RREntity SaveData)
        {
            return service.CxglService.SavePromobill_RR(SaveData);
        }
        public UIResult ShowOneData(PROMOBILL_RREntity Data)
        {
            var res = service.CxglService.Promobill_RRShowOneData(Data);
            return new UIResult(
                new
                {
                    mainData = res.Item1,
                    itemData = res.Item2
                }
            );
        }
        public string ExecData(PROMOBILLEntity SaveData)
        {
            return service.CxglService.ExecPromobill(SaveData);
        }
        public string BeginData(PROMOBILLEntity SaveData)
        {
            return service.CxglService.BeginPromobill(SaveData);
        }
        public string StopData(PROMOBILLEntity SaveData)
        {
            return service.CxglService.StopPromobill(SaveData);
        }
    }
}