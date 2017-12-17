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
    [DbTable("BILL_NOTICE_ITEM", "通知单子表")]
    public partial class BILL_NOTICE_ITEMEntity : EntityBase
    {
        public BILL_NOTICE_ITEMEntity()
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
        /// 账单单号
        /// <summary>
        [Field("账单单号")]
        public string FINAL_BILLID
        {
            get; set;
        }
        /// <summary>
        /// 金额
        /// <summary>
        [Field("金额")]
        public string NOTICE_MONEY
        {
            get; set;
        }
    }
}
