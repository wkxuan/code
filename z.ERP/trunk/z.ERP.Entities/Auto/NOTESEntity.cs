/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:16
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("NOTES", "")]
    public partial class NOTESEntity : TableEntityBase
    {
        public NOTESEntity()
        {
        }

        public NOTESEntity(string id)
        {
            ID = id;
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
        public string TKEY
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TVALUE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TITLE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NOTES
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CREATER
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string CREATED
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CREATENAME
        {
            get; set;
        }
    }
}
