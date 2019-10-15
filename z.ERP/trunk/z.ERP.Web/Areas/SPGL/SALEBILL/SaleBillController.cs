using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.SPGL.SALEBILL
{
    public class SaleBillController: BaseController
    {
        public ActionResult SaleBillList()
        {
            ViewBag.Title = "销售补录单";
            return View();
        }
        public ActionResult SaleBillEdit(string Id)
        {
            ViewBag.Title = "编辑销售补录单";
            return View("SaleBillEdit", model: (EditRender)Id);
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
        public UIResult GetPay()
        {
            return new UIResult(service.DataService.GetPay());
        }
        public UIResult SearchKind()
        {
            var res = service.SpglService.GetKindInit();
            return new UIResult(
                new
                {
                    treeorg = res.Item1
                }
            );
        }
        public override ImportMsg ImportExcelDataHandle(DataTable dt)
        {
            return service.SpglService.SaleBillImport(dt);
        }     
    }
}