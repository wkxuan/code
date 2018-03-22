/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:18
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_MENU", "角色_菜单")]
    public partial class ROLE_MENUEntity : TableEntityBase
    {
        public ROLE_MENUEntity()
        {
        }

        public ROLE_MENUEntity(string roleid, string modulecode, string menuid)
        {
            ROLEID = roleid;
            MODULECODE = modulecode;
            MENUID = menuid;
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
        /// 位置代码
        /// <summary>
        [PrimaryKey]
        [Field("位置代码")]
        public string MODULECODE
        {
            get; set;
        }
        /// <summary>
        /// 菜单编号
        /// <summary>
        [PrimaryKey]
        [Field("菜单编号")]
        public string MENUID
        {
            get; set;
        }
    }
}
