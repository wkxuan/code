/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 23:51:17
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MERCHANT", "")]
    public partial class MERCHANTEntity : EntityBase
    {
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
