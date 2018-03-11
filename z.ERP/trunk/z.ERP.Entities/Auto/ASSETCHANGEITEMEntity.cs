/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:18:56
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ASSETCHANGEITEM", "资产调整项目")]
    public partial class ASSETCHANGEITEMEntity : EntityBase
    {
        public ASSETCHANGEITEMEntity()
        {
        }

        public ASSETCHANGEITEMEntity(string billid, string assetid)
        {
            BILLID = billid;
            ASSETID = assetid;
        }

        /// <summary>
        /// 新可使用面积
        /// <summary>
        [Field("新可使用面积")]
        public string AREA_USABLE_NEW
        {
            get; set;
        }
        /// <summary>
        /// 新可租赁面积
        /// <summary>
        [Field("新可租赁面积")]
        public string AREA_RENTABLE_NEW
        {
            get; set;
        }
        /// <summary>
        /// 序号
        /// <summary>
        [PrimaryKey]
        [Field("序号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 资产编号
        /// <summary>
        [PrimaryKey]
        [Field("资产编号")]
        public string ASSETID
        {
            get; set;
        }
        /// <summary>
        /// 原资产类型
        /// <summary>
        [Field("原资产类型")]
        public string ASSET_TYPE_OLD
        {
            get; set;
        }
        /// <summary>
        /// 新资产类型
        /// <summary>
        [Field("新资产类型")]
        public string ASSET_TYPE_NEW
        {
            get; set;
        }
        /// <summary>
        /// 原建筑面积
        /// <summary>
        [Field("原建筑面积")]
        public string AREA_BUILD_OLD
        {
            get; set;
        }
        /// <summary>
        /// 新建筑面积
        /// <summary>
        [Field("新建筑面积")]
        public string AREA_BUILD_NEW
        {
            get; set;
        }
        /// <summary>
        /// 原可使用面积
        /// <summary>
        [Field("原可使用面积")]
        public string AREA_USABLE_OLD
        {
            get; set;
        }
        /// <summary>
        /// 原可租赁面积
        /// <summary>
        [Field("原可租赁面积")]
        public string AREA_RENTABLE_OLD
        {
            get; set;
        }
    }
}
