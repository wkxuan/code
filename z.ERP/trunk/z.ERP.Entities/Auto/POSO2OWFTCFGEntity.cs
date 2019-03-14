using z.DBHelper.DBDomain;

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

        [Field("加密方式")]
        public string ENCRYPTION
        {
            get;set;
        }

        [Field("密钥")]
        public string KEY
        {
            get;set;
        }

        [Field("公钥")]
        public string KEY_PUB
        {
            get; set;
        }

        [Field("日志路径")]
        public string LOG
        {
            get;set;
        }

    }
}
