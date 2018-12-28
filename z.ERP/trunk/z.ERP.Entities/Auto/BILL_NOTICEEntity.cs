/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:05
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_NOTICE", "租赁通知单")]
    public partial class BILL_NOTICEEntity : TableEntityBase
    {
        public BILL_NOTICEEntity()
        {
        }

        public BILL_NOTICEEntity(string billid)
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
        /// 租约
        /// <summary>
        [Field("租约")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 年月
        /// <summary>
        [Field("年月")]
        public string NIANYUE
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
