using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class PROMOBILL_RREntity
    {
        [ForeignKey(nameof(BILLID), nameof(PROMOBILL_RR_BRANDEntity.BILLID))]
        public List<PROMOBILL_RR_BRANDEntity> PROMOBILL_RR_BRAND
        {
            get;
            set;
        }
    }
}
