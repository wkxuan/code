using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.JSGL.BILL_OBTAIN_SK
{
    public class Bill_Obtain_SkController: BaseController
    {
        public ActionResult Bill_Obtain_SkList()
        {
            ViewBag.Title = "商户收款处理";
            return View();
        }
        public ActionResult Bill_Obtain_SkEdit(string Id)
        {
            ViewBag.Title = "编辑商户收款处理";
            return View(model: Id);
        }
        public ActionResult Bill_Obtain_SkDetail(string Id)
        {
            ViewBag.Title = "浏览商户收款处理";
            //var entity = service.JsglService.GetBillObtainSklDetail(new BILL_OBTAINEntity(Id));
            //ViewBag.bill = entity.Item1;
            //return View(entity);
            return View();
        }
    }
}