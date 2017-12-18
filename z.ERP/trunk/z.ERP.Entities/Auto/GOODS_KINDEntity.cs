/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:02
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("GOODS_KIND", "商品分类")]
    public partial class GOODS_KINDEntity : EntityBase
    {
        public GOODS_KINDEntity()
        {
        }

        /// <summary>
        /// ID
        /// <summary>
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
