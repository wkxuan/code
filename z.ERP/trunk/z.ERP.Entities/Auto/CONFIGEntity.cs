/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 23:51:15
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONFIG", "")]
    public partial class CONFIGEntity : EntityBase
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
        public string DEF_VAL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CUR_VAL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MAX_VAL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MIN_VAL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
