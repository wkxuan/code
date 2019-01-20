using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("HYXF_QD_HYCXDJ")]
    public class HYXF_QD_HYCXDJ : ProcedureEntityBase
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
