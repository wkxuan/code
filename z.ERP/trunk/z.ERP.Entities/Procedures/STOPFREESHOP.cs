using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("STOPFREESHOP")]
    public class STOPFREESHOP : ProcedureEntityBase
    {
        [ProcedureField("P_BILLID")]
        public string P_BILLID
        {
            get;
            set;
        }
        [ProcedureField("P_TERMINATE")]
        public string P_TERMINATE
        {
            get;
            set;
        }
    }
}
