using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.WebService.Controllers.Model;
using z.ERP.WebService.Model;

namespace z.ERP.WebService.Controllers
{
    public class PosController : ControllerBase
    {
        [ServiceAble("PosSale")]
        public void Sale(PosSaleModel Model)
        {

        }
    }
}