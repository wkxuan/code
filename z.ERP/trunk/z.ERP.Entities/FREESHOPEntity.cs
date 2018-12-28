using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class FREESHOPEntity
    {
        [ForeignKey(nameof(BILLID), nameof(FREESHOPEntity.BILLID))]
        public List<FREESHOPITEMEntity> FREESHOPITEM
        {
            get;
            set;
        }
    }
}
