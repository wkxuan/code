/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/5 22:24:36
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MENUTREE", "")]
    public partial class MENUTREEEntity : EntityBase
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
