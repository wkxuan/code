/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:22
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BRAND", "")]
    public partial class BRANDEntity : EntityBase
    {
        public BRANDEntity()
        {
        }

        public BRANDEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CODE
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
        public string CATEGORYID
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
        public string PHONENUM
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
