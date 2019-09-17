using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class FR_PLANEntity
    {
        [ForeignKey(nameof(ID), nameof(FR_PLAN_ITEMEntity.ID))]
        public List<FR_PLAN_ITEMEntity> FR_PLAN_ITEM
        {
            get;
            set;
        }
    }
}
