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
    [DbTable("ORG", "")]
    public partial class ORGEntity : EntityBase
    {
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ORGCODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ORGNAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ORG_TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string LEVEL_LAST
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string CREATE_TIME
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
        [DbType(DbType.DateTime)]
        public string UPDATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VOID_FLAG
        {
            get; set;
        }
    }
}
