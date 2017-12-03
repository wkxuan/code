/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:59
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MENU", "")]
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
        /// 
        /// <summary>
        [PrimaryKey]
        public string MENUID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MENUCODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MENUNAME
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
