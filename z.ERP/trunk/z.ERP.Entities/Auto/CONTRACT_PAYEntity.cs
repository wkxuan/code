/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:53
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_PAY", "")]
    public partial class CONTRACT_PAYEntity : EntityBase
    {
        public CONTRACT_PAYEntity()
        {
        }

        public CONTRACT_PAYEntity(string contractid, string payid, string termid, string startdate)
        {
            CONTRACTID = contractid;
            PAYID = payid;
            TERMID = termid;
            STARTDATE = startdate;
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
        public string PAYID
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
        [PrimaryKey]
        [DbType(DbType.DateTime)]
        public string STARTDATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string ENDDATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string KL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZNGZID
        {
            get; set;
        }
    }
}
