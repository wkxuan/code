using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_CONTRACT_UPDATE")]
    public class EXEC_CONTRACT_UPDATE : ProcedureEntityBase
    {
        [ProcedureField("in_CONTRACTID")]
        public string in_CONTRACTID
        {
            get;
            set;
        }
        [ProcedureField("in_USERID")]
        public string in_USERID
        {
            get;
            set;
        }
    }
}
