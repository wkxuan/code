using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
        [DbProcedure("MAKE_CONTRACT_INFO")]
        public class MAKE_CONTRACT_INFO : ProcedureEntityBase
        {
            [ProcedureField("in_CONTRACTID")]
            public string in_CONTRACTID
            {
                get;
                set;
            }    
    }
}
