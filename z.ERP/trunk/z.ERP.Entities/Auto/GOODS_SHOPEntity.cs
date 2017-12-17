/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:21
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("GOODS_SHOP", "商品店铺")]
    public partial class GOODS_SHOPEntity : EntityBase
    {
        public GOODS_SHOPEntity()
        {
        }

        /// <summary>
        /// 商品代码
        /// <summary>
        [Field("商品代码")]
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 分店ID
        /// <summary>
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 店铺ID
        /// <summary>
        [Field("店铺ID")]
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 业态ID
        /// <summary>
        [Field("业态ID")]
        public string CATEGORYID
        {
            get; set;
        }
    }
}
