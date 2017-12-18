/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:03
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
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

        /// <summary>
        /// 商户代码
        /// <summary>
        [Field("商户代码")]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 品牌
        /// <summary>
        [Field("品牌")]
        public string BRANDID
        {
            get; set;
        }
    }
}
