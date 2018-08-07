using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.WebService.Controllers;
using z.ERP.WebService.Model;
using z.Extensions;

namespace z.ERP.WebService.Ashx
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            LoginResponseDTO res;
            LoginRequestDTO dto = new LoginRequestDTO()
            {
                platformId = HttpExtension.GetRequestParam("PlatformId"),
                userCode = HttpExtension.GetRequestParam("UserName"),
                userPassword = HttpExtension.GetRequestParam("UserPassword")
            };
            if (dto.platformId.IsEmpty() || dto.userCode.IsEmpty() || dto.userPassword.IsEmpty())
            {
                res = new LoginResponseDTO()
                {
                    success = false,
                    errorMsg = "登陆信息不完整"
                };
            }
            else
                res = new CommonController().Login(dto);
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