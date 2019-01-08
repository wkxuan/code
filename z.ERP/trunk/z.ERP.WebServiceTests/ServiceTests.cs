using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using z.ERP.API.PosServiceAPI;
using z.ERP.Entities.Service.Pos;
using z.ERP.Services;
using z.Extensions;

namespace z.ERP.WebService.Wcf.Tests
{
    [TestClass()]
    public class ServiceTests
    {
        [TestMethod()]
        public void DoTest()
        {
            try
            {
                RequestDTO dto = new RequestDTO()
                {
                    SecretKey = "fjRVf+gZB+h1a+wXW2wOANRfaqn95kr0A9zPTpkW+D8ADbj421cRkF0+vYUCTzpEeZZuRqu5K9s50TgqbiJyaezcsU/z5E1lc2XnIyHOiBASwsMka93Mkwljk91bL20Vvh/jU5jGFf6wKorBjvjL7jTG7Cbo0CtWZ95Ip5Q0dAE=",
                    ServiceName = "GetVipCard",
                    Context = new GetVipCardRequest()
                    {
                        condType = 1,
                        condValue = "123123"
                    }.ToJson()
                };
                ServiceTransfer st = new ServiceTransfer();
                ResponseDTO res = st.Do(dto);
                var ress = res.Context.ToObj<VipCard>();
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod()]
        public void CalcAccountsPayableTest()
        {
            try
            {
                RequestDTO dto = new RequestDTO()
                {
                    SecretKey = "TfzHzL9AJGeYLwWM0NECZYfDbbPPb+mDO7I8YnbXwmFuzEpZdmwVYItWCg6/7nUn88rpDGzjPBhepR7Vwu/G5lwN40hIjKkVKkdUoj/U7LK7QbYooVj5NeZE/lRPP0P1DddX1XXgtujLAyoUB5pjPYxRxirWZKsIPO9VFOLU55s=",
                    ServiceName = "CalcAccountsPayable",
                    Context = "{contractID:0,vipIsDiscount:1,validType:2,ValidID:'30000000',deptID:1,deptCode:'01',goodsList:[{id:11,code:'00010001',price:100,frontendOffAmount:0,count:2,deptid:0,deptCode:''}]}"
                };
                ServiceTransfer st = new ServiceTransfer();
                ResponseDTO res = st.Do(dto);
                var ress = res.Context.ToObj<CalcAccountsPayableResult>();
            }
            catch (Exception ex)
            {
                
            }
        }

        [TestMethod()]
        public void ConfirmDealTest()
        {
            TTranGoods goods = new TTranGoods()
            {
                tickInx = 0,
                assistantId = 1,
                inx = 0,
                id = 11,
                code = "00010001",
                name = "奈雪の茶",
                price = 100,
                count = 2,
                totalOffAmount = 200,
                accountsPayable = 200,
                deptID = 1,
                deptCode = "01",
                frontendOffAmount = 0,
                backendOffAmount = 0,
                changeDiscount = 0,
                backendOffID = 0,
                memberOff = 0,
                memberOffID = 0,
                fullCutOffAmount = 0,
                fullCutOffID = 0,
                roundOff = 0
            };

            TTranPayments pays = new TTranPayments()
            {
                Id = 1,
                PayMoney = 200
            };


            try
            {
                RequestDTO dto = new RequestDTO()
                {
                    SecretKey = "fjRVf+gZB+h1a+wXW2wOANRfaqn95kr0A9zPTpkW+D8ADbj421cRkF0+vYUCTzpEeZZuRqu5K9s50TgqbiJyaezcsU/z5E1lc2XnIyHOiBASwsMka93Mkwljk91bL20Vvh/jU5jGFf6wKorBjvjL7jTG7Cbo0CtWZ95Ip5Q0dAE=",
                    ServiceName = "ConfirmDeal",
                    Context = new ReqConfirmDeal()
                    {
                        contractID = 0,
                        validType = "2",
                        validID = "30000000",
                        deptID = 1,
                        deptCode = "01",
                        outOrder = "",
                        erpTranID = "187",
                        crmTranID = "1173",
                        DDJLBH = "",
                        goodsList = new List<TTranGoods>()
                        {
                         goods
                         },
                        paysList = new List<TTranPayments>()
                        {
                         pays
                        },
                        couponsList = null,
                        cashCashList = null,
                        creditDetailList = null,
                        //  public List<TTranPayments> paysList;
                        //  public List<TTranCoupon> couponsList;
                        //  public List<CashCardDetails> cashCashList;
                        //  public List<CreditDetail> creditDetailList; //银行付款明细

                    }.ToJson()
                };
                ServiceTransfer st = new ServiceTransfer();
                ResponseDTO res = st.Do(dto);
                var ress = res.Context.ToObj<ConfirmDealResult>();
            }
            catch (Exception ex)
            {

            }
        }


        /* [TestMethod()]
         public void aaaaaaaaa()
         {
             try
             {
                 ServiceBase service = new ServiceBase();
                 service.CommonService.a();
             }
             catch (Exception ex)
             {
             }
         }
         [TestMethod()]
         public void bbbbbbbbb()
         {
             try
             {
                 //ServiceBase service = new ServiceBase();
                 //var a = service.PosService.GetVipCoupon(new GetVipCardRequest()
                 //{
                 //    condType = 1,
                 //    condValue = "123123"
                 //});
             }
             catch (Exception ex)
             {
             }
         } */
    }
}