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
    [DbTable("ROLE_MENU", "")]
    public partial class ROLE_MENUEntity : EntityBase
    {
        public ROLE_MENUEntity()
        {
        }

        public ROLE_MENUEntity(string roleid, string moduleid, string menuid)
        {
            ROLEID = roleid;
            MODULEID = moduleid;
            MENUID = menuid;
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
        [PrimaryKey]
        public string MODULEID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string MENUID
        {
            get; set;
        }
    }
}
