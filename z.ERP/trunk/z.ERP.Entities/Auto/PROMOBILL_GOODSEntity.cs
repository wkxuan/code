using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PROMOBILL_GOODS", "促销单商品子表")]
    public partial class PROMOBILL_GOODSEntity : TableEntityBase
    {
        public PROMOBILL_GOODSEntity()
        {
        }

        public PROMOBILL_GOODSEntity(string id,string goodsid)
        {
            BILLID = id;
            GOODSID = goodsid;
        }
        /// <summary>
        /// 促销单id
        /// <summary>
        [PrimaryKey]
        [Field("促销单id")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 序号
        /// <summary>
        [Field("序号")]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 商品id
        /// <summary>
        [Field("商品id")]
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 值1 折扣率
        /// <summary>
        [Field("值1 折扣率")]
        public string VALUE1
        {
            get; set;
        }
        /// <summary>
        /// 值2 满减方案
        /// <summary>
        [Field("值2 满减方案")]
        public string VALUE2
        {
            get; set;
        }
    }
}

