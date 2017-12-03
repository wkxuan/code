/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:49
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_ADJUST_ITEM", "")]
    public partial class BILL_ADJUST_ITEMEntity : EntityBase
    {
        public BILL_ADJUST_ITEMEntity()
        {
        }

        public BILL_ADJUST_ITEMEntity(string billid, string contractid, string termid)
        {
            BILLID = billid;
            CONTRACTID = contractid;
            TERMID = termid;
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
        [PrimaryKey]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string TERMID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MUST_MONEY
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FINAL_BILLID
        {
            get; set;
        }
    }
}
