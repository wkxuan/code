using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WLSETTLE", "物料结算单WLSETTLE")]
    public partial class WLSETTLEEntity : TableEntityBase
    {
        public WLSETTLEEntity()
        {
        }

        public WLSETTLEEntity(string billid)
        {
            BILLID = billid;
        }

        [PrimaryKey]
        [Field("物料结算单单号ID")]
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
        [Field("供应商代码")]
        public string MERCHANTID
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
        /// 审核时间
        /// <summary>
        [Field("审核时间")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
    }
}
