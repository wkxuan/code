#if SQLLITE
namespace z.DBHelper.Connection
{
    public class SqliteConnection : IDbConnectionInfo
    {
        private string _DataFile;
        private string _psw;

        public string ToConnectionString()
        {
            System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connstr.DataSource = @DataFile;
            connstr.Password = Psw;
            return connstr.ToString();
        }

        /// <summary>
        /// 访问密码
        /// </summary>
        public string Psw
        {
            get { return _psw; }
            set { _psw = value; }
        }

        /// <summary>
        /// 数据文件物理路径
        /// </summary>
        public string DataFile
        {
            get { return _DataFile; }
            set { _DataFile = value; }
        }

    }
}
#endif