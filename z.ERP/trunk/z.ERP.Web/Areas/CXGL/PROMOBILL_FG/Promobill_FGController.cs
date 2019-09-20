using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CXGL.PROMOBILL_FG
{
    public class Promobill_FGController: BaseController
    {
        public ActionResult Promobill_FGList()
        {
            ViewBag.Title = "促销赠品单";
            return View();
        }
        public ActionResult Promobill_FGEdit(string Id)
        {
            ViewBag.Title = "促销赠品单";
            return View("Promobill_FGEdit", model: (EditRender)Id);
        }
        public string Save(PROMOBILLEntity SaveData)
        {
            return service.CxglService.SavePromobill_FG(SaveData);
        }
        public void Delete(List<PROMOBILLEntity> DeleteData)
        {
            service.CxglService.DeletePromobill_FG(DeleteData);
        }
        public UIResult ShowOneData(PROMOBILLEntity Data)
        {
            var res = service.CxglService.Promobill_FGShowOneData(Data);
            return new UIResult(
                new
                {
                    mainData = res.Item1,
                    itemData = res.Item2
                }
            );
        }
        public string ExecData(PROMOBILLEntity Data)
        {
            return service.CxglService.ExecPromobill(Data);
        }
        public string BeginData(PROMOBILLEntity Data)
        {
            return service.CxglService.BeginPromobill(Data);
        }
        public string StopData(PROMOBILLEntity Data)
        {
            return service.CxglService.StopPromobill(Data);
        }
    }
}