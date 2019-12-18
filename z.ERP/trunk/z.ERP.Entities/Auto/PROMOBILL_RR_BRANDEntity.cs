using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PROMOBILL_RR_BRAND", "随机立减活动定义单")]
    public partial class PROMOBILL_RR_BRANDEntity : TableEntityBase
    {
        public PROMOBILL_RR_BRANDEntity()
        {
        }

        public PROMOBILL_RR_BRANDEntity(string id, string brandid)
        {
            BILLID = id;
            BRANDID = brandid;
        }
        [PrimaryKey]
        [Field("促销单id")]
        public string BILLID
        {
            get; set;
        }
        [Field("序号")]
        public string INX
        {
            get; set;
        }
        [PrimaryKey]
        [Field("品牌")]
        public string BRANDID
        {
            get; set;
        }
        [Field("订单金额下限 0不限制")]
        public string AMOUNT_LIMIT
        {
            get; set;
        }
        [Field("立减下限")]
        public string REDUCE_DOWN
        {
            get; set;
        }
        [Field("立减上限")]
        public string REDUCE_UP
        {
            get; set;
        }
        [Field("立减上限(订单金额比例) 0不限制")]
        public string REDUCE_UP_RATE
        {
            get; set;
        }
        [Field("预算费用 0 不限制")]
        public string BUDGET
        {
            get; set;
        }
    }
}
