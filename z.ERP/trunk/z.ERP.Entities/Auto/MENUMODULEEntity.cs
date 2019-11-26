using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("MENUMODULE", "模块菜单")]
    public partial class MENUMODULEEntity : TableEntityBase
    {
        public MENUMODULEEntity()
        {
        }

        public MENUMODULEEntity(string moduleid)
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
        public string ICON
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
        public string TYPE
        {
            get; set;
        }
    }
}
