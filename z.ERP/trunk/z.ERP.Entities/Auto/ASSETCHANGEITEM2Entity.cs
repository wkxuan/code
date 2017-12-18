/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:11:59
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ASSETCHANGEITEM2", "资产调整项目2")]
    public partial class ASSETCHANGEITEM2Entity : EntityBase
    {
        public ASSETCHANGEITEM2Entity()
        {
        }

        /// <summary>
        /// 序号
        /// <summary>
        [Field("序号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 原资产编号
        /// <summary>
        [Field("原资产编号")]
        public string ASSETID_OLD
        {
            get; set;
        }
        /// <summary>
        /// 新资产编号
        /// <summary>
        [Field("新资产编号")]
        public string ASSETID_NEW
        {
            get; set;
        }
        /// <summary>
        /// 新资产代码
        /// <summary>
        [Field("新资产代码")]
        public string ASSETCODE_NEW
        {
            get; set;
        }
        /// <summary>
        /// 建筑面积
        /// <summary>
        [Field("建筑面积")]
        public string AREA_BUILD_NEW
        {
            get; set;
        }
        /// <summary>
        /// 可使用面积
        /// <summary>
        [Field("可使用面积")]
        public string AREA_USABLE_NEW
        {
            get; set;
        }
        /// <summary>
        /// 可租赁面积
        /// <summary>
        [Field("可租赁面积")]
        public string AREA_RENTABLE_NEW
        {
            get; set;
        }
    }
}
