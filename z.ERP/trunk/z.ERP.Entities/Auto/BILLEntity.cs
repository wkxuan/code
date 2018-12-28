/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:04
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL", "账单BILL")]
    public partial class BILLEntity : TableEntityBase
    {
        public BILLEntity()
        {
        }

        public BILLEntity(string billid)
        {
            BILLID = billid;
        }

        /// <summary>
        /// 账单ID
        /// <summary>
        [PrimaryKey]
        [Field("账单ID")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 卖场ID
        /// <summary>
        [Field("卖场ID")]
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
        /// 租约号
        /// <summary>
        [Field("租约号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 项目ID
        /// <summary>
        [Field("项目ID")]
        public string TERMID
        {
            get; set;
        }
        /// <summary>
        /// 权责发生月
        /// <summary>
        [Field("权责发生月")]
        public string NIANYUE
        {
            get; set;
        }
        /// <summary>
        /// 收付实现月
        /// <summary>
        [Field("收付实现月")]
        public string YEARMONTH
        {
            get; set;
        }
        /// <summary>
        /// 应收金额
        /// <summary>
        [Field("应收金额")]
        public string MUST_MONEY
        {
            get; set;
        }
        /// <summary>
        /// 已收金额
        /// <summary>
        [Field("已收金额")]
        public string RECEIVE_MONEY
        {
            get; set;
        }
        /// <summary>
        /// 返还金额
        /// <summary>
        [Field("返还金额")]
        public string RRETURN_MONEY
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
        /// 登记人时间
        /// <summary>
        [Field("登记人时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
    }
}
