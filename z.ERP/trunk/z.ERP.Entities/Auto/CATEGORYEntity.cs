/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:07
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CATEGORY", "业态")]
    public partial class CATEGORYEntity : TableEntityBase
    {
        public CATEGORYEntity()
        {
        }

        public CATEGORYEntity(string categoryid)
        {
            CATEGORYID = categoryid;
        }

        /// <summary>
        /// 业态编码
        /// <summary>
        [PrimaryKey]
        [Field("业态编码")]
        public string CATEGORYID
        {
            get; set;
        }
        /// <summary>
        /// 业态代码
        /// <summary>
        [Field("业态代码")]
        public string CATEGORYCODE
        {
            get; set;
        }
        /// <summary>
        /// 业态名称
        /// <summary>
        [Field("业态名称")]
        public string CATEGORYNAME
        {
            get; set;
        }
        /// <summary>
        /// 末级标志
        /// <summary>
        [Field("末级标志")]
        public string LEVEL_LAST
        {
            get; set;
        }
        /// <summary>
        /// 父级
        /// <summary>
        [Field("父级节点")]
        public string CATEGORYIDCASCADER
        {
            get; set;
        }
    }
}
