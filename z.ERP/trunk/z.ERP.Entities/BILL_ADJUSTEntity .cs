using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class BILL_ADJUSTEntity
    {
        [ForeignKey(nameof(BILLID), nameof(BILL_ADJUST_ITEMEntity.BILLID))]
        public List<BILL_ADJUST_ITEMEntity> BILL_ADJUST_ITEM
        {
            get;
            set;
        }
    }
}
