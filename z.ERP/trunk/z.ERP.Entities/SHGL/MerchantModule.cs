using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class MerchantModuleEntity:MERCHANTEntity
    {
        public List<MERCHANT_BRANDEntity> MERCHANT_BRAND
        {
            get;
            set;
        }
    }
}
