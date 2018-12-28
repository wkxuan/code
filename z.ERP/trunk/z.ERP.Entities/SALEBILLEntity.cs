using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class SALEBILLEntity
    {
        [ForeignKey(nameof(BILLID), nameof(SALEBILLEntity.BILLID))]
        public List<SALEBILLITEMEntity> SALEBILLITEM
        {
            get;
            set;
        }
    }
}
