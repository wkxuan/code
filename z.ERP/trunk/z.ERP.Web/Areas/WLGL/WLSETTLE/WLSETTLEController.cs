using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WLGL.WLSETTLE
{
    public class WLSETTLEController : BaseController
    {
        public ActionResult WLSETTLEList()
        {
            ViewBag.Title = "物料结算单";
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