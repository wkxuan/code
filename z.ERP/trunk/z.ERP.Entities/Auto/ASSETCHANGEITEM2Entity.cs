/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:20
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ASSETCHANGEITEM2", "")]
    public partial class ASSETCHANGEITEM2Entity : EntityBase
    {
        public ASSETCHANGEITEM2Entity()
        {
        }

        public ASSETCHANGEITEM2Entity(string billid, string assetid_old, string assetcode_new)
        {
            BILLID = billid;
            ASSETID_OLD = assetid_old;
            ASSETCODE_NEW = assetcode_new;
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
        public string ASSETID_OLD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ASSETID_NEW
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ASSETCODE_NEW
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
        public string AREA_USABLE_NEW
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
