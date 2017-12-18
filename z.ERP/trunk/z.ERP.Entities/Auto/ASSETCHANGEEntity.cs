/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:11:59
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ASSETCHANGE", "资产调整")]
    public partial class ASSETCHANGEEntity : EntityBase
    {
        public ASSETCHANGEEntity()
        {
        }

        /// <summary>
        /// 序号
        /// <summary>
        [Field("序号")]
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
        /// 变更类型
        /// <summary>
        [Field("变更类型")]
        public string CHANGE_TYPE
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// <summary>
        [Field("备注")]
        public string DESCRIPTION
        {
            get; set;
        }
        /// <summary>
        /// 制单人
        /// <summary>
        [Field("制单人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 制单人名称
        /// <summary>
        [Field("制单人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 制单日期
        /// <summary>
        [Field("制单日期")]
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
        /// 审核人名称
        /// <summary>
        [Field("审核人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 审核日期
        /// <summary>
        [Field("审核日期")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
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
    }
}
