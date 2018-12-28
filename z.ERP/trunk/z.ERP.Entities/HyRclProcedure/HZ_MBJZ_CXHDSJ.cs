using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("HZ_MBJZ_CXHDSJ")]
    public class HZ_MBJZ_CXHDSJ : ProcedureEntityBase
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
