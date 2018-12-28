using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("PROTEST")]
    public class ProTest : ProcedureEntityBase
    {
        [DbType(DbType.Int32)]
        [ProcedureField("in_orgid")]
        public int Id
        {
            get;
            set;
        }
        [ProcedureField("in_a")]
        public string a
        {
            get;
            set;
        }
        [ProcedureField("out_orgcode")]
        public string Code
        {
            get;
            set;
        }

        [DbType(DbType.DateTime)]
        [ProcedureField("out_date")]
        public DateTime time
        {
            get;
            set;
        }

        [DbType(DbType.Int32)]
        [ProcedureField("out_int")]
        public int i
        {
            get;
            set;
        }

    }
}
