using System.Collections.Generic;
using z.ERP.API.PosServiceAPI;
using z.ERP.Entities.Service.Pos;
using z.ERP.Services;
using z.WebServiceBase.Controllers;
using z.WebServiceBase.Model;

namespace z.ERP.WebService.Controllers
{
    public class PosController : BaseController
    {
        internal PosController() : base()
        {
          
        }



        /// <summary>
        /// 商品-单价
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ServiceAble("FindGoods")]
        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            return service.PosService.FindGoods(filter);
        }

        /// <summary>
        /// 最大交易号
        /// </summary>
        /// <returns></returns>
        [ServiceAble("GetLastDealid")]
        public long GetLastDealid()
        {
            return service.PosService.GetLastDealid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
        public LoginConfigInfo GetConfig()
        {
            return service.PosService.GetConfig();
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

        [ServiceAble("Sale")]
        public void Sale(SaleRequest Request)
        {
            service.PosService.Sale(Request);
        }

        [ServiceAble("GetSaleSummary")]
        public SaleSummaryResult GetSaleSummary(SaleSummaryFilter filter)
        {
            return service.PosService.GetSaleSummary(filter);
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

    }
}