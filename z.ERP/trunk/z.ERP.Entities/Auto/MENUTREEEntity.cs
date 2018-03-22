/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:15
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MENUTREE", "")]
    public partial class MENUTREEEntity : TableEntityBase
    {
        public MENUTREEEntity()
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
        public string PID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MENUID
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
    }
}
