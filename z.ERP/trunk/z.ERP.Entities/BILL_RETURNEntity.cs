using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class BILL_RETURNEntity
    {
        [ForeignKey(nameof(BILLID), nameof(BILL_RETURN_ITEMEntity.BILLID))]
        public List<BILL_RETURN_ITEMEntity> BILL_RETURN_ITEM
        {
            get;
            set;
        }
    }
}
