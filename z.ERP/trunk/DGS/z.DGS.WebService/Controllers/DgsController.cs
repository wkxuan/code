using System.Collections.Generic;
using z.WebServiceBase.Controllers;
using z.WebServiceBase.Model;

namespace z.DGS.WebService.Controllers
{
    public class DgsController : BaseController
    {
        internal DgsController() : base()
        {
          
        }

        [ServiceAble("Test")]
        public string  Test()
        {
            return service.DgsService.Test();
        }
    }
}