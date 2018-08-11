using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_ASSET_SPILT")]
    public class EXEC_ASSET_SPILT : ProcedureEntityBase
    {
        [ProcedureField("V_BILLID")]
        public string V_BILLID
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
