/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:19:02
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("USERS", "")]
    public partial class USERSEntity : EntityBase
    {
        public USERSEntity()
        {
        }

        /// <summary>
        /// 
        /// <summary>
        public string USERID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string USERCODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string USERNAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string USER_TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string USER_FLAG
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VOID_FLAG
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PASSWORD
        {
            get; set;
        }
    }
}
