using z.ERP.WebService.Model;
using z.SSO;
using z.SSO.Model;

namespace z.ERP.WebService.Controllers
{
    public class CommonController : BaseController
    {
        public LoginResponseDTO Login(LoginRequestDTO dto)
        {
            var user = service.HomeService.GetUserByCode(dto.userCode, dto.userPassword);

            

            string LoginStr = ServiceUserHelper.GetSrc(new ServiceUser()
            {
                PlatformId = dto.platformId,
                Id = user.Id,
                Name = user.Name
            });
            UserApplication.Login(LoginStr, null);
            return new LoginResponseDTO()
            {
                success = true,
                secretKey = LoginStr,
                userid = employee.Id,
                userName = employee.Name
            };
        }
    }
}