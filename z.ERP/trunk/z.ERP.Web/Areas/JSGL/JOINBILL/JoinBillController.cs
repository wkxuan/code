using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Search;


namespace z.ERP.Web.Areas.JSGL.JOINBILL
{
    public class JoinBillController : BaseController
    {
        // GET: JSGL/JoinBill
        public ActionResult JoinBillList()
        {
            ViewBag.Title = "联营结算单";
            return View(new SearchRender()
            {
                Permission_Browse = "10700600",
                Permission_Add = "10700601",
                Permission_Del = "10700601",
                Permission_Edit = "10700601",
                Permission_Exec = "10700602"
            });
        }
        public ActionResult JoinBillEdit(string Id)
        {
            ViewBag.Title = "编辑联营结算单";
            return View(model: Id);
        }
        public ActionResult JoinBillDetail(string Id)
        {
            ViewBag.Title = "浏览联营结算单";
            var entity = service.JsglService.GetJoinBillDetail(new JOIN_BILLEntity(Id));
            ViewBag.bill = entity.Item1;
            return View(entity);
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