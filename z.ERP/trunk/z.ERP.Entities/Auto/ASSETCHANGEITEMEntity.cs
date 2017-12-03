/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:48
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ASSETCHANGEITEM", "")]
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
        /// 
        /// <summary>
        [PrimaryKey]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ASSETID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ASSET_TYPE_OLD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ASSET_TYPE_NEW
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_BUILD_OLD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_BUILD_NEW
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_USABLE_OLD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_USABLE_NEW
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_RENTABLE_OLD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_RENTABLE_NEW
        {
            get; set;
        }
    }
}
