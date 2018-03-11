/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:19:01
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MERCHANT_BRAND", "商户品牌")]
    public partial class MERCHANT_BRANDEntity : EntityBase
    {
        public MERCHANT_BRANDEntity()
        {
        }

        public MERCHANT_BRANDEntity(string merchantid, string brandid)
        {
            MERCHANTID = merchantid;
            BRANDID = brandid;
        }

        /// <summary>
        /// 商户代码
        /// <summary>
        [PrimaryKey]
        [Field("商户代码")]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 品牌
        /// <summary>
        [PrimaryKey]
        [Field("品牌")]
        public string BRANDID
        {
            get; set;
        }
    }
}
