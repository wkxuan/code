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

namespace z.ERP.Web.Areas.WLGL.WLOutStock
{
    public class WLOutStockController : BaseController
    {
        public ActionResult WLOutStockList()
        {
            ViewBag.Title = "物料购进单冲红";
            return View(new SearchRender()
            {
                Permission_Browse = "10900403",
                Permission_Add = "10900401",
                Permission_Del = "10900401",
                Permission_Edit = "10900401",
                Permission_Exec = "10900402"
            });
        }
        public ActionResult WLOutStockMx(string Id)
        {
            ViewBag.Title = "物料购进单冲红信息浏览";
            var entity = service.WyglService.GetWlOutStockElement(new WLOUTSTOCKEntity(Id));
            ViewBag.data = entity.Item1;
            return View();
        }
        public ActionResult WLOutStockEdit(string Id)
        {
            ViewBag.Title = "物料购进单冲红信息编辑";

            return View("WLOutStockEdit", model: (EditRender)Id);

        }


        [Permission("10900301")]
        public void Delete(List<WLOUTSTOCKEntity> DeleteData)
        {
            service.WyglService.WLDeleteOutStock(DeleteData);
        }

        [Permission("10900401")]
        public string Save(WLOUTSTOCKEntity SaveData)
        {
            return service.WyglService.SaveWlOutStock(SaveData);
        }

        [Permission("10900402")]
        public void ExecData(WLOUTSTOCKEntity Data)
        {
            service.WyglService.ExecWlOutStock(Data);
        }


        public UIResult SearchWLOUTSTOCK(WLOUTSTOCKEntity Data)
        {
            var res = service.WyglService.GetWlOutStockElement(Data);
            return new UIResult(
                new
                {
                    OUTSTOCK = res.Item1,
                    OUTSTOCKITEM = res.Item2
                }
            );
        }
    }
}