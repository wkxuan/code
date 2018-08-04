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

        [ServiceAble("FindGoods")]
        public List<FindGoodsResult> FindGoods(FindGoodsFilter filter)
        {
            return service.PosService.FindGoods(filter);
        }
    }
}