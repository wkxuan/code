using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("MERCHANT_PAYMENT", "商户付款方式")]
    public partial class MERCHANT_PAYMENTEntity : TableEntityBase
    {
        public MERCHANT_PAYMENTEntity()
        {
        }

        public MERCHANT_PAYMENTEntity(string merchantid, string paymentid)
        {
            MERCHANTID = merchantid;
            PAYMENTID = paymentid;
        }

        /// <summary>
        /// 商户代码
        /// <summary>
        [PrimaryKey]
        [Field("商户代码")]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 品牌
        /// <summary>
        [PrimaryKey]
        [Field("付款方式编码")]
        public string PAYMENTID
        {
            get; set;
        }
        [Field("银行卡号")]
        public string CARDNO
        {
            get; set;
        }
        [Field("银行名称")]
        public string BANKNAME
        {
            get; set;
        }
        [Field("开户人")]
        public string HOLDERNAME
        {
            get; set;
        }
        [Field("身份证号")]
        public string IDCARD
        {
            get; set;
        }
    }
}
