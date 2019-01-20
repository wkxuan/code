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

namespace z.ERP.Web.Areas.WLGL.WLInStock
{
    public class WLInStockController : BaseController
    {
        public ActionResult WLInStockList()
        {
            ViewBag.Title = "物料购进单";
            return View(new SearchRender()
            {
                Permission_Browse = "10900303",
                Permission_Add = "10900301",
                Permission_Del = "10900301",
                Permission_Edit = "10900301",
                Permission_Exec = "10900302"
            });
        }
        public ActionResult WLInStockMx(string Id)
        {
            ViewBag.Title = "物料购进单信息浏览";
            var entity = service.WyglService.GetWlInStockElement(new WLINSTOCKEntity(Id));
            ViewBag.data = entity.Item1;
            return View();
        }
        public ActionResult WLInStockEdit(string Id)
        {
            ViewBag.Title = "物料购进单信息息编辑";

            return View("WLInStockEdit", model: (EditRender)Id);

        }


        [Permission("10900301")]
        public void Delete(List<WLINSTOCKEntity> DeleteData)
        {
            service.WyglService.WLDeleteInStock(DeleteData);
        }

        [Permission("10900301")]
        public string Save(WLINSTOCKEntity SaveData)
        {
            return service.WyglService.SaveWlInStock(SaveData);
        }

        [Permission("10900302")]
        public void ExecData(WLINSTOCKEntity Data)
        {
            service.WyglService.ExecWlInStock(Data);
        }


        public UIResult SearchWLINSTOCK(WLINSTOCKEntity Data)
        {
            var res = service.WyglService.GetWlInStockElement(Data);
            return new UIResult(
                new
                {
                    STOCK = res.Item1,
                    STOCKITEM = res.Item2
                }
            );
        }
    }
}