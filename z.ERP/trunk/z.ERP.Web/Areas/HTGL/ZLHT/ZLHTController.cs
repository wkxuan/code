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

namespace z.ERP.Web.Areas.HTGL.ZLHT
{
    public class ZLHTController: BaseController
    {
        public ActionResult HtList()
        {
            ViewBag.Title = "租约列表信息";
            return View();
        }

        public ActionResult HtEdit(string Id)
        {
            ViewBag.Title = "租赁租约信息编辑";
            ViewBag.STYLE = 2;
            return View("HtEdit",model: Id);
        }

        public string Save(CONTRACTEntity SaveData)
        {
            return service.HtglService.SaveContract(SaveData);
        }

        public UIResult SearchContract(CONTRACTEntity Data)
        {
            var res = service.HtglService.GetContractElement(Data);
            return new UIResult(
                new
                {
                    contract = res.Item1,
                    contractBrand =res.Item2,
                    contractShop=res.Item3,
                    ContractParm = res.Item4,
                    ContractRentParm=res.Item5,
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


        public UIResult zlYdFj(List<CONTRACT_RENTEntity> Data, CONTRACTEntity ContractData)
        {
            return new UIResult(service.HtglService.zlYdFj(Data, ContractData));
        }

    }
}