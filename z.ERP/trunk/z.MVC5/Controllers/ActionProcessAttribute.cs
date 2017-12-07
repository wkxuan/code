using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Models;
using z.MVC5.Results;
using z.Results;

namespace z.MVC5.Controllers
{
    /// <summary>
    /// 异常持久化类
    /// </summary>
    public class ActionProcessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                UIResult res;
                //异常处理
                if (filterContext.Exception != null)
                {
                    filterContext.ExceptionHandled = true;
                    if (filterContext.Exception.GetInnerException() is zExceptionBase)
                    {
                        zExceptionBase ex = filterContext.Exception.GetInnerException() as zExceptionBase;
                        res = new UIResult()
                        {
                            Flag = ex.Flag,
                            Msg = ex.Message
                        };
                    }
                    else
                    {
                        res = new UIResult()
                        {
                            Flag = -1,
                            Msg = filterContext.Exception.InnerMessage()
                        };
                    }
                    filterContext.Result = res;
                }


                if (filterContext.Result is EmptyResult)
                {
                    filterContext.Result = new UIResult();
                }
                if (filterContext.Result is UIResult)
                {
                    filterContext.Result = new UIResult()
                    {
                        Data = (filterContext.Result as UIResult).GetData()
                    };
                }
            }
            else
            {
                filterContext.Controller.ViewBag.ControllerUrl =
                    IOExtension.MakeUri(
                    HttpExtension.GetWebBasePath(),
                    filterContext.RouteData.Values["area"].ToString(),
                    filterContext.RouteData.Values["controller"].ToString()) + "/";
                filterContext.Controller.ViewBag.CommonControllerUrl =
                    IOExtension.MakeUri(
                    HttpExtension.GetWebBasePath(),
                    "Base",
                    "Common") + "/";
            }
        }
    }
}
