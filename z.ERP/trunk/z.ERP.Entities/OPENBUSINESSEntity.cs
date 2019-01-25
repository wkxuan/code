using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;
using z.ERP.Entities.Auto;

namespace z.ERP.Entities
{
    public partial class OPENBUSINESSEntity
    {
        [ForeignKey(nameof(BILLID), nameof(OPENBUSINESSITEMEntity.BILLID))]
        public List<OPENBUSINESSITEMEntity> OPENBUSINESSITEM
        {
            get;
            set;
        }
    }
}
