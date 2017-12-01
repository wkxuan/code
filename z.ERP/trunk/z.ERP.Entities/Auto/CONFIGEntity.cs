/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:23
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONFIG", "")]
    public partial class CONFIGEntity : EntityBase
    {
        public CONFIGEntity()
        {
        }

        public CONFIGEntity(string id)
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
