using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("HYXF_ZX_HYK_DJSQGZ")]
    public class HYXF_ZX_HYK_DJSQGZ : ProcedureEntityBase
    {

        [DbType(DbType.DateTime)]
        [ProcedureField("pRCLRQ")]
        public DateTime pRCLRQ
        {
            get;
            set;
        }
    }
}
