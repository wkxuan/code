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

    public enum 部门类型
    {
        核算部门 = 1,
        招商部门 = 2,
        其他部门 = 3
    }

    public enum 合同类型
    {
        租赁合同 = 1,
        联营合同 = 2
    }
    public enum 收费项目成本费用
    {
        记成本 = 1,
        记费用 = 2
    }
    public enum 收费项目类型
    {
        保证金押金 = 1,
        每月收费项目 = 2,
        能源费费 = 3
    }
    public enum 收费项目现金货扣
    {
        现金 = 1,
        货扣 = 2
    }
    public enum 收款方式返款
    {
        返款 = 1,
        不返款 = 2
    }
    public enum 收款方式积分
    {
        积分 = 1,
        不积分 = 2
    }
    public enum 收款方式找零方式
    {
        不能多收 = 1,
        多收不找零 = 2
    }
    public enum 停用标记
    {
        在用 = 1,
        停用 = 2,
    }

    public enum 支付方式类型
    {
        现金 = 1,
        提货卡 = 2,
        优惠券 = 3,
        银行卡 = 4,
        支付宝 = 5,
        微信 = 6
    }
}
