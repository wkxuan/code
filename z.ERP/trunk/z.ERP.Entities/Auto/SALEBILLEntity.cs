using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("SALEBILL", "销售补录单")]
    public partial class SALEBILLEntity : TableEntityBase
    {
        public SALEBILLEntity()
        {
        }

        public SALEBILLEntity(string billid)
        {
            BILLID = billid;
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
        /// 分店ID
        /// <summary>
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// POS终端号
        /// <summary>
        [Field("POS终端号")]
        public string POSNO
        {
            get; set;
        }
        /// <summary>
        /// 退铺日期
        /// <summary>
        [Field("记账日期")]
        [DbType(DbType.DateTime)]
        public string ACCOUNT_DATE
        {
            get; set;
        }
        /// <summary>
        /// 收银员
        /// <summary>
        [Field("收银员")]
        public string CASHIERID
        {
            get; set;
        }
        /// <summary>
        /// 营业员
        /// <summary>
        [Field("营业员")]
        public string CLERKID
        {
            get; set;
        }
        /// <summary>
        /// 交易流水号
        /// <summary>
        [Field("交易流水号")]
        public string DEALID
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
