/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 23:51:17
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MENU", "")]
    public partial class MENUEntity : EntityBase
    {
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
