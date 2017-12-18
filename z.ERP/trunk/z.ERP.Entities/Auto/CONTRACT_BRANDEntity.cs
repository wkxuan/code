/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:01
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_BRAND", "合同品牌")]
    public partial class CONTRACT_BRANDEntity : EntityBase
    {
        public CONTRACT_BRANDEntity()
        {
        }

        /// <summary>
        /// 合同号
        /// <summary>
        [Field("合同号")]
        public string CONTRACTID
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
        /// <summary>
        /// 分摊比例
        /// <summary>
        [Field("分摊比例")]
        public string FTBL
        {
            get; set;
        }
    }
}
