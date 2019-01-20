using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class WLINSTOCKEntity
    {
        [ForeignKey(nameof(BILLID), nameof(WLINSTOCKITETMEntity.BILLID))]
        public List<WLINSTOCKITETMEntity> WLINSTOCKITETM
        {
            get;
            set;
        }
    }
}
