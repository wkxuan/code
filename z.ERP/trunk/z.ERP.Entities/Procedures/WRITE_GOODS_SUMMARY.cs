using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("WRITE_GOODS_SUMMARY")]
    public class WRITE_GOODS_SUMMARY : ProcedureEntityBase
    {
        [ProcedureField("in_RQ")]
        public string in_RQ
        {
            get;
            set;
        }
        [ProcedureField("in_BRANCHID")]
        public string in_BRANCHID
        {
            get;
            set;
        }
    }
}
