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
    [DbTable("LATEFEERULE", "")]
    public partial class LATEFEERULEEntity : EntityBase
    {
        public LATEFEERULEEntity()
        {
        }

        public LATEFEERULEEntity(string id)
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
        public string DAYS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AMOUNTS
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
