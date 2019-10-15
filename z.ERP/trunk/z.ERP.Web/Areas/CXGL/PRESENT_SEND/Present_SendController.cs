using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CXGL.PRESENT_SEND
{
    public class Present_SendController:BaseController
    {
        public ActionResult Present_SendList()
        {
            ViewBag.Title = "赠品发放单";
            return View();
        }
        public ActionResult Present_SendEdit(string Id)
        {
            ViewBag.Title = "赠品发放单";
            return View("Present_SendEdit", model: (EditRender)Id);
        }
        public UIResult GetSaleTicket(string BRANCHID,string POSNO,string DEALID) {
            var data = service.CxglService.GetSaleTicket(BRANCHID, POSNO, DEALID);
            return new UIResult(new {
                ticketinfo = data.Item1,
                Status = data.Item2,
            });
        }
        public UIResult GetPresentList(List<PROMOBILL_FG_RULEEntity> Data) {
            return new UIResult(service.CxglService.GetPresentList(Data));
        }
        public string Save(PRESENT_SENDEntity SaveData)
        {
            return service.CxglService.SavePresent_Send(SaveData);
        }
        public void Delete(List<PRESENT_SENDEntity> DeleteData)
        {
            service.CxglService.DeletePresent_Send(DeleteData);
        }
        public string ExecData(PRESENT_SENDEntity Data)
        {
            return service.CxglService.ExecPresent_Send(Data);
        }
        public UIResult ShowOneData(PRESENT_SENDEntity Data)
        {
            var res = service.CxglService.Present_SendShowOneData(Data);
            return new UIResult(
                new
                {
                    mainData = res.Item1,
                    ticketData= res.Item2,
                    itemData = res.Item3
                }
            );
        }
    }
}