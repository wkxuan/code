#if MYSQL
namespace z.DBHelper.Connection
{
    public class MysqlConnection : IDbConnectionInfo
    {

        public string ToConnectionString()
        {
            return string.Format("Data Source={0};Database={1};CharSet={2};port={3};User Id={4};Password={5}",
                Data_Source,
                Database,
                Charset,
                Port,
                UID,
                PWD);
        }

        private string _Data_Source;
        private string _database;
        private string _charset;
        private string _Port;
        private string _UID;
        private string _PWD;

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
        /// 字符集
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