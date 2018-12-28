using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

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
    }
}
