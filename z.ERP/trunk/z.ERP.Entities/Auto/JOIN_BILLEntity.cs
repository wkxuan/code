/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:58
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("JOIN_BILL", "")]
    public partial class JOIN_BILLEntity : EntityBase
    {
        public JOIN_BILLEntity()
        {
        }

        public JOIN_BILLEntity(string billid)
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
        public string MERCHANTID
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
        public string TYPE
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
        public string JE_17
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JE_11
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JE_QT
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZZSJE_17
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZZSJE_11
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZZSJE_QT
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SELL_JE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SELL_COST
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string KKJE
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
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
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
    }
}
