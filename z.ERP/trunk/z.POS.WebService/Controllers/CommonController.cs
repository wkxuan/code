using System;
using z.Extensions;
using z.SSO;
using z.SSO.Model;
using z.WebServiceBase;
using z.WebServiceBase.Model;

namespace z.POS.WebService.Controllers
{
    public class CommonController : BaseController
    {
        LoginResponseDTO _login(LoginRequestDTO dto)
        {
            var user = service.HomeService.GetUserByCode(dto.UserCode, dto.UserPassword);
            string LoginStr = ServiceUserHelper.GetSrc(new ServiceUser()
            {
                PlatformId = dto.PlatformId,
                Id = user.Id,
                Name = user.Name
            });
            UserApplication.Login(LoginStr, null);
            return new LoginResponseDTO()
            {
                Success = true,
                SecretKey = LoginStr,
                UserId = employee.Id,
                UserName = employee.Name
            };
        }

        public ResponseDTO Do(RequestDTO dto)
        {
            ResponseDTO res;
            if (dto == null)
                res = new ResponseDTO()
                {
                    Success = false,
                    ErrorMsg = "传入对象为空"
                };
            try
            {
                ServiceTransfer st = new ServiceTransfer();
                res = st.Do(dto);
            }
            catch (Exception ex)
            {
                res = new ResponseDTO()
                {
                    Success = false,
                    ErrorMsg = ex.InnerMessage(),
                    Context = ""
                };
            }
            Log.Info($"{(UserApplication.HasLogin ? employee.Id : "未登录")}:{dto.ServiceName}", dto.Context, res);
            return res;
        }

        public LoginResponseDTO Login(LoginRequestDTO dto)
        {
            LoginResponseDTO res;
            if (dto == null)
                res = new LoginResponseDTO()
                {
                    Success = false,
                    ErrorMsg = "传入对象为空"
                };
            else
            {
                try
                {
                    res = _login(dto);
                }
                catch (Exception ex)
                {
                    res = new LoginResponseDTO()
                    {
                        Success = false,
                        ErrorMsg = ex.Message
                    };
                }
            }
            Log.Info($"Login", dto, res);
            return res;
        }
    }
}