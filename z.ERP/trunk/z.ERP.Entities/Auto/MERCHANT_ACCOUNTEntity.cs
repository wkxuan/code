using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("MERCHANT_ACCOUNT", "商户预收款余额表")]
    public partial class MERCHANT_ACCOUNTEntity
    {
        public MERCHANT_ACCOUNTEntity() {

        }
        public MERCHANT_ACCOUNTEntity(string merchantid,string fee_account_id)
        {

        }
        /// <summary>
        /// 商户代码
        /// <summary>
        [PrimaryKey]
        [Field("商户代码")]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 核销单位代码
        /// <summary>
        [Field("核销单位代码")]
        public string FEE_ACCOUNT_ID
        {
            get; set;
        }
        /// <summary>
        /// 商户预售款余额
        /// <summary>
        [Field("商户预售款余额")]
        public string BALANCE
        {
            get; set;
        }
        /// <summary>
        /// 累计已使用金额
        /// <summary>
        [Field("累计已使用金额")]
        public string USED_MONEY
        {
            get; set;
        }
    }
}
