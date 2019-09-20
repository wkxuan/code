using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_PROMOBILL_STOP")]
    public class EXEC_PROMOBILL_STOP : ProcedureEntityBase
    {
        [ProcedureField("in_BILLID")]
        public string in_BILLID
        {
            get;
            set;
        }
        [ProcedureField("in_USERID")]
        public string in_USERID
        {
            get;
            set;
        }
    }
}
