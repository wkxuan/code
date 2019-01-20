using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("HYK_YHQ_QD_CXDJ")]
    public class HYK_YHQ_QD_CXDJ : ProcedureEntityBase
    {

        [DbType(DbType.DateTime)]
        [ProcedureField("pRCLRQ")]
        public DateTime pRCLRQ
        {
            get;
            set;
        }
        [ProcedureField("pZXR")]
        public string pZXR
        {
            get;
            set;
        }
    }
}
