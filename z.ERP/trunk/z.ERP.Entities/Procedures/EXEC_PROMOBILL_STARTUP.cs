using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_PROMOBILL_STARTUP")]
    public class EXEC_PROMOBILL_STARTUP : ProcedureEntityBase
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
