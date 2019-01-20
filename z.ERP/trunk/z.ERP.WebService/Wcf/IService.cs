using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using z.WebServiceBase.Model;

namespace z.ERP.WebService.Wcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService”。
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        ResponseDTO Do(RequestDTO dto);

        [OperationContract]
        LoginResponseDTO Login(LoginRequestDTO dto);
    }
}
