using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.JSGL.VOUCHER
{
    public class VoucherController : BaseController
    {
        public ActionResult VoucherList()
        {
            ViewBag.Title = "凭证模板";
            return View(new SearchRender()
            {
                Permission_Browse = "-1",
                Permission_Add = "108001",
                Permission_Del = "108001",
                Permission_Edit = "108001",
                Permission_Exec = "-1"
            });
        }
        public ActionResult VoucherEdit(string Id)
        {
            ViewBag.Title = "凭证模板编辑";
            return View("VoucherEdit", model: (EditRender)Id);
        }
        public ActionResult VoucherDetail(string Id)
        {
            ViewBag.Title = "凭证模板浏览";
            var entity = service.CwglService.GetVoucherElement(new VOUCHEREntity(Id));
            ViewBag.voucher = entity.Item1;
            ViewBag.voucherSql = entity.Item2;
            return View();  //entity
        }

        public void Delete(List<VOUCHEREntity> DeleteData)
        {
            service.CwglService.DeleteVoucher(DeleteData);
        }

        [Permission("108001")]
        public string Save(VOUCHEREntity SaveData)
        {
            return service.CwglService.SaveVoucher(SaveData);
        }

        public UIResult SearchVoucher(VOUCHEREntity Data)
        {
            var res = service.CwglService.GetVoucherElement(Data);
            return new UIResult(
                new
                {
                    voucher= res.Item1,
                    voucherSql = res.Item2,
                    voucherRecord = res.Item3,
                    voucherRecordPzkm = res.Item4,
                    voucherRecordZy = res.Item5
                }
                );
        }
        [Permission("108001")]
        public void ExecData(VOUCHEREntity Data)
        {
            service.CwglService.ExecVoucher(Data);
        }

    }
}