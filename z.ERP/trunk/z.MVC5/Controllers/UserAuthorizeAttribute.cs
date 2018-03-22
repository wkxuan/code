using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using z.Extensions;
using z.MVC5.Attributes;
using z.MVC5.Models;
using z.MVC5.Results;
using z.SSO;
using z.SSO.Model;

namespace z.MVC5.Controllers
{
    class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            PermissionAttribute attr;
            if (!HasPermission(filterContext, out attr))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new UIResult()
                    {
                        Data = new
                        {
                            Flag = 104,
                            Msg = $"没有权限:{attr.Key}"
                        }
                    };
                }
                else
                {
                    var viewdata = new ViewDataDictionary();
                    viewdata.Add(new KeyValuePair<string, object>("Model", new NoPermissionModel()
                    {
                        PermissionKey = attr.Key.ToString()
                    }));
                    filterContext.Result = new ViewResult() { ViewName = "/Areas/Base/NoPermission.cshtml", ViewData = viewdata };
                }
            }
        }

        public bool HasPermission(AuthorizationContext filterContext, out PermissionAttribute attr)
        {
            attr = filterContext.ActionDescriptor.GetCustomAttributes(false).FirstOrDefault(a => a is PermissionAttribute) as PermissionAttribute;
            if (attr == null)
            {
                return true;
            }
            else
            {
                return UserApplication.GetUser<Employee>().HasPermission(attr.Key);
            }
        }

    }
}
