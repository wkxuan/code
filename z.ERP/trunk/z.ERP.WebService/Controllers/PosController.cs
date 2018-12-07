using System.Collections.Generic;
using z.ERP.API.PosServiceAPI;
using z.ERP.Entities.Service.Pos;
using z.ERP.WebService.Model;

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
        [ServiceAble("GetClerkShop")]
        public UserYYYResult GetClerkShop(string usercode)
        {
            return service.PosService.GetClerkShop(usercode);
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

        [ServiceAble("GetVipCard")]
        public VipCard GetVipCard(GetVipCardRequest Request)
        {
            return service.PosService.GetVipCard(Request);
        }

        [ServiceAble("GetArticleVipDisc")]
        public ArticleVipDisc[] GetArticleVipDisc(GetArticleVipDiscRequest request)
        {
            return service.PosService.GetArticleVipDisc(request);
        }

        [ServiceAble("GetCashCard")]
        public CashCard GetCashCard(GetCashCardRequest Request)
        {
            return service.PosService.GetCashCard(Request);
        }

        [ServiceAble("PrepareTransCashCardPayment")]
        public int PrepareTransCashCardPayment(PrepareTransCashCardPaymentRequest Request)
        {
            return service.PosService.PrepareTransCashCardPayment(Request);
        }


    }
}