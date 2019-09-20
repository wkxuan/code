using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PRESENT_SEND", "赠品发放")]
    public partial class PRESENT_SENDEntity: TableEntityBase
    {
        public PRESENT_SENDEntity() { }
        public PRESENT_SENDEntity(string id) {
            BILLID = id;
        }
        /// <summary>
        /// id
        /// <summary>
        [PrimaryKey]
        [Field("id")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 门店id
        /// <summary>
        [Field("门店id")]
        public string BRANCHID
        {
            get; set;
        }      
        /// <summary>
        /// 录入人
        /// <summary>
        [Field("录入人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 录入人名称
        /// <summary>
        [Field("录入人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 录入人时间
        /// <summary>
        [Field("录入人时间")]
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
        /// <summary>
        /// 状态
        /// <summary>
        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
    }
}
