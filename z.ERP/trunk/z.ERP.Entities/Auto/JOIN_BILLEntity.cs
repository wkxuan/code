/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:03
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("JOIN_BILL", "联营结算单")]
    public partial class JOIN_BILLEntity : EntityBase
    {
        public JOIN_BILLEntity()
        {
        }

        /// <summary>
        /// 单号
        /// <summary>
        [Field("单号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 分店ID
        /// <summary>
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 租约号
        /// <summary>
        [Field("租约号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 商户
        /// <summary>
        [Field("商户")]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 年月
        /// <summary>
        [Field("年月")]
        public string NIANYUE
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [Field("开始日期")]
        [DbType(DbType.DateTime)]
        public string START_DATE
        {
            get; set;
        }
        /// <summary>
        /// 结束日期
        /// <summary>
        [Field("结束日期")]
        [DbType(DbType.DateTime)]
        public string END_DATE
        {
            get; set;
        }
        /// <summary>
        /// 类型
        /// <summary>
        [Field("类型")]
        public string TYPE
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
        /// 金额_17
        /// <summary>
        [Field("金额_17")]
        public string JE_17
        {
            get; set;
        }
        /// <summary>
        /// 金额_11
        /// <summary>
        [Field("金额_11")]
        public string JE_11
        {
            get; set;
        }
        /// <summary>
        /// 金额_其他
        /// <summary>
        [Field("金额_其他")]
        public string JE_QT
        {
            get; set;
        }
        /// <summary>
        /// 税额_17
        /// <summary>
        [Field("税额_17")]
        public string ZZSJE_17
        {
            get; set;
        }
        /// <summary>
        /// 税额_11
        /// <summary>
        [Field("税额_11")]
        public string ZZSJE_11
        {
            get; set;
        }
        /// <summary>
        /// 税额_其他
        /// <summary>
        [Field("税额_其他")]
        public string ZZSJE_QT
        {
            get; set;
        }
        /// <summary>
        /// 销售金额
        /// <summary>
        [Field("销售金额")]
        public string SELL_JE
        {
            get; set;
        }
        /// <summary>
        /// 销售成本
        /// <summary>
        [Field("销售成本")]
        public string SELL_COST
        {
            get; set;
        }
        /// <summary>
        /// 扣款金额
        /// <summary>
        [Field("扣款金额")]
        public string KKJE
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// <summary>
        [Field("描述")]
        public string DESCRIPTION
        {
            get; set;
        }
        /// <summary>
        /// 登记人
        /// <summary>
        [Field("登记人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 登记人名称
        /// <summary>
        [Field("登记人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 登记时间
        /// <summary>
        [Field("登记时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 审核人
        /// <summary>
        [Field("审核人")]
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 审核时间
        /// <summary>
        [Field("审核时间")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
        /// <summary>
        /// 审核人名称
        /// <summary>
        [Field("审核人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
    }
}
