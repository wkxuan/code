using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class PRESENT_SENDEntity
    {
        [ForeignKey(nameof(BILLID), nameof(PRESENT_SEND_TICKETEntity.BILLID))]
        public List<PRESENT_SEND_TICKETEntity> PRESENT_SEND_TICKET
        {
            get;
            set;
        }
        [ForeignKey(nameof(BILLID), nameof(PRESENT_SEND_ITEMEntity.BILLID))]
        public List<PRESENT_SEND_ITEMEntity> PRESENT_SEND_ITEM
        {
            get;
            set;
        }
    }
}
