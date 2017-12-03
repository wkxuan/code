/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:56
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ENERGY_FILES", "")]
    public partial class ENERGY_FILESEntity : EntityBase
    {
        public ENERGY_FILESEntity()
        {
        }

        public ENERGY_FILESEntity(string fileid)
        {
            FILEID = fileid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string FILEID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FILECODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FILENAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREAID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string FACTORY_DATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VALUE_LAST
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string DATE_LAST
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MULTIPLE
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
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
