using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WYGL.MARCHINAREAR
{
    public class MarchinArearController: BaseController
    {
        public ActionResult MarchinArearList()
        {
            ViewBag.Title = "商户进场管理";
            return View();
        }
        public ActionResult MarchinArearEdit(string Id)
        {
            ViewBag.Title = "编辑商户进场管理";
            return View("MarchinArearEdit", model: (EditRender)Id);
        }
        public string Save(MARCHINAREAREntity SaveData)
        {
            return service.WyglService.SaveMarchInArear(SaveData);
        }

        public void Delete(List<MARCHINAREAREntity> DeleteData)
        {
            service.WyglService.DeleteMarchInArear(DeleteData);
        }
        public void ExecData(MARCHINAREAREntity Data)
        {
            service.WyglService.ExecMarchInArearData(Data);
        }

        [Permission("1")]
        public UIResult SearchElement(MARCHINAREAREntity Data)
        {
            return new UIResult(service.WyglService.GetMarchInArearElement(Data));
        }
        public UIResult GetContract(CONTRACTEntity Data)
        {
            return new UIResult(service.WyglService.GetContract(Data));
        }
        public UIResult ShowOneMarchinArearEdit(MARCHINAREAREntity Data)
        {
            return new UIResult(service.WyglService.GetMarchInArearElement(Data));
        }
    }
}