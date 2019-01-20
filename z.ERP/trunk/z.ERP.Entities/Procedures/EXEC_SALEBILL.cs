using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_SALEBILL")]
    public class EXEC_SALEBILL: ProcedureEntityBase
    {
        [ProcedureField("P_BILLID")]
        public string P_BILLID
        {
            get;
            set;
        }
        [ProcedureField("P_VERIFY")]
        public string P_VERIFY
        {
            get;
            set;
        }
    }
}
