using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;

namespace z.ERP.Web.Areas.JSGL.JOINBILL
{
    public class JoinBillController : BaseController
    {
        // GET: JSGL/JoinBill
        public ActionResult JoinBillList()
        {
            ViewBag.Title = "联营结算单";
            return View();
        }
        public ActionResult JoinBillEdit(string Id)
        {
            ViewBag.Title = "编辑联营结算单";
            return View("JoinBillEdit", model: (EditRender)Id);
        }

        public UIResult GetJoinBillElement(JOIN_BILLEntity Data)
        {
            return new UIResult(service.JsglService.GetJoinBillElement(Data));
        }
        public UIResult ShowOneJoinDetail(JOIN_BILLEntity Data)
        {
            return new UIResult(service.JsglService.ShowOneJoinDetail(Data));
        }
    }
}