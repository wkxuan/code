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

namespace z.ERP.Web.Areas.WYGL.MARCHINAREAR
{
    public class MarchinArearController: BaseController
    {
        public ActionResult MarchinArearList()
        {
            ViewBag.Title = "商户进场管理";
            return View(new SearchRender()
            {
                Permission_Browse = "10300500",
                Permission_Add = "10300501",
                Permission_Del = "10300501",
                Permission_Edit = "10300501",
                Permission_Exec = "10300502",
            });
        }
        public ActionResult MarchinArearEdit(string Id)
        {
            ViewBag.Title = "编辑商户进场管理";
            return View("MarchinArearEdit", model: (EditRender)Id);
        }

        public ActionResult MarchinArearDetail(string Id)
        {
            ViewBag.Title = "浏览商户进场管理";
            var entity = service.WyglService.GetMarchinArearDetail(new MARCHINAREAREntity(Id));
            ViewBag.regist = entity.Item1;
            ViewBag.item = entity.Item2;
            return View(entity);
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
    }
}