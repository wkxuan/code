using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("UPDATE_SALE_GOODS_RATE")]
    public class UPDATE_SALE_GOODS_RATE : ProcedureEntityBase
    {
        [DbType(DbType.DateTime)]
        [ProcedureField("in_RQ")]
        public DateTime in_RQ
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
