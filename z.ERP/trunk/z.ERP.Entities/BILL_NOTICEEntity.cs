using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class BILL_NOTICEEntity
    {
        [ForeignKey(nameof(BILLID), nameof(BILL_NOTICE_ITEMEntity.BILLID))]
        public List<BILL_NOTICE_ITEMEntity> BILL_NOTICE_ITEM
        {
            get;
            set;
        }
    }
}
