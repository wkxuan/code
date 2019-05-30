using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;


namespace z.ERP.Entities
{
    public partial class ADJUSTDISCOUNTEntity
    {
        [ForeignKey(nameof(ADID), nameof(ADJUSTDISCOUNTEntity.ADID))]
        public List<ADJUSTDISCOUNTITEMEntity> ADJUSTDISCOUNTITEM
        {
            get;
            set;
        }
    }
}
