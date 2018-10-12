using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("POSO2OWFTCFG", "POS第三方支付配置表")]
    public partial class POSO2OWFTCFGEntity: TableEntityBase
    {
        public POSO2OWFTCFGEntity()
        {
        }

        public POSO2OWFTCFGEntity(string posno)
        {
            POSNO = posno;
        }

        [PrimaryKey]
        [Field("终端号")]
        public string POSNO
        {
            get; set;
        }

        [Field("前置机地址")]
        public string URL
        {
            get;set;
        }

        public string PID
        {
            get;set;
        }

        public string KEY
        {
            get;set;
        }

        [Field("日志路径")]
        public string LOG
        {
            get;set;
        }

    }
}
