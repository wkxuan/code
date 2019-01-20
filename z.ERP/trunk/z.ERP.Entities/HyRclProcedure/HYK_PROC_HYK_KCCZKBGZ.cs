using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("HYK_PROC_HYK_KCCZKBGZ")]
    public class HYK_PROC_HYK_KCCZKBGZ : ProcedureEntityBase
    {

        [DbType(DbType.DateTime)]
        [ProcedureField("pRCLRQ")]
        public DateTime pRCLRQ
        {
            get;
            set;
        }
        [ProcedureField("pNY")]
        public string pNY
        {
            get;
            set;
        }
    }
}
