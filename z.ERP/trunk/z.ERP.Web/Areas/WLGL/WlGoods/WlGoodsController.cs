using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WLGL.WlGoods
{
    public class WlGoodsController : BaseController
    {
        public ActionResult WlGoodsList()
        {
            ViewBag.Title = "物料列表信息";
            return View();
        }
        public ActionResult WlGoodsEdit(string Id)
        {
            ViewBag.Title = "物料信息编辑";

            return View("WlGoodsEdit", model: (EditRender)Id);
        }
        [Permission("10900201")]
        public void Delete(List<WL_GOODSEntity> DeleteData)
        {
            service.WyglService.WLDeleteGoods(DeleteData);
        }

        [Permission("10900201")]
        public string Save(WL_GOODSEntity SaveData)
        {
            return service.WyglService.SaveWlGoods(SaveData);
        }
        public UIResult SearchWlGoods(WL_GOODSEntity Data)
        {
            var res = service.WyglService.GetWlGoodsElement(Data);
            return new UIResult(
                new
                {
                    goods = res.Item1
                }
            );
        }
        [Permission("10900202")]
        public void ExecData(WL_GOODSEntity Data)
        {
            service.WyglService.ExecWLGoodsData(Data);
        }
    }
}