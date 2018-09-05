#if SYBASE
namespace z.DBHelper.Connection
{
    public class SybaseConnection : IDbConnectionInfo
    {
        public string ToConnectionString()
        {
            return string.Format("Data Source={0};database={1};charset={2};Port={3};UID={4};PWD={5};language={6}",
                Data_Source,
                Database,
                Charset,
                Port,
                UID,
                PWD,
                Language);
        }

        private string _Data_Source;
        private string _database;
        private string _charset = "cp850";
        private string _Port;
        private string _UID;
        private string _PWD;
        private string _language = "us_english";

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string PWD
        {
            get { return _PWD; }
            set { _PWD = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UID
        {
            get { return _UID; }
            set { _UID = value; }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public string Port
        {
            get { return _Port; }
            set { _Port = value; }
        }


        /// <summary>
        /// 字符集(cp850)
        /// </summary>
        public string Charset
        {
            get { return _charset; }
            set { _charset = value; }
        }

        /// <summary>
        /// 库
        /// </summary>
        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        /// <summary>
        /// 服务器
        /// </summary>
        public string Data_Source
        {
            get { return _Data_Source; }
            set { _Data_Source = value; }
        }

    }
}
#endif