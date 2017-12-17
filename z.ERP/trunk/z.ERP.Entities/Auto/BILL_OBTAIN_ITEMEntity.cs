/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:18
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_OBTAIN_ITEM", "账单付款核销付款单子表")]
    public partial class BILL_OBTAIN_ITEMEntity : EntityBase
    {
        public BILL_OBTAIN_ITEMEntity()
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
        /// 类型(0租赁账单1联营结算单)
        /// <summary>
        [Field("类型(0租赁账单1联营结算单)")]
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 账单单号
        /// <summary>
        [Field("账单单号")]
        public string FINAL_BILLID
        {
            get; set;
        }
        /// <summary>
        /// 应付金额
        /// <summary>
        [Field("应付金额")]
        public string MUST_MONEY
        {
            get; set;
        }
        /// <summary>
        /// 付款金额
        /// <summary>
        [Field("付款金额")]
        public string RECEIVE_MONEY
        {
            get; set;
        }
    }
}
