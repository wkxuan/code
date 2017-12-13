using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Enum
{
    public enum 物业标记
    {
        保底租金 = 1,
        保底租金含物业费 = 2,
        保底租金金额段 = 3,    //不同销售额段，不同租金
    }
    public enum 合作方式
    {
        纯扣 = 1,
        纯保底 = 2,
        保底与流水取高 = 3,
        保底与流水取和 = 4,
    }
    public enum 阶梯式标记
    {
        阶梯式 = 1,
        非阶梯式 = 2,
    }
}
