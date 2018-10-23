using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("Exec_BILL_ADJUST")]
    public class Exec_BILL_ADJUST : ProcedureEntityBase
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
