using System.Collections.Generic;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class PROMOBILLEntity
    {
        [ForeignKey(nameof(BILLID), nameof(PROMOBILL_GOODSEntity.BILLID))]
        public List<PROMOBILL_GOODSEntity> PROMOBILL_GOODS
        {
            get;
            set;
        }
        [ForeignKey(nameof(BILLID), nameof(PROMOBILL_FG_RULEEntity.BILLID))]
        public List<PROMOBILL_FG_RULEEntity> PROMOBILL_FG_RULE
        {
            get;
            set;
        }
    }
}
