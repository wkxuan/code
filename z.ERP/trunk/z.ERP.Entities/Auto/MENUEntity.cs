/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/19 20:30:50
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MENU", "菜单(通过代码长度代表菜单与子菜单)")]
    public partial class MENUEntity : EntityBase
    {
        public MENUEntity()
        {
        }

        public MENUEntity(string menuid)
        {
            MENUID = menuid;
        }

        /// <summary>
        /// 菜单代码
        /// <summary>
        [Field("菜单代码")]
        public string MENUCODE
        {
            get; set;
        }
        /// <summary>
        /// 菜单名称
        /// <summary>
        [Field("菜单名称")]
        public string MENUNAME
        {
            get; set;
        }
        /// <summary>
        /// 启用标记
        /// <summary>
        [Field("启用标记")]
        public string ENABLE_FLAG
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
