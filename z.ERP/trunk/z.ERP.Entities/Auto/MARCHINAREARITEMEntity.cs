using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("MARCHINAREARITEM", "商户进场管理单据明细")]
    public partial class MARCHINAREARITEMEntity: TableEntityBase
    {
        public MARCHINAREARITEMEntity()
        {
        }

        public MARCHINAREARITEMEntity(string billid, string shopid)
        {
            BILLID = billid;
            SHOPID = shopid;            
        }
        /// <summary>
        /// 单号
        /// <summary>
        [PrimaryKey]
        [Field("单号")]
        public string BILLID
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
    }
}
