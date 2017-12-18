/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:03
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
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

        /// <summary>
        /// 菜单编号
        /// <summary>
        [Field("菜单编号")]
        public string MENUID
        {
            get; set;
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
    }
}
