/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 23:51:16
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_UPDATE", "")]
    public partial class CONTRACT_UPDATEEntity : EntityBase
    {
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
        public string CONTRACTID_OLD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string INITINATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string INITINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string INITINATE_TIME
        {
            get; set;
        }
    }
}
