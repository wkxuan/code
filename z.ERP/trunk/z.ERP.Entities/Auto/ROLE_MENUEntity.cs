/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:22
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
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

        /// <summary>
        /// 角色编码
        /// <summary>
        [Field("角色编码")]
        public string ROLEID
        {
            get; set;
        }
        /// <summary>
        /// 位置编号
        /// <summary>
        [Field("位置编号")]
        public string MODULEID
        {
            get; set;
        }
        /// <summary>
        /// 菜单编号
        /// <summary>
        [Field("菜单编号")]
        public string MENUID
        {
            get; set;
        }
    }
}
