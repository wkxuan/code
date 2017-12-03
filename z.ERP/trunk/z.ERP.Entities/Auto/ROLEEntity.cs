/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:59:01
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE", "")]
    public partial class ROLEEntity : EntityBase
    {
        public ROLEEntity()
        {
        }

        public ROLEEntity(string roleid)
        {
            ROLEID = roleid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ROLEID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ROLECODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ROLENAME
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
        /// <summary>
        /// 
        /// <summary>
        public string VOID_FLAG
        {
            get; set;
        }
    }
}
