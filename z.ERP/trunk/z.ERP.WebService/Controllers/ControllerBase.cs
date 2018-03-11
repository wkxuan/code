using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.IOC.Simple;

namespace z.ERP.WebService.Controllers
{
    public class ControllerBase
    {
        SimpleIOC ioc;
        public ControllerBase()
        {
            List<Type> mrs = new List<Type>();
            ioc = new SimpleIOC(mrs);
        }
    }
}