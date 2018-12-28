/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:10
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_STATUS", "")]
    public partial class CONTRACT_STATUSEntity : TableEntityBase
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
