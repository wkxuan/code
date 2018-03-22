/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:17
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE", "角色")]
    public partial class ROLEEntity : TableEntityBase
    {
        public ROLEEntity()
        {
        }

        public ROLEEntity(string roleid)
        {
            ROLEID = roleid;
        }

        /// <summary>
        /// 角色编码
        /// <summary>
        [PrimaryKey]
        [Field("角色编码")]
        public string ROLEID
        {
            get; set;
        }
        /// <summary>
        /// 角色代码
        /// <summary>
        [Field("角色代码")]
        public string ROLECODE
        {
            get; set;
        }
        /// <summary>
        /// 角色名称
        /// <summary>
        [Field("角色名称")]
        public string ROLENAME
        {
            get; set;
        }
        /// <summary>
        /// 角色机构
        /// <summary>
        [Field("角色机构")]
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 作废标记
        /// <summary>
        [Field("作废标记")]
        public string VOID_FLAG
        {
            get; set;
        }
    }
}
