﻿using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using System.Collections.Generic;
using z.MVC5.Results;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.Search;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Entities.Enum;

namespace z.ERP.Web.Areas.HTGL.ZLHT
{
    public class ZLHTController : BaseController
    {
        public ActionResult HtList()
        {
            ViewBag.Title = "租约列表信息";
            return View(new SearchRender()
            {
                Permission_Browse = "10600200",
                Permission_Add = "10600201",
                Permission_Edit = "10600201",
                Permission_Del = "10600201",
                Permission_Exec = "10600202",
                Permission_Bg = "10600203"
            });
        }
        public ActionResult HtEdit(string Id)
        {
            ViewBag.Title = "租赁租约信息";
            return View("HtEdit", model: (EditRender)Id);
        }
        [Permission("10600201")]
        public string Save(CONTRACTEntity SaveData)
        {
            return service.HtglService.SaveContract(SaveData);
        }
        public void Delete(List<CONTRACTEntity> DeleteData)
        {
            service.HtglService.DeleteContract(DeleteData);
        }
        [Permission("10600202")]
        public string ExecData(CONTRACTEntity Data)
        {
            return service.HtglService.ExecData(Data);
        }
        public string StartUp(CONTRACTEntity Data)
        {
            return service.HtglService.StartUp(Data);
        }
        public string Stop(CONTRACTEntity Data)
        {
            return service.HtglService.Stop(Data);
        }
        public UIResult SearchContract(CONTRACTEntity Data)
        {
            var res = service.HtglService.GetContractElement(Data);
            var mid = res.Item1.MERCHANTID.Value;
            var paymentid= res.Item1.PAYMENTID.Value+"";
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
                    contractPayment= mpayment
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
        public UIResult zlYdFj(List<CONTRACT_RENTEntity> Data, CONTRACTEntity ContractData)
        {
            return new UIResult(service.HtglService.zlYdFj(Data, ContractData));
        }
        public UIResult SearchInit()
        {
            SearchItem item = new SearchItem();
            var feeRule = service.XtglService.GetFeeRule(item);
            var lateFeeRule = service.XtglService.GetLateFeeRule(item);
            //var org_zs = service.DataService.org_zs();
            var operrule = service.DataService.operrule(); 
            var org = service.DataService.org_zslist();
            return new UIResult(
                new
                {
                    FeeRule = feeRule,
                    LateFeeRule = lateFeeRule,
                    //Org_zs = org_zs,
                    Operrule = operrule,
                    Org= org
                }
            );
        }
        public string Output(SearchItem item)
        {
            return service.HtglService.GetContractOutput(item);
        }
        //返回节点数据，并且返回当前节点要面临的操作步骤
        public UIResult Srchsplc(SPLCEntity Data)
        {
            var res = service.XtglService.GetSplc(Data);
            return new UIResult(
                new
                {
                    splcjd = res.Item1,
                    splcjg = res.Item2,
                    curJdid = res.Item3
                }
            );
        }
        public void ExecSplc(SPLCJG_MENUEntity Data)
        {
            if (Data.JGTYPE == ((int)审批流程节点类型.结束).ToString())
            {
                var Data1 = new CONTRACTEntity();
                Data1.CONTRACTID = Data.BILLID;
                var res = service.HtglService.GetContractElement(Data1);
                Data1.HTLX = res.Item1.HTLX;
                Data1.STATUS = res.Item1.STATUS;
                service.HtglService.ExecData(Data1);
            }
            else
            {
                service.XtglService.ExecMenuSplc(Data);
            }   
        }
        //检查合同做变更时是否已存在未启动的变更合同
        public string checkHtBgData(CONTRACTEntity Data)
        {
            return service.HtglService.checkHtBgData(Data);
        }
    }
}