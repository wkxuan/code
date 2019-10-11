using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.HTGL.LYHT
{
    public class LYHTController : BaseController
    {
        public ActionResult HtList()
        {
            ViewBag.Title = "租约列表信息";
            return View(new SearchRender()
            {
                Permission_Browse = "10600100",
                Permission_Add = "10600101",
                Permission_Del = "10600101",
                Permission_Edit = "10600101",
                Permission_Exec = "10600102",
                Permission_Bg  = "10600103"
            });
        }
        public ActionResult HtEdit(string Id)
        {
            ViewBag.Title = "联营租约信息编辑";
            return View("HtEdit", (EditRender)Id);
        }
        [Permission("10600101")]
        public string Save(CONTRACTEntity SaveData)
        {
            return service.HtglService.SaveContract(SaveData);
        }
        public void Delete(List<CONTRACTEntity> DeleteData)
        {
            service.HtglService.DeleteContract(DeleteData);
        }
        [Permission("10600102")]
        public void ExecData(CONTRACTEntity Data)
        {
            service.HtglService.ExecData(Data);
        }
        public UIResult SearchContract(CONTRACTEntity Data)
        {
            var res = service.HtglService.GetContractElement(Data);
            var mid = res.Item1.MERCHANTID.Value;
            var paymentid = res.Item1.PAYMENTID.Value + "";
            var mpayment = service.ShglService.GetMerchantPayment(mid, paymentid);
            return new UIResult(
                new
                {
                    contract = res.Item1,
                    contractBrand = res.Item2,
                    contractShop = res.Item3,
                    ContractParm = res.Item4,
                    ContractRentParm = res.Item5,
                    contractPay = res.Item6,
                    contractCost = res.Item7,
                    contractPayment = mpayment
                }
            );
        }
        public UIResult GetBrand(BRANDEntity Data)
        {
            return new UIResult(service.DataService.GetBrand(Data));
        }
        public UIResult GetShop(SHOPEntity Data)
        {
            return new UIResult(service.DataService.GetShop(Data));
        }
        public UIResult GetFeeSubject(FEESUBJECTEntity Data)
        {
            return new UIResult(service.DataService.GetFeeSubject(Data));
        }
        public UIResult lyYdFj(List<CONTRACT_RENTEntity> Data, CONTRACTEntity ContractData)
        {
            return new UIResult(service.HtglService.LyYdfj(Data, ContractData));
        }
    }
}