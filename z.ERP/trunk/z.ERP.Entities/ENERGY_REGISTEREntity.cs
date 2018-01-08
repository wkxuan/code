using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class ENERGY_REGISTEREntity
    {
        [ForeignKey(nameof(BILLID),nameof(ENERGY_REGISTER_ITEMEntity.BILLID))]
        public List<ENERGY_REGISTER_ITEMEntity> ENERGY_REGISTER_ITEM
        {
            get;
            set;
        }
    }
}
