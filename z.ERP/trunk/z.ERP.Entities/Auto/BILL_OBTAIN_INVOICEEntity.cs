using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("BILL_OBTAIN_INVOICE", "账单发票子表")]
    public partial class BILL_OBTAIN_INVOICEEntity:TableEntityBase
    {
        public BILL_OBTAIN_INVOICEEntity() {

        }
        public BILL_OBTAIN_INVOICEEntity(string billid,string invoiceid)
        {
            BILLID = billid;
            INVOICEID = invoiceid;
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
        /// 账单单号
        /// <summary>
        [PrimaryKey]
        [Field("发票号")]
        public string INVOICEID
        {
            get; set;
        }
    }
}
