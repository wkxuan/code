using System;
using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.Extensions;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.HTGL.LYHT
{
    public class LYHTController : BaseController
    {
        public ActionResult HtList()
        {
            ViewBag.Title = "联营租约列表信息";
            return View();
        }
        public ActionResult HtEdit(string Id)
        {
            ViewBag.Title = "联营租约信息";
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
        public UIResult SearchInit()
        {
            SearchItem item = new SearchItem();
            var feeRule = service.XtglService.GetFeeRule(item);
            var lateFeeRule = service.XtglService.GetLateFeeRule(item);
            var operrule = service.DataService.operrule();
            var org = service.DataService.org_zslist();
            var jsfs = EnumExtension.EnumToSelectItem<结算方式>();

            return new UIResult(
                new
                {
                    FeeRule = feeRule,
                    LateFeeRule = lateFeeRule,
                    Operrule = operrule,
                    Org = org,
                    jsfs= jsfs
                }
            );
        }
        //检查合同做变更时是否已存在未启动的变更合同
        public string checkHtBgData(CONTRACTEntity Data)
        {
            return service.HtglService.checkHtBgData(Data);
        }
        public string Output(List<CONTRACTEntity> Data)
        {
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            FileHelper.DeleteDirzip();
            foreach (var item in Data)
            {
                var res = service.HtglService.GetContractOutPut(item);
                NPOIHelper.SpireDoc(time, res);
            }
            var path = NPOIHelper.FilePath(time);
            return path;
        }      
    }
}