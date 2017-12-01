/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:27
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MERCHANT", "")]
    public partial class MERCHANTEntity : EntityBase
    {
        public MERCHANTEntity()
        {
        }

        public MERCHANTEntity(string merchantid)
        {
            MERCHANTID = merchantid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SH
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string BANK
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string BANK_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ADRESS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTACTPERSON
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PHONE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PIZ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string WEIXIN
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string QQ
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
