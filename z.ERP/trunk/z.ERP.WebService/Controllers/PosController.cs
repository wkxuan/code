﻿using System.Collections.Generic;
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
        /// 获取登陆配置
        /// </summary>
        /// <returns></returns>

        public LoginConfigInfo GetConfig()
        {
            return service.PosService.GetConfig();
        }

        [ServiceAble("BindAddress")]
        public void BindAddress(Address ads)
        {
            service.PosService.BindAddress(ads);
        }

        [ServiceAble("RefreshTime")]
        public void RefreshTime(bool isQuit = false)
        {
            service.PosService.RefreshTime(isQuit);
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <returns></returns>
        [ServiceAble("GetNotice")]
        public NoticeResult GetNotice()
        {
            return service.PosService.GetNotice();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="passw"></param>
        [ServiceAble("ChangePassword")]
        public void ChangePassword(PasswordInfo passw)
        {
            service.PosService.ChangePassword(passw);
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

        [ServiceAble("GetSaleCountSummary")]
        public SaleCountSummaryResult GetSaleCountSummary(SaleSummaryFilter filter)
        {
            return service.PosService.GetSaleCountSummary(filter);
        }

        /// <summary>
        /// 销售明细列表_分页
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ServiceAble("GetSaleLists")]
        public SaleListsResult GetSaleLists(SaleListsFilter filter)
        {
            return service.PosService.GetSaleLists(filter);
        }

        /// <summary>
        /// 获取积分抵现规则
        /// </summary>
        /// <returns></returns>
        [ServiceAble("GetRedemptionRule")]
        public RedeRuleResult GetRedemptionRule()
        {
            return service.PosService.GetRedemptionRule();
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
        public ConfirmDealResult ConfirmDeal(ReqConfirmDeal req)
        {
            return service.PosService.ConfirmDeal(req);
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