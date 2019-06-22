using System;
using z.Extensions;
using z.POS80.WebService.Controllers;
using z.WebServiceBase.Model;

namespace z.POS80.WebService.Wcf
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
            return new CommonController().Login(dto);
        }

    }
}
