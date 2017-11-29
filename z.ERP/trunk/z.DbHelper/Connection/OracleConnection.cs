
namespace z.DBHelper.Connection
{
    /// <summary>
    /// oracle的链接基类
    /// </summary>
    public class OracleDbConnection : IDbConnectionInfo
    {
        private string _PROTOCOL = "TCP";
        private string _HOST;
        private string _PORT = "1521";
        private string _SERVICE_NAME;
        private string _USERID;
        private string _PASSWORD;

        public string ToConnectionString()
        {

            return string.Format(@" data source= 
                                        (DESCRIPTION =
                                            (ADDRESS =
                                            (PROTOCOL = {0})
                                            (HOST = {1})
                                            (PORT = {2})
                                        )         
                                        (CONNECT_DATA =
                                            (SERVICE_NAME = {3})
                                        )
                                        );User Id={4};Password={5};",
                                this.PROTOCOL,
                                this.HOST,
                                this.PORT,
                                this.SERVICE_NAME,
                                this.USERID,
                                this.PASSWORD);
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string PASSWORD
        {
            get { return _PASSWORD; }
            set { _PASSWORD = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string USERID
        {
            get { return _USERID; }
            set { _USERID = value; }
        }

        /// <summary>
        /// 服务名
        /// </summary>
        public string SERVICE_NAME
        {
            get { return _SERVICE_NAME; }
            set { _SERVICE_NAME = value; }
        }

        /// <summary>
        /// 端口号(1521)
        /// </summary>
        public string PORT
        {
            get { return _PORT; }
            set { _PORT = value; }
        }

        /// <summary>
        /// 主机
        /// </summary>
        public string HOST
        {
            get { return _HOST; }
            set { _HOST = value; }
        }

        /// <summary>
        /// 协议(TCP)
        /// </summary>
        public string PROTOCOL
        {
            get { return _PROTOCOL; }
            set { _PROTOCOL = value; }
        }
    }
}
