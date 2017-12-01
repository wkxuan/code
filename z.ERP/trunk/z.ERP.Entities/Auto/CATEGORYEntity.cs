/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:23
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CATEGORY", "")]
    public partial class CATEGORYEntity : EntityBase
    {
        public CATEGORYEntity()
        {
        }

        public CATEGORYEntity(string categoryid)
        {
            CATEGORYID = categoryid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CATEGORYID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CATEGORYCODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CATEGORYNAME
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
        [DbType(DbType.DateTime)]
        public string UPDATE_TIME
        {
            get; set;
        }
    }
}
