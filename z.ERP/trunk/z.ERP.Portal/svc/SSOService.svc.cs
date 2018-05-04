using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using z.SSO.Model;

namespace z.ERP.Portal.svc
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SSOService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SSOService.svc 或 SSOService.svc.cs，然后开始调试。
    public class SSOService : BaseService, ISSOService
    {
        public User GetUserById(string id)
        {
            return service.HomeService.GetUserById(id);
        }

        public User GetUserByCode(string code,string password)
        {
            return service.HomeService.GetUserByCode(code, password);
        }

        public string[] GetPermissionByUserId(string userid)
        {
            return service.HomeService.GetPermissionByUserId(userid);
        }
    }
}
