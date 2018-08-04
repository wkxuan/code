using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using z.Encryption;
using z.ERP.WebService.Controllers;
using z.ERP.WebService.Model;

namespace z.ERP.WebService.Wcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service.svc 或 Service.svc.cs，然后开始调试。
    public class Service : IService
    {
        public ResponseDTO Do(RequestDTO dto)
        {
            dto = new RequestDTO()
            {
                Key = "fjRVf+gZB+h1a+wXW2wOANRfaqn95kr0A9zPTpkW+D8ADbj421cRkF0+vYUCTzpEeZZuRqu5K9s50TgqbiJyaezcsU/z5E1lc2XnIyHOiBASwsMka93Mkwljk91bL20Vvh/jU5jGFf6wKorBjvjL7jTG7Cbo0CtWZ95Ip5Q0dAE=",
                ServiceName = "FindGoods",
                Context = "{goodsdm:'1',clerkid:'2'}"
            };
            ServiceTransfer st = new ServiceTransfer();
            return st.Do(dto);
        }

        public LoginResponseDTO Login(LoginRequestDTO dto)
        {
            try
            {
                CommonController Controller = new CommonController();
                return Controller.Login(dto);
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO()
                {
                    Success = false,
                    ErrorMsg = ex.Message
                };
            }
        }

    }
}
