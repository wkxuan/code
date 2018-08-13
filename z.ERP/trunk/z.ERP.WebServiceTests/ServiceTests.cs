using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using z.ERP.Entities.Service.Pos;
using z.Extensions;

namespace z.ERP.WebService.Wcf.Tests
{
    [TestClass()]
    public class ServiceTests
    {
        [TestMethod()]
        public void DoTest()
        {
            RequestDTO dto = new RequestDTO()
            {
                SecretKey = "fjRVf+gZB+h1a+wXW2wOANRfaqn95kr0A9zPTpkW+D8ADbj421cRkF0+vYUCTzpEeZZuRqu5K9s50TgqbiJyaezcsU/z5E1lc2XnIyHOiBASwsMka93Mkwljk91bL20Vvh/jU5jGFf6wKorBjvjL7jTG7Cbo0CtWZ95Ip5Q0dAE=",
                ServiceName = "Sale",
                Context = new SaleRequest()
                {
                    // account_date="2018-"
                }.ToJson()
            };
            ServiceTransfer st = new ServiceTransfer();
            ResponseDTO res = st.Do(dto);
            var ress = res.Context.ToObj<List<FindGoodsResult>>();
        }
    }
}