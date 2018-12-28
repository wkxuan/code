using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_CONTRACT")]
    public class EXEC_CONTRACT: ProcedureEntityBase
    {
        [ProcedureField("V_CONTRACTID")]
        public string V_CONTRACTID
        {
            get;
            set;
        }
        [ProcedureField("V_USERID")]
        public string V_USERID
        {
            get;
            set;
        }
    }
}
