/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:05
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_NOTICE_ITEM", "通知单子表")]
    public partial class BILL_NOTICE_ITEMEntity : TableEntityBase
    {
        public BILL_NOTICE_ITEMEntity()
        {
        }

        public BILL_NOTICE_ITEMEntity(string billid, string final_billid)
        {
            BILLID = billid;
            FINAL_BILLID = final_billid;
        }

        /// <summary>
        /// 单号
        /// <summary>
        [PrimaryKey]
        [Field("单号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 账单单号
        /// <summary>
        [PrimaryKey]
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
