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
    [DbTable("CONTRACT_SHOP", "合同商铺")]
    public partial class CONTRACT_SHOPEntity : EntityBase
    {
        public CONTRACT_SHOPEntity()
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
        /// 商铺ID
        /// <summary>
        [Field("商铺ID")]
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
        /// <summary>
        /// 面积
        /// <summary>
        [Field("面积")]
        public string AREA
        {
            get; set;
        }
    }
}
