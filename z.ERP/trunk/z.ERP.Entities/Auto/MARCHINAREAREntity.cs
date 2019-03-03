using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("MARCHINAREAR", "商户进场管理单据")]
    public partial class MARCHINAREAREntity : TableEntityBase
    {
        public MARCHINAREAREntity()
        {
        }

        public MARCHINAREAREntity(string billid)
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
        /// 租约号
        /// <summary>
        [Field("租约号")]
        public string CONTRACTID
        {
            get; set;
        }        
        /// <summary>
        /// 进场日期
        /// <summary>
        [Field("进场日期")]
        [DbType(DbType.DateTime)]
        public string MARCHINDATE
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
        /// 审核时间
        /// <summary>
        [Field("审核时间")]
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
    }
}
