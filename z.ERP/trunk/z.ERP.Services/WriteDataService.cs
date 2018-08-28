using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ERP.Entities;
using z.Exceptions;

namespace z.ERP.Services
{
    public class WriteDataService: ServiceBase
    {
        internal WriteDataService()
        {

        }

        public void CanRcl(WRITEDATAEntity WRITEDATA)
        {
           
            //循环查询分店信息
            //写表提示是否能够日处理

            RCL_HOSTEntity host = DbHelper.Select(new RCL_HOSTEntity());
            if (host.HOSTNAME != null) {
                throw new LogicException($"日处理正在进行中!");
            }
        }
    }
}
