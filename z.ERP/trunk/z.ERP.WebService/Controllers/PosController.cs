using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public string GetLastDealid()
        {
            return service.PosService.GetLastDealid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [ServiceAble("GetYYY")]
        public UserYYYResult GetYYY(string code)
        {
            return service.PosService.GetYYY(code);
        }

        [ServiceAble("GetFKFS")]
        public List<FKFSResult> GetFKFS()
        {
            return service.PosService.GetFKFS();
        }

        [ServiceAble("GetDeal")]
        public DealResult GetDeal(string dealid)
        {
            return service.PosService.GetDeal();
        }

        [ServiceAble("Sale")]
        public void Sale(SaleRequest Request)
        {
            service.PosService.Sale(Request);
        }
    }
}