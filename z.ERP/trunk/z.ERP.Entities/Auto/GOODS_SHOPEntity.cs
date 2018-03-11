/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:19:00
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
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

        public GOODS_SHOPEntity(string goodsid, string branchid, string shopid)
        {
            GOODSID = goodsid;
            BRANCHID = branchid;
            SHOPID = shopid;
        }

        /// <summary>
        /// 商品代码
        /// <summary>
        [PrimaryKey]
        [Field("商品代码")]
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 分店ID
        /// <summary>
        [PrimaryKey]
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 店铺ID
        /// <summary>
        [PrimaryKey]
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
