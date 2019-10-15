using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CXGL.PROMOBILL_DIS
{
    public class PROMOBILL_DISController : BaseController
    {
        public ActionResult Promobill_DISList()
        {
            ViewBag.Title = "促销折扣单";
            return View("Promobill_DISList");
        }
        public ActionResult Promobill_DISEdit(string Id)
        {
            ViewBag.Title = "促销折扣单信息";
            return View("Promobill_DISEdit", model: (EditRender)Id);
        }
        public string Save(PROMOBILLEntity SaveData)
        {
            return service.CxglService.SavePromobill(SaveData);
        }
        public void Delete(List<PROMOBILLEntity> DeleteData)
        {
            service.CxglService.DeletePromobill(DeleteData);
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
        public UIResult ShowOneData(PROMOBILLEntity Data)
        {
            var res = service.CxglService.PromobillShowOneData(Data);
            return new UIResult(
                new
                {
                    mainData = res.Item1,
                    itemData = res.Item2
                }
            );
        }
    }
}