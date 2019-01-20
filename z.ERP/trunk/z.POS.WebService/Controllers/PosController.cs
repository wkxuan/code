using System.Collections.Generic;
using z.WebServiceBase.Controllers;
using z.WebServiceBase.Model;

namespace z.POS.WebService.Controllers
{
    public class PosController : BaseController
    {
        internal PosController() : base()
        {
          
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
        
    }
}