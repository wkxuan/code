/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:08
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT", "合同")]
    public partial class CONTRACTEntity : TableEntityBase
    {
        public CONTRACTEntity()
        {
        }

        public CONTRACTEntity(string contractid)
        {
            CONTRACTID = contractid;
        }

        /// <summary>
        /// 合同号
        /// <summary>
        [PrimaryKey]
        [Field("合同号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 原合同号
        /// <summary>
        [Field("原合同号")]
        public string CONTRACT_OLD
        {
            get; set;
        }
        /// <summary>
        /// 核算方式
        /// <summary>
        [Field("核算方式")]
        public string STYLE
        {
            get; set;
        }
        /// <summary>
        /// 合同类型
        /// <summary>
        [Field("合同类型")]
        public string HTLX
        {
            get; set;
        }
        /// <summary>
        /// 纸质合同号
        /// <summary>
        [Field("纸质合同号")]
        public string CONTRACTID_PAPER
        {
            get; set;
        }
        /// <summary>
        /// 招商部门
        /// <summary>
        [Field("招商部门")]
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 合同开始日期
        /// <summary>
        [Field("合同开始日期")]
        [DbType(DbType.DateTime)]
        public string CONT_START
        {
            get; set;
        }
        /// <summary>
        /// 合同结束日期
        /// <summary>
        [Field("合同结束日期")]
        [DbType(DbType.DateTime)]
        public string CONT_END
        {
            get; set;
        }
        /// <summary>
        /// 起租日期
        /// <summary>
        [Field("起租日期")]
        [DbType(DbType.DateTime)]
        public string QZRQ
        {
            get; set;
        }
        /// <summary>
        /// 租用面积
        /// <summary>
        [Field("租用面积")]
        public string AREAR
        {
            get; set;
        }
        /// <summary>
        /// 建筑面积
        /// <summary>
        [Field("建筑面积")]
        public string AREA_BUILD
        {
            get; set;
        }
        /// <summary>
        /// 进项税率
        /// <summary>
        [Field("进项税率")]
        public string JXSL
        {
            get; set;
        }
        /// <summary>
        /// 销项税率
        /// <summary>
        [Field("销项税率")]
        public string XXSL
        {
            get; set;
        }
        /// <summary>
        /// 商场编号
        /// <summary>
        [Field("商场编号")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 商户代码
        /// <summary>
        [Field("商户代码")]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 租金收费规则
        /// <summary>
        [Field("租金收费规则")]
        public string FEERULE_RENT
        {
            get; set;
        }
        /// <summary>
        /// 物业费收费规则
        /// <summary>
        [Field("物业费收费规则")]
        public string FEERULE_WYF
        {
            get; set;
        }
        /// <summary>
        /// 租金滞纳规则
        /// <summary>
        [Field("租金滞纳规则")]
        public string ZNID_RENT
        {
            get; set;
        }
        /// <summary>
        /// 物业费滞纳规则
        /// <summary>
        [Field("物业费滞纳规则")]
        public string ZNID_WYF
        {
            get; set;
        }
        /// <summary>
        /// 合作方式
        /// <summary>
        [Field("合作方式")]
        public string OPERATERULE
        {
            get; set;
        }
        /// <summary>
        /// 标准周期(季度周期还是合同开始日周期,增加系统参数然后可以修改)
        /// <summary>
        [Field("标准周期(季度周期还是合同开始日周期,增加系统参数然后可以修改)")]
        public string STANDARD
        {
            get; set;
        }
        /// <summary>
        /// 其他条款
        /// <summary>
        [Field("其他条款")]
        public string DESCRIPTION
        {
            get; set;
        }
        /// <summary>
        /// 录入员
        /// <summary>
        [Field("录入员")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 录入员名称
        /// <summary>
        [Field("录入员名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 录入时间
        /// <summary>
        [Field("录入时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 确认人
        /// <summary>
        [Field("确认人")]
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 确认人名称
        /// <summary>
        [Field("确认人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 确认时间
        /// <summary>
        [Field("确认时间")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
        /// <summary>
        /// 审批人
        /// <summary>
        [Field("审批人")]
        public string APPROVE
        {
            get; set;
        }
        /// <summary>
        /// 审批人名称
        /// <summary>
        [Field("审批人名称")]
        public string APPROVE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 审批时间
        /// <summary>
        [Field("审批时间")]
        [DbType(DbType.DateTime)]
        public string APPROVE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 启动人
        /// <summary>
        [Field("启动人")]
        public string INITINATE
        {
            get; set;
        }
        /// <summary>
        /// 启动人名称
        /// <summary>
        [Field("启动人名称")]
        public string INITINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 启动时间
        /// <summary>
        [Field("启动时间")]
        [DbType(DbType.DateTime)]
        public string INITINATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 终止人
        /// <summary>
        [Field("终止人")]
        public string TERMINATE
        {
            get; set;
        }
        /// <summary>
        /// 终止人名称
        /// <summary>
        [Field("终止人名称")]
        public string TERMINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 终止时间
        /// <summary>
        [Field("终止时间")]
        [DbType(DbType.DateTime)]
        public string TERMINATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 状态
        /// <summary>
        [Field("状态")]
        public string STATUS
        {
            get; set;
        }

        /// <summary>
        /// 起始日清算
        /// <summary>
        [Field("起始日清算")]
        public string QS_START
        {
            get; set;
        }

        /// <summary>
        /// 销售额标记
        /// <summary>
        [Field("销售额标记")]
        public string TAB_FLAG
        {
            get; set;
        }

        [Field("合同员id")]
        public string SIGNER
        {
            get;set;
        }
        [Field("合同员名称")]
        public string SIGNER_NAME
        {
            get;set;
        }


    }
}
