using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class CONTRACTEntity
    {
        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_BRANDEntity.CONTRACTID))]
        public List<CONTRACT_BRANDEntity> CONTRACT_BRAND
        {
            get;
            set;
        }

        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_SHOPEntity.CONTRACTID))]
        public List<CONTRACT_SHOPEntity> CONTRACT_SHOP
        {
            get;
            set;
        }
    }
}
