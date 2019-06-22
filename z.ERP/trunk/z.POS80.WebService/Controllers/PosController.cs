using System.Collections.Generic;
using z.POS80.Entities.Pos;
using z.WebServiceBase.Controllers;
using z.WebServiceBase.Model;

namespace z.POS80.WebService.Controllers
{
    public class PosController : BaseController
    {
        internal PosController() : base()
        {
          
        }

        public LoginConfigInfo GetConfig()
        {
            return service.PosService.GetConfig();
        }

        [ServiceAble("BindAddress")]
        public void BindAddress(Address ads)
        {
            service.PosService.BindAddress(ads);
        }

        /// <summary>
        /// 最大交易号,测试方法,开始做就要删除
        /// </summary>
        /// <returns></returns>
        [ServiceAble("GetLastDealid")]
        public long GetLastDealid()
        {
            return service.PosService.GetLastDealid();
        }

        [ServiceAble("FindGoods")]
        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            return service.PosService.FindGoods(filter);
        }

        [ServiceAble("GetClerkShop")]
        public UserYYYResult GetClerkShop(PersonInfo req)
        {
            return service.PosService.GetClerkShop(req);
        }

        [ServiceAble("GetPayList")]
        public List<FKFSResult> GetPayList()
        {
            return service.PosService.GetPayList();
        }

        [ServiceAble("GetDeal")]
        public SaleRequest GetDeal(GetDealFilter filter)
        {
            return service.PosService.GetDeal(filter);
        }

        /*
        [ServiceAble("Sale")]
        public void Sale(SaleRequest Request)
        {
            service.PosService.Sale(Request);
        }
        */

        [ServiceAble("GetSaleList")]
        public SaleListResult GetSaleList(SaleListFilter filter)
        {
            return service.PosService.GetSaleList(filter);
        }

        [ServiceAble("GetSaleSummary")]
        public SaleSummaryResult GetSaleSummary(SaleSummaryFilter filter)
        {
            return service.PosService.GetSaleSummary(filter);
        }


        [ServiceAble("GetMemberInfo")]
        public GetMemberCardDetailsResult GetMemberInfo(ReqMemberCard reqMC)
        {
            return service.PosService.GetMemberInfo(reqMC);
        }

        [ServiceAble("GetCardPayable")]
        public GetCardPayableResult GetCardPayable(ReqGetCardPayable reqMth)
        {
            return service.PosService.GetCardPayable(reqMth);
        }

        [ServiceAble("CalcAccountsPayable")]
        public CalcAccountsPayableResult CalcAccountsPayable(ReqGetGoods reqMth)
        {
            return service.PosService.CalcAccountsPayable(reqMth);
        }

        [ServiceAble("ConfirmDeal")]
        public ConfirmDealResult ConfirmDeal(ReqConfirmDeal ReqConfirm)
        {
            return service.PosService.ConfirmDeal(ReqConfirm);
        }

        [ServiceAble("CalcAccountsBackable")]
        public RespBackable CalcAccountsBackable(ReqBackAble req)
        {
            return service.PosService.CalcAccountsBackable(req);
        }

        [ServiceAble("ConfirmBackDeal")]
        public ConfirmBackDealResult ConfirmBackDeal(ReqConfirmBackDeal req)
        {
            return service.PosService.ConfirmBackDeal(req);
        }



    }
}