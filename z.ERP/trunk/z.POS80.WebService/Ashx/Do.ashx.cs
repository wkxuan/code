using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.Extensions;
using z.POS80.WebService.Controllers;
using z.WebServiceBase.Model;

namespace z.POS80.WebService.Ashx
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