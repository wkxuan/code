using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Attributes;
using z.MVC5.Models;
using z.MVC5.Results;
using z.Results;
using z.SSO;
using z.SSO.Model;

namespace z.MVC5.Controllers
{
    public class ActionProcessAttribute : ActionFilterAttribute
    {
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
                            Data = new
                            {
                                Flag = ex.Flag,
                                Msg = ex.Message
                            }
                        };
                    }
                    else
                    {
                        res = new UIResult()
                        {
                            Data = new
                            {
                                Flag = -1,
                                Msg = filterContext.Exception.InnerMessage()
                            }
                        };
                    }
                    filterContext.Result = res;
                }
                else
                {
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
                    else if (filterContext.Result is ContentResult)
                    {
                        filterContext.Result = new UIResult()
                        {
                            Data = (filterContext.Result as ContentResult).Content
                        };
                    }
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
                filterContext.Controller.ViewBag.BaseUrl = HttpExtension.GetWebBasePath();
                filterContext.Controller.ViewBag.Domain = HttpContext.Current.Request.Url.Host;
            }
            base.OnActionExecuted(filterContext);
        }
    }
}
