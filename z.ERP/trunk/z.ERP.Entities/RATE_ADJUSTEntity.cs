using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;


namespace z.ERP.Entities
{
    public partial class RATE_ADJUSTEntity
    {
        [ForeignKey(nameof(ID), nameof(RATE_ADJUSTEntity.ID))]
        public List<RATE_ADJUST_ITEMEntity> RATE_ADJUST_ITEM
        {
            get;
            set;
        }
    }
}
