using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class DEF_ALERTEntity
    {
        [ForeignKey(nameof(ID), nameof(ALERT_FIELDEntity.ID))]
        public List<ALERT_FIELDEntity> ALERT_FIELD
        {
            get;
            set;
        }

    }
}
