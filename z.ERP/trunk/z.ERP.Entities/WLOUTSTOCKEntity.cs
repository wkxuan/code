using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class WLOUTSTOCKEntity
    {
        [ForeignKey(nameof(BILLID), nameof(WLOUTSTOCKITETMEntity.BILLID))]
        public List<WLOUTSTOCKITETMEntity> WLOUTSTOCKITETM
        {
            get;
            set;
        }
    }
}
