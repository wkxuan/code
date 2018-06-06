using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using z.Extensions;
using z.LogFactory;
using z.MVC5.Models;

namespace z.MVC5.Controllers
{
    /// <summary>
    /// 异常持久化类
    /// </summary>
    public class ExceptionLogAttribute : HandleErrorAttribute
    {
        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Controller");
            }
        }
        /// <summary>
        /// 触发异常时调用的方法,ajax的异常已经处理过了
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.ExceptionHandled = true;
                var viewdata = new ViewDataDictionary();
                viewdata.Add(new KeyValuePair<string, object>("Model", new ErrorModel()
                {
                    Ex = filterContext.Exception.GetInnerException(),
                    Site = filterContext.RouteData.GetRequiredString("controller") + "/" + filterContext.RouteData.GetRequiredString("action")
                }));
                Log.Error(filterContext.Exception);
                filterContext.Result = new ViewResult() { ViewName = "/Areas/Base/Error.cshtml", ViewData = viewdata };
            }
        }
    }
}
