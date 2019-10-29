using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PAY_CHARGES", "收款方式手续费设置")]
    public partial class PAY_CHARGESEntity : TableEntityBase
    {
        public PAY_CHARGESEntity()
        {
        }

        public PAY_CHARGESEntity(string branchid, string payid)
        {
            BRANCHID = branchid;
            PAYID = payid;
        }

        [PrimaryKey]
        [Field("门店id")]
        public string BRANCHID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("支付方式id")]
        public string PAYID
        {
            get; set;
        }

        [Field("单笔最低")]
        public string FLOOR
        {
            get; set;
        }


        [Field("单笔封顶")]
        public string CEILING
        {
            get; set;
        }
        [Field("比率")]
        public string RATE
        {
            get; set;
        }
    }
}
