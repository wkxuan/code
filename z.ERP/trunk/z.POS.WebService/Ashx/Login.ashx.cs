using System.Web;
using z.Extensions;
using z.WebServiceBase.Model;
using z.POS.WebService.Controllers;

namespace z.POS.WebService.Ashx
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
                PlatformId = HttpExtension.GetRequestParam("PlatformId"),
                UserCode = HttpExtension.GetRequestParam("UserCode"),
                UserPassword = HttpExtension.GetRequestParam("UserPassword")
            };
            if (dto.PlatformId.IsEmpty() || dto.UserCode.IsEmpty() || dto.UserPassword.IsEmpty())
            {
                res = new LoginResponseDTO()
                {
                    Success = false,
                    ErrorMsg = "登陆信息不完整"
                };
            }
            else
            {
                res = new CommonController().Login(dto);
            }
                
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