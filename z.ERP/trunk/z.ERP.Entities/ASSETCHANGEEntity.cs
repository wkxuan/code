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
        [ForeignKey(nameof(BILLID), nameof(ASSETCHANGEITEM2Entity.BILLID))]
        public List<ASSETCHANGEITEM2Entity> ASSETCHANGEITEM2
        {
            get;
            set;
        }
    }
}
