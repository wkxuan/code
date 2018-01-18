using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class ASSETCHANGEEntity
    {
        [ForeignKey(nameof(BILLID), nameof(ASSETCHANGEITEMEntity.BILLID))]
        public List<ASSETCHANGEITEMEntity> ASSETCHANGEITEM
        {
            get;
            set;
        }
        [ForeignKey(nameof(BILLID), nameof(ASSETCHANGEITEMEntity.BILLID))]
        public List<ASSETCHANGEITEMEntity> ASSETCHANGEITEM2
        {
            get;
            set;
        }
    }
}
