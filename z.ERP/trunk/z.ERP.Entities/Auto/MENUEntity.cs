/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:15
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("MENU", "")]
    public partial class MENUEntity : TableEntityBase
    {
        public MENUEntity()
        {
        }

        /// <summary>
        /// 
        /// <summary>
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string URL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PLATFORMID
        {
            get; set;
        }
    }
}
