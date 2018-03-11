/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:18:57
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_RETURN_ITEM", "保证金返还单子表")]
    public partial class BILL_RETURN_ITEMEntity : EntityBase
    {
        public BILL_RETURN_ITEMEntity()
        {
        }

        public BILL_RETURN_ITEMEntity(string billid, string final_billid)
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
        /// 账单编号
        /// <summary>
        [PrimaryKey]
        [Field("账单编号")]
        public string FINAL_BILLID
        {
            get; set;
        }
        /// <summary>
        /// 返还金额
        /// <summary>
        [Field("返还金额")]
        public string RETURN_MONEY
        {
            get; set;
        }
    }
}
