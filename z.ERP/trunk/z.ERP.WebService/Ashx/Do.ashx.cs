using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.WebService.Controllers;
using z.Extensions;

namespace z.ERP.WebService.Ashx
{
    /// <summary>
    /// Service 的摘要说明
    /// </summary>
    public class Do : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var res = new CommonController().Do(new RequestDTO()
            {
                SecretKey = HttpExtension.GetRequestParam("SecretKey"),
                ServiceName = HttpExtension.GetRequestParam("ServiceName"),
                Context = HttpExtension.GetRequestParam("Context")
            });
            context.Response.ContentType = "text/plain";
            context.Response.Write(res.ToJson());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}