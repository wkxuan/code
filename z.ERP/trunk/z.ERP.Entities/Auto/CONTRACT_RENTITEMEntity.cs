/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:54
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_RENTITEM", "")]
    public partial class CONTRACT_RENTITEMEntity : EntityBase
    {
        public CONTRACT_RENTITEMEntity()
        {
        }

        public CONTRACT_RENTITEMEntity(string contractid, string inx, string yearmonth, string startdate)
        {
            CONTRACTID = contractid;
            INX = inx;
            YEARMONTH = yearmonth;
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
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string YEARMONTH
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
        public string RENTS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string CREATEDATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string QSBJ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string BILLID
        {
            get; set;
        }
    }
}
