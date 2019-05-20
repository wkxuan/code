﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Enum
{
    public enum 物业标记
    {
        保底租金 = 1,
        保底租金含物业费 = 2
    }
    public enum 合作方式
    {
        纯扣 = 1,
        纯租 = 2,
        保底与流水取高 = 3,
        保底与流水取和 = 4,
    }

    public enum 月费用收费方式
    {
        按日计算固定金额 = 1,
        按日计算月固定金额 = 2,
        按销售金额比例 = 3,
        月固定金额 = 4

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

    public enum 核算方式
    {
        租赁合同 = 1,
        联营合同 = 2,
        多经点位 = 3
    }

    public enum 合同类型
    {
        原始合同 = 1,
        变更合同 = 2
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
        能源费费 = 3,
        其他 = 4
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
        不限制 = 0,
        不能多收 = 1,
        多收不找零 = 2
    }
    public enum 停用标记
    {
        在用 = 1,
        停用 = 2,
    }

    public enum 通知单生成方式
    {
        租金 = 1,
        提成租金 = 2,
    }

    public enum 支付方式类型
    {
        现金 = 1,
        提货卡 = 2,
        优惠券 = 3,
        银行卡 = 4,
        支付宝 = 5,
        微信 = 6,
        扫码支付 = 7
    }
    public enum 单元类型
    {
        铺位 = 1,
        临时场地 = 2,
        广告位 = 3,
        写字间 = 4
    }
    public enum 单元状态
    {
        不可用 = 1,
        正常 = 2,
        拆分 = 3
    }
    public enum 租用状态
    {
        空置 = 1,
        出租 = 2
    }
    public enum POS类型
    {
        统一收银 = 1,
        商铺 = 2,
        接口 = 3
    }
    public enum 普通单据状态
    {
        未审核 = 1,
        审核 = 2,
    }

    public enum 业务类型单据
    {
        物料购进单 = 0,
        物料购进单冲红 = 1,
    }

    public enum 合同状态
    {
        未审核 = 1,
        审核 = 2,
        启动 = 3,
        到期 = 4,
        终止 = 5,
    }

    public enum 联营合同合作方式
    {
        扣点 = 1,
        保底销售 = 2,
        保底毛利 = 3
    }
    public enum 资产调整类型
    {
        资产类型 = 1,
        资产面积 = 2,
        资产拆分 = 3,
    }
    public enum 末级标记
    {
        非末级 = 1,
        末级 = 2,
    }
    public enum 商品类型
    {
        品种 = 1,
        大类 = 2
    }
    public enum 商品状态
    {
        未审核 = 1,
        审核 = 2,
        淘汰 = 3,
    }
    public enum 用户类型
    {
        收款员 = 1,
        营业员 = 2,
        采购员 = 3,
        保管员 = 4,
        物价员 = 5,
        财务人员 = 6,
        合同员 = 7,
        核算员 = 8,
        其它 = -1
    }
    public enum 标记
    {
        是 = 1,
        否 = 2
    }
    public enum 用户标记
    {
        正常 = 1,
        停用 = 2
    }
    public enum 结算单状态
    {
        未审核 = 1,
        审核 = 2,
        部分付款 = 3,
        全部付款 = 4
    }

    public enum 通知单类型
    {
        其它 = 1,
        月末 = 2
    }


    public enum 周期方式
    {
        合同起始日期 = 1,
        自然周期 = 2
    }

    public enum 起始日清算
    {
        是 = 1,
        否 = 2
    }

    public enum 销售额标记
    {
        含税 = 1,
        未税 = 2
    }

    public enum 收款类型
    {
        预收款 = 1,
        保证金收款 = 2,
        账单收款 = 3,

    }
    public enum 账单类型
    {
        账单 = 1,
        调整单 = 2,
        减免单 = 3
    }
    public enum 账单类型Part
    {
        账单 = 1,
        调整单 = 2,
    }
    public enum 月末标记
    {
        月末 = -1
    }
    public enum 账单状态
    {
        未审核 = 1,
        审核 = 2,
        部分付款 = 3,
        全部付款 = 4,
        返还 = 5,
        终止 = 6,
    }
    public enum 账单状态Part
    {
        审核 = 2,
        部分付款 = 3,
    }
    public enum 退铺单状态
    {
        未审核 = 1,
        审核 = 2,
        终止 = 3,
    }

    public enum 查询类型
    {
        日数据 = 1,
        月数据 = 2
    }

    public enum 日处理步骤
    {
        成功 = 0,
        更新交易商品税率 = 1,
        汇总商品销售表 = 2,
        汇总统计维度表 = 3,
        汇总手续费表 = 4,
        转移交易数据 = 5,
        生成租金每月收费项目账单 = 6,
        生成销售相关账单 = 7,
        租约变更启动 = 8,
        生成缴费通知单 = 9
    }

    public enum 会员日处理步骤
    {
        成功 = 0,
        启动促销单据 = 1,
        启动优惠券促销活动单据 = 2,
        更新CRMJZRQ = 3,
        金额账日报表 = 4,
        库存卡保管帐 = 5,
        金额帐消费日报表 = 6,
        优惠券帐消费日报表 = 7,
        会员卡转为睡眠状态 = 8,
        会员卡警告等级变动 = 9,
        会员卡有效期延长规则 = 10,
        会员卡状态变动处理 = 11,
        有效期到期冻结 = 12,
        积分处理 = 13,
        优惠券帐日报表 = 14,
        积分返还礼品进销存报表 = 15,
        汇总储值卡消费明细 = 16,
        移动后台不刷卡历史记录到消费记录 = 17,
        汇总会员卡消费明细 = 18,
        会员卡积分日报表 = 19,
        会员卡等级升迁 = 20,
        汇总柜组消费积分 = 21,
        储值卡日报表 = 22,
        库存储值卡保管帐 = 23,
        储值卡消费日报表 = 24,
        按商品汇总促销优惠券金额 = 25,
        按部门汇总促销优惠券金额 = 26,
        按合同汇总促销优惠券金额 = 27,
        汇总满百减折数据 = 38,
        将消费数据移动到会员消费纪录中 = 29,
        将消费数据删除 = 30,

    }

    public enum 加密方式     //第三方支付加密方式
    {
        MD5 = 1,
        RSA = 2
    }
    public enum 布局图状态
    {
        未审核 = 1,
        审核 = 3,
        终止 = 4,
        作废 = 5
    }
    public enum 列表或汇总
    {
        普通列表 = 1,
        期间汇总 = 2
    }
    public enum 是否已付清
    {
        已付清 = 1,
        未付清 = 4
    }
    public enum 租金收取方式
    {
        提成租金 = 1,
        固定租金 = 2
    }
    public enum 发票类型
    {
        百元票 = 1,
        千元票 = 2,
        万元票 = 3,
        十万元票 = 4,
        百万元票 = 5,
        千万元票 = 6
    }


    /// <summary>
    /// 暂时使用未来改成从一个固定表里面获取
    /// </summary>
    public enum 审批流程菜单号
    {
        租赁合同 = 10600200
    }

    public enum 审批流程节点类型
    {
        开始 = 1,
        表决 = 2,
        结束 = 3
    }
}
