using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;
using z.ERP.Entities.Auto;

namespace z.ERP.Entities
{
    public partial class BILL_OBTAINEntity
    {
        [ForeignKey(nameof(BILLID), nameof(BILL_OBTAIN_ITEMEntity.BILLID))]
        public List<BILL_OBTAIN_ITEMEntity> BILL_OBTAIN_ITEM
        {
            get;
            set;
        }
        [ForeignKey(nameof(BILLID), nameof(BILL_OBTAIN_INVOICEEntity.BILLID))]
        public List<BILL_OBTAIN_INVOICEEntity> BILL_OBTAIN_INVOICE
        {
            get;
            set;
        }
    }
}
