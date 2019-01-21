using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Model;
using z.ERP.Entities.Enum;
using System.Data;
using z.MathTools;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.WLGL.WLSETTLE
{
    public class WLSETTLEController : BaseController
    {
        public ActionResult WLSETTLEList()
        {
            ViewBag.Title = "物料结算单";
            return View(new SearchRender()
            {
                Permission_Browse = "10900803",
                Permission_Add = "10900801",
                Permission_Del = "10900801",
                Permission_Edit = "10900801",
                Permission_Exec = "10900802"
            });
        }
        public ActionResult WLSETTLEMx(string Id)
        {
            ViewBag.Title = "物料结算单浏览";
            var entity = service.WyglService.GetWLSETTLEElement(new WLSETTLEEntity(Id));
            ViewBag.data = entity.Item1;
            return View();
        }
        public ActionResult WLSETTLEEdit(string Id)
        {
            ViewBag.Title = "物料结算单信息编辑";

            return View("WLSETTLEEdit", model: (EditRender)Id);

        }


        [Permission("10900801")]
        public void Delete(List<WLSETTLEEntity> DeleteData)
        {
            service.WyglService.WLDeleteWLSETTLE(DeleteData);
        }

        [Permission("10900801")]
        public string Save(WLSETTLEEntity SaveData)
        {
            return service.WyglService.SaveWLSETTLE(SaveData);
        }

        [Permission("10900802")]
        public void ExecData(WLSETTLEEntity Data)
        {
            service.WyglService.ExecWLSETTLE(Data);
        }


        public UIResult SearchWLSETTLE(WLSETTLEEntity Data)
        {
            var res = service.WyglService.GetWLSETTLEElement(Data);
            return new UIResult(
                new
                {
                    MAIN = res.Item1,
                    ITEM = res.Item2
                }
            );
        }
    }
}