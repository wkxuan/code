/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:13
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("GOODS_KIND", "商品分类")]
    public partial class GOODS_KINDEntity : TableEntityBase
    {
        public GOODS_KINDEntity()
        {
        }

        public GOODS_KINDEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// ID
        /// <summary>
        [PrimaryKey]
        [Field("ID")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 代码
        /// <summary>
        [Field("代码")]
        public string CODE
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// <summary>
        [Field("名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 末级标记
        /// <summary>
        [Field("末级标记")]
        public string LAST_BJ
        {
            get; set;
        }
    }
}
