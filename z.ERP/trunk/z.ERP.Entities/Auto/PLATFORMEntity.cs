/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/7/17 23:59:28
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PLATFORM", "")]
    public partial class PLATFORMEntity : TableEntityBase
    {
        public PLATFORMEntity()
        {
        }

        public PLATFORMEntity(string id, string match)
        {
            ID = id;
            MATCH = match;
        }

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
        public string DOMAIN
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string MATCH
        {
            get; set;
        }
    }
}
