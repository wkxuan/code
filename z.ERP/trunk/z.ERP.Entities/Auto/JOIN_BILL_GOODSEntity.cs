/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:14
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("JOIN_BILL_GOODS", "联营结算单商品")]
    public partial class JOIN_BILL_GOODSEntity : TableEntityBase
    {
        public JOIN_BILL_GOODSEntity()
        {
        }

        public JOIN_BILL_GOODSEntity(string billid, string goodsid, string drate)
        {
            BILLID = billid;
            GOODSID = goodsid;
            DRATE = drate;
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
        /// 商品内码
        /// <summary>
        [PrimaryKey]
        [Field("商品内码")]
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 扣率
        /// <summary>
        [PrimaryKey]
        [Field("扣率")]
        public string DRATE
        {
            get; set;
        }
        /// <summary>
        /// 税率
        /// <summary>
        [Field("税率")]
        public string JXSL
        {
            get; set;
        }
        /// <summary>
        /// 销售数量
        /// <summary>
        [Field("销售数量")]
        public string SELL_SL
        {
            get; set;
        }
        /// <summary>
        /// 销售金额
        /// <summary>
        [Field("销售金额")]
        public string SELL_JE
        {
            get; set;
        }
        /// <summary>
        /// 优惠金额
        /// <summary>
        [Field("优惠金额")]
        public string YHJE
        {
            get; set;
        }
        /// <summary>
        /// 结算金额价款
        /// <summary>
        [Field("结算金额价款")]
        public string SELL_COST
        {
            get; set;
        }
        /// <summary>
        /// 税金
        /// <summary>
        [Field("税金")]
        public string ZZSJE
        {
            get; set;
        }
    }
}
