using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Model;
using z.ERP.Entities.Enum;
using System.Data;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.Search;
using z.ERP.Web.Areas.Layout.EditDetail;


namespace z.ERP.Web.Areas.ZLHT_BG.ZlHt_Bg
{
    public class ZlHt_BgController : BaseController
    {
        public ActionResult ZlHt_BgEdit(string Id)
        {
            ViewBag.Title = "租赁租约信息变更";
            return View("ZlHt_BgEdit", model: (EditRender)Id);
        }
        [Permission("10600401")]
        public string Save(CONTRACTEntity SaveData)
        {
            return service.HtglService.SaveContract(SaveData);
        }
        public ActionResult ZlHt_BgDetail(string Id)
        {
            ViewBag.Title = "租赁租约变更浏览";
            var entity = service.HtglService.GetContractElement(new CONTRACTEntity(Id));
            ViewBag.contract = entity.Item1;
            ViewBag.contractBrand = entity.Item2;
            ViewBag.contractShop = entity.Item3;
            ViewBag.ContractParm = entity.Item4;
            ViewBag.ContractRentParm = entity.Item5;
            ViewBag.contractPay = entity.Item6;
            ViewBag.contractCost = entity.Item7;
            return View();
        }

        public UIResult SearchContract(CONTRACTEntity Data)
        {
            var res = service.HtglService.GetContractElement(Data);
            return new UIResult(
                new
                {
                    contract = res.Item1,
                    contractBrand = res.Item2,
                    contractShop = res.Item3,
                    ContractParm = res.Item4,
                    ContractRentParm = res.Item5,
                    contractPay = res.Item6,
                    contractCost = res.Item7
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

        public void Delete(List<CONTRACTEntity> DeleteData)
        {
            service.HtglService.DeleteContract(DeleteData);
        }
        [Permission("10600402")]
        public void ExecData(CONTRACTEntity Data)
        {
            service.HtglService.ExecHtBgData(Data);
        }

        public UIResult SearchInit()
        {
            SearchItem item = new SearchItem();
            return service.XtglService.GetFeeRule(item);
        }

    }
}