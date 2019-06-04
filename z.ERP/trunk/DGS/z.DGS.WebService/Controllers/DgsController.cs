using System.Collections.Generic;
using z.WebServiceBase.Controllers;
using z.WebServiceBase.Model;
using z.DGS.Entities;

namespace z.DGS.WebService.Controllers
{
    public class DgsController : BaseController
    {
        internal DgsController() : base()
        {
          
        }

        /// <summary>
        /// 销售采集接口
        /// </summary>
        /// <param name="req"></param>
        [ServiceAble("SaleGather")]
        public void SaleGather(SaleGatherReq req)
        {
            service.DgsService.SaleGather(req);
        }
    }
}