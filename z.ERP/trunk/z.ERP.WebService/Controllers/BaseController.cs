using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.Services;
using z.IOC.Simple;
using z.LogFactory;
using z.SSO;
using z.SSO.Model;

namespace z.ERP.WebService.Controllers
{
    public class BaseController
    {
        SimpleIOC ioc;
        public BaseController()
        {
            List<Type> mrs = new List<Type>();
            ioc = new SimpleIOC(mrs);
            service = new ServiceBase();
        }

        public object Create(Type t)
        {
            return ioc.Create(t);
        }

        /// <summary>
        /// 当前登录对象
        /// </summary>
        protected Employee employee
        {
            get
            {
                return UserApplication.GetUser<ServiceUser>();
            }
        }

        protected ServiceBase service
        {
            get;
            set;
        }

        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Pos");
            }
        }

    }
}