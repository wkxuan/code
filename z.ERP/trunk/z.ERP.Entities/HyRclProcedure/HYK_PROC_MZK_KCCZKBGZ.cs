using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("HYK_PROC_MZK_KCCZKBGZ")]
    public class HYK_PROC_MZK_KCCZKBGZ : ProcedureEntityBase
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
