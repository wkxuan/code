using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WLGL.WLInStock
{
    public class WLInStockController : BaseController
    {
        public ActionResult WLInStockList()
        {
            ViewBag.Title = "物料购进单";
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