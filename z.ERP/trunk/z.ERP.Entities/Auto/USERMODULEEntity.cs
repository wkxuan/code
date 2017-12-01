/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:28
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("USERMODULE", "")]
    public partial class USERMODULEEntity : EntityBase
    {
        public USERMODULEEntity()
        {
        }

        public USERMODULEEntity(string moduleid)
        {
            MODULEID = moduleid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string MODULEID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MODULECODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MODULENAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MENUID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ENABLE_FLAG
        {
            get; set;
        }
    }
}
