/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:18:58
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
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

        public CONTRACT_SHOPEntity(string contractid, string shopid)
        {
            CONTRACTID = contractid;
            SHOPID = shopid;
        }

        /// <summary>
        /// 租用面积
        /// <summary>
        [Field("租用面积")]
        public string AREA_RENTABLE
        {
            get; set;
        }
        /// <summary>
        /// 合同号
        /// <summary>
        [PrimaryKey]
        [Field("合同号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 商铺ID
        /// <summary>
        [PrimaryKey]
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
        /// 建筑面积
        /// <summary>
        [Field("建筑面积")]
        public string AREA
        {
            get; set;
        }
    }
}
