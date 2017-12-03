/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:58
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("GOODS_SHOP", "")]
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
        /// 
        /// <summary>
        [PrimaryKey]
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CATEGORYID
        {
            get; set;
        }
    }
}
