using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("SALEBILLITEM", "销售补录单明细")]
    public partial class SALEBILLITEMEntity : TableEntityBase
    {
        public SALEBILLITEMEntity()
        {
        }

        public SALEBILLITEMEntity(string billid, string goodsid,string payid)
        {
            BILLID = billid;
            GOODSID = goodsid;
            PAYID = payid;
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
        /// 商品id
        /// <summary>
        [Field("商品ID")]
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 收款方式
        /// <summary>
        [Field("收款方式")]
        public string PAYID
        {
            get; set;
        }
        /// <summary>
        /// 销售数量
        /// <summary>
        [Field("销售数量")]
        public string QUANTITY
        {
            get; set;
        }
        /// <summary>
        /// 销售金额
        /// <summary>
        [Field("销售金额")]
        public string AMOUNT
        {
            get; set;
        }
        /// <summary>
        /// 商铺
        /// <summary>
        [Field("商铺")]
        public string SHOPID
        {
            get; set;
        }
    }

}
