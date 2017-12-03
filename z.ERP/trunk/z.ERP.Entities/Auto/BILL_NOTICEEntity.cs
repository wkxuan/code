/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:50
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_NOTICE", "")]
    public partial class BILL_NOTICEEntity : EntityBase
    {
        public BILL_NOTICEEntity()
        {
        }

        public BILL_NOTICEEntity(string billid)
        {
            BILLID = billid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NIANYUE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DESCRIPTION
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
    }
}
