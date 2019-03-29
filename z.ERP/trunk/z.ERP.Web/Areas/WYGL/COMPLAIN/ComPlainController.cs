using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WYGL.COMPLAIN
{
    public class ComPlainController:BaseController
    {
        public ActionResult ComPlainList()
        {
            ViewBag.Title = "投诉管理";
            return View(new SearchRender()
            {
                Permission_Browse = "10300700",
                Permission_Add = "10300701",
                Permission_Del = "10300701",
                Permission_Edit = "10300701",
                Permission_Exec = "10300702",
            });
        }
        public ActionResult ComPlainEdit(string Id)
        {
            ViewBag.Title = "编辑投诉单";
            return View("ComPlainEdit", model: (EditRender)Id);
        }

        public ActionResult ComPlainDetail(string Id)
        {
            ViewBag.Title = "浏览投诉单";
            var entity = service.WyglService.GetComPlainDetail(new COMPLAINEntity(Id));
            ViewBag.regist = entity.Item1;
            return View(entity);
        }
        public string Save(COMPLAINEntity SaveData)
        {
            return service.WyglService.SaveComPlain(SaveData);
        }

        public void Delete(List<COMPLAINEntity> DeleteData)
        {
            service.WyglService.DeleteComPlain(DeleteData);
        }

        public void ExecData(COMPLAINEntity Data)
        {
            service.WyglService.ExecComPlainData(Data);
        }

        [Permission("1")]
        public UIResult SearchElement(COMPLAINEntity Data)
        {
            return new UIResult(service.WyglService.GetComPlainElement(Data));
        }
        public UIResult ShowOneComPlainEdit(COMPLAINEntity Data)
        {
            return new UIResult(service.WyglService.GetComPlainElement(Data));
        }
    }
}