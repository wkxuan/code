/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:55
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_STATUS", "")]
    public partial class CONTRACT_STATUSEntity : EntityBase
    {
        public CONTRACT_STATUSEntity()
        {
        }

        public CONTRACT_STATUSEntity(string contractid, string start_date)
        {
            CONTRACTID = contractid;
            START_DATE = start_date;
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
        [DbType(DbType.DateTime)]
        public string START_DATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string END_DATE
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
    }
}
