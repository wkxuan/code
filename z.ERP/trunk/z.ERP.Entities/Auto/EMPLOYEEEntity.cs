/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/11/28 21:03:15
 * 生成人：书房
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("EMPLOYEE", "")]
    public partial class EMPLOYEEEntity : EntityBase
    {
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
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
        public string LOGINNAME
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
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
    }
}
