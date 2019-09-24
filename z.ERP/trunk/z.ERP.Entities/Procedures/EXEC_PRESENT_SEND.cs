using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_PRESENT_SEND")]
    public class EXEC_PRESENT_SEND: ProcedureEntityBase
    {
        [ProcedureField("in_BILLID")]
        public string in_BILLID
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
