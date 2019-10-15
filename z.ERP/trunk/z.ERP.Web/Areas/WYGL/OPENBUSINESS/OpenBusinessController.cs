using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WYGL.OpenBusiness
{
    public class OpenBusinessController : BaseController
    {
        public ActionResult OpenBusinessList()
        {
            ViewBag.Title = "店铺开业单";
            return View();
        }
        public ActionResult OpenBusinessEdit(string Id)
        {
            ViewBag.Title = "店铺开业单";
            return View("OpenBusinessEdit", model: (EditRender)Id);
        }
        public string Save(OPENBUSINESSEntity SaveData)
        {
            return service.WyglService.SaveOpenBusiness(SaveData);
        }

        public void Delete(List<OPENBUSINESSEntity> DeleteData)
        {
            service.WyglService.DeleteOpenBusiness(DeleteData);
        }
        public void ExecData(OPENBUSINESSEntity Data)
        {
            service.WyglService.ExecOpenBusinessData(Data);
        }

        [Permission("1")]
        public UIResult SearchElement(OPENBUSINESSEntity Data)
        {
            return new UIResult(service.WyglService.GetOpenBusinessElement(Data));
        }
        public UIResult GetContract(CONTRACTEntity Data)
        {
            return new UIResult(service.WyglService.GetContract(Data));
        }
        public UIResult ShowOneOpenBusinessEdit(OPENBUSINESSEntity Data)
        {
            return new UIResult(service.WyglService.GetOpenBusinessElement(Data));
        }
    }
}