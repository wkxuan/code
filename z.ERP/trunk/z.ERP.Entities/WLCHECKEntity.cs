using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class WLCHECKEntity
    {
        [ForeignKey(nameof(BILLID), nameof(WLCHECKITEMEntity.BILLID))]
        public List<WLCHECKITEMEntity> WLCHECKITEM
        {
            get;
            set;
        }
    }
}
