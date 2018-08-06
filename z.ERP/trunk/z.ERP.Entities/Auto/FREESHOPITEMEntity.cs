using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FREESHOPITEM", "退铺单明细")]
    public partial class FREESHOPITEMEntity : TableEntityBase
    {
        public FREESHOPITEMEntity()
        {
        }

        public FREESHOPITEMEntity(string billid, string shopid)
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
        /// 分店ID
        /// <summary>
        [Field("店铺ID")]
        public string SHOPID
        {
            get; set;
        }
    }

}
