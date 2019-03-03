using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.IOC.Simple;
using z.LogFactory;
using z.SSO;
using z.SSO.Model;

namespace z.WebServiceBase.Controllers
{
    public class ServiceBaseController
    {
        SimpleIOC ioc;
        public ServiceBaseController()
        {
            List<Type> mrs = new List<Type>();
            ioc = new SimpleIOC(mrs);
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

        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Pos");
            }
        }

    }
}