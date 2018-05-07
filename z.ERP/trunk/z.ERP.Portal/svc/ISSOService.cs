using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using z.SSO.Model;

namespace z.ERP.Portal.svc
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISSOService”。
    [ServiceContract]
    public interface ISSOService
    {
        [OperationContract]
        User GetUserById(string id);

        [OperationContract]
        User GetUserByCode(string code, string password);

        [OperationContract]
        string[] GetPermissionByUserId(string userid);
    }
}
