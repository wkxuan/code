using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WYGL.OpenBusiness
{
    public class OpenBusinessController : BaseController
    {
        public ActionResult OpenBusinessList()
        {
            ViewBag.Title = "店铺开业单";
            return View(new SearchRender()
            {
                Permission_Browse = "10300600",
                Permission_Add = "10300601",
                Permission_Del = "10300601",
                Permission_Edit = "10300601",
                Permission_Exec = "10300602",
            });
        }
        public ActionResult OpenBusinessEdit(string Id)
        {
            ViewBag.Title = "店铺开业单";
            return View("OpenBusinessEdit", model: (EditRender)Id);
        }

        public ActionResult OpenBusinessDetail(string Id)
        {
            ViewBag.Title = "浏览店铺开业单";
            var entity = service.WyglService.GetOpenBusinessDetail(new OPENBUSINESSEntity(Id));
            ViewBag.regist = entity.Item1;
            ViewBag.item = entity.Item2;
            return View(entity);
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