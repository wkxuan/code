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
    [DbTable("BRANCH", "")]
    public partial class BRANCHEntity : EntityBase
    {
        public BRANCHEntity()
        {
        }

        public BRANCHEntity(string id, string validity_start)
        {
            ID = id;
            VALIDITY_START = validity_start;
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
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_BUILD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_USABLE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_RENTABLE
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
        public string ADDRESS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTACT
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTACT_NUM
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VERSIONID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        [DbType(DbType.DateTime)]
        public string VALIDITY_START
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string VALIDITY_END
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PRINTNAME
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
