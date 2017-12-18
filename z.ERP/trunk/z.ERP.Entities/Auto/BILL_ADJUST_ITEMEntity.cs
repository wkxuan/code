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
    [DbTable("BILL_ADJUST_ITEM", "账单调整单子表")]
    public partial class BILL_ADJUST_ITEMEntity : EntityBase
    {
        public BILL_ADJUST_ITEMEntity()
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
        /// 调整金额
        /// <summary>
        [Field("调整金额")]
        public string MUST_MONEY
        {
            get; set;
        }
        /// <summary>
        /// 账单编号
        /// <summary>
        [Field("账单编号")]
        public string FINAL_BILLID
        {
            get; set;
        }
    }
}
