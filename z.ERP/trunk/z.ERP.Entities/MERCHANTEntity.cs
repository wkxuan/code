using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class MERCHANTEntity
    {
        [ForeignKey(nameof(MERCHANTID), nameof(MERCHANT_BRANDEntity.MERCHANTID))]
        public List<MERCHANT_BRANDEntity> MERCHANT_BRAND
        {
            get;
            set;
        }
    }
}
