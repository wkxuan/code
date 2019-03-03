using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class WLSETTLEEntity
    {
        [ForeignKey(nameof(BILLID), nameof(WLSETTLEITEMEntity.BILLID))]
        public List<WLSETTLEITEMEntity> WLSETTLEITEM
        {
            get;
            set;
        }
    }
}
