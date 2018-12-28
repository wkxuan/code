using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("HZ_YHQ_CXHDBYHT")]
    public class HZ_YHQ_CXHDBYHT : ProcedureEntityBase
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
