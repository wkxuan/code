using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class WLUSESEntity
    {
        [ForeignKey(nameof(BILLID), nameof(WLUSESITETMEntity.BILLID))]
        public List<WLUSESITETMEntity> WLUSESITETM
        {
            get;
            set;
        }
    }
}
