using System;
using z.ERP.WebService.Controllers;
using z.ERP.WebService.Model;
using z.Extensions;
using z.ERP.Entities.Service.Pos;

namespace z.ERP.WebService.Wcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service.svc 或 Service.svc.cs，然后开始调试。
    public class Service : IService
    {
        public ResponseDTO Do(RequestDTO dto)
        {
            return new CommonController().Do(dto);
        }

        public LoginResponseDTO Login(LoginRequestDTO dto)
        {
            LoginResponseDTO res = new CommonController().Login(dto);

            res.ConfigInfo = null;

            if (res.Success)
            {
                LoginConfigInfo lgi = new PosController().GetConfig();

                if (lgi == null)
                {
                    res.UserId = null;
                    res.UserName = null;
                    res.SecretKey = null;
                    res.Success = false;
                    res.ErrorMsg = "终端未定义";
                }
                else
                    res.ConfigInfo = lgi.ToJson<LoginConfigInfo>();
            }
            return res;

            //  return new CommonController().Login(dto);
        }

    }
}
