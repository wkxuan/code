using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Procedures
{
    [DbProcedure("EXEC_CONTRACT_STARTUP")]
    public class EXEC_CONTRACT_STARTUP : ProcedureEntityBase
    {
        [ProcedureField("V_CONTRACTID")]
        public string V_CONTRACTID
        {
            get;
            set;
        }
        [ProcedureField("V_USERID")]
        public string V_USERID
        {
            get;
            set;
        }
    }
}
