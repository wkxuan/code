using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_BILL_NOTICE")]
    public class EXEC_BILL_NOTICE : ProcedureEntityBase
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
