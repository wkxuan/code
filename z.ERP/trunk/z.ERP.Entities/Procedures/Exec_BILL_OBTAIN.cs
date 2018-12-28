using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("Exec_BILL_OBTAIN")]
    public class Exec_BILL_OBTAIN : ProcedureEntityBase
    {
        [ProcedureField("p_BILLID")]
        public string p_BILLID
        {
            get;
            set;
        }
        [ProcedureField("p_VERIFY")]
        public string p_VERIFY
        {
            get;
            set;
        }
    }
}
