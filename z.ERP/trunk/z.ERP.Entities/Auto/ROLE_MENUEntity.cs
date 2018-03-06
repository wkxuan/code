﻿/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/5 22:24:38
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_MENU", "角色_菜单")]
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
        /// 角色编码
        /// <summary>
        [PrimaryKey]
        [Field("角色编码")]
        public string ROLEID
        {
            get; set;
        }
        /// <summary>
        /// 位置编号
        /// <summary>
        [PrimaryKey]
        [Field("位置编号")]
        public string MODULEID
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
