using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.SPGL.SALEBILL
{
    public class SaleBillController: BaseController
    {
        public ActionResult SaleBillList()
        {
            ViewBag.Title = "销售补录单";
            return View(new SearchRender()
            {
                Permission_Browse = "10500400",
                Permission_Add = "10500401",
                Permission_Del = "10500401",
                Permission_Edit = "10500401",
                Permission_Exec = "10500402"
            });
        }
        public ActionResult SaleBillEdit(string Id)
        {
            ViewBag.Title = "编辑销售补录单";
            return View("SaleBillEdit", model: (EditRender)Id);
        }
        public ActionResult SaleBillDetail(string Id)
        {
            ViewBag.Title = "浏览销售补录单";
            var entity = service.SpglService.GetSaleBillDetail(new SALEBILLEntity(Id));
            ViewBag.salebill = entity.Item1;
            ViewBag.salebillitem = entity.Item2;
            return View(entity);            
        }
        [Permission("10500401")]
        public string Save(SALEBILLEntity SaveData)
        {
            return service.SpglService.SaveSaleBill(SaveData);            
        }

        public void Delete(List<SALEBILLEntity> DeleteData)
        {
            service.SpglService.DeleteSaleBill(DeleteData);
        }

        public UIResult ShowOneSaleBillEdit(SALEBILLEntity Data)
        {
            return new UIResult(service.SpglService.ShowOneSaleBillEdit(Data));
        }
        [Permission("10500402")]
        public void ExecData(SALEBILLEntity Data)
        {
            service.SpglService.ExecSaleBillData(Data);
        }
        public UIResult GetPay(PAYEntity Data)
        {
            return new UIResult(service.DataService.GetPay(Data));
        }
    }
}