/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:19
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("USERMODULE", "用户模块")]
    public partial class USERMODULEEntity : TableEntityBase
    {
        public USERMODULEEntity()
        {
        }

        public USERMODULEEntity(string moduleid)
        {
            MODULEID = moduleid;
        }

        /// <summary>
        /// 模块编号
        /// <summary>
        [PrimaryKey]
        [Field("模块编号")]
        public string MODULEID
        {
            get; set;
        }
        /// <summary>
        /// 模块代码
        /// <summary>
        [Field("模块代码")]
        public string MODULECODE
        {
            get; set;
        }
        /// <summary>
        /// 模块名称
        /// <summary>
        [Field("模块名称")]
        public string MODULENAME
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
        /// <summary>
        /// 启用标记
        /// <summary>
        [Field("启用标记")]
        public string ENABLE_FLAG
        {
            get; set;
        }
        public string PMODULEID
        {
            get; set;
        }
        public string INX
        {
            get; set;
        }
        public string ICON
        {
            get; set;
        }
    }
}
