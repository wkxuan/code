using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.SPGL.GOODS
{
    public class GoodsController:BaseController
    {
        public ActionResult GoodsList()
        {
            ViewBag.Title = "商品信息";
            return View();
        }
        public ActionResult GoodsEdit(string Id)
        {
            ViewBag.Title = "编辑商品信息";
            return View("GoodsEdit", model: (EditRender)Id);
        }
        [Permission("10500201")]
        public string Save(GOODSEntity SaveData)
        {
            return service.SpglService.SaveGoods(SaveData);
        }

        public void Delete(List<GOODSEntity> DeleteData)
        {
            service.SpglService.DeleteGoods(DeleteData);
        }

        public UIResult ShowOneEdit(GOODSEntity Data)
        {
            var res = service.SpglService.ShowOneEdit(Data);
            return new UIResult( new { goods = res.Item1, goods_shop = res.Item2, goods_group = res.Item3 } );
        }

        public UIResult GetContract(CONTRACTEntity Data)
        {
            return new UIResult(service.SpglService.GetContract(Data));
        }

        [Permission("10500202")]
        public void ExecData(GOODSEntity Data)
        {
            service.SpglService.ExecData(Data);
        }
        public UIResult SearchInit()
        {
            var res = service.SpglService.GetKindInit();
            return new UIResult(
                new
                {
                    treeorg = res.Item1
                }
            );
        }
        public UIResult GetKLZinfo(CONTJSKLEntity Data)
        {
            return new UIResult(service.SpglService.GetKLZinfo(Data));
        }
    }
}