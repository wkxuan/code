using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.JSGL.VOUCHER
{
    public class VoucherController : BaseController
    {
        public ActionResult VoucherList()
        {
            ViewBag.Title = "凭证模板";
            return View();
        }
        public ActionResult VoucherEdit(string Id)
        {
            ViewBag.Title = "凭证模板编辑";
            return View("VoucherEdit", model: (EditRender)Id);
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
                    voucher = res.Item1,
                    voucherSql = res.Item2,
                    voucherRecord = res.Item3,
                    voucherRecordPzkm = res.Item4,
                    voucherRecordZy = res.Item5
                }
                );
        }
        [Permission("-108001")]
        public void ExecData(VOUCHEREntity Data)
        {
            service.CwglService.ExecVoucher(Data);
        }

    }
}