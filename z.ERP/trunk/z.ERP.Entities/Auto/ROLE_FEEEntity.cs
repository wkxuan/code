/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 23:51:18
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_FEE", "")]
    public partial class ROLE_FEEEntity : EntityBase
    {
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ROLEID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string TRIMID
        {
            get; set;
        }
    }
}
