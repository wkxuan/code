#if SqlServer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.Connection
{
    public class SqlServerDbConnection : IDbConnectionInfo
    {
        public string ToConnectionString()
        {
            return string.Format("server={0};database={1};uid={2};pwd={3}",
             Server, Database, Uid, Pwd);
        }
        private string server;
        private string database;
        private string uid;
        private string pwd;

        public string Server
        {
            get
            {
                return server;
            }

            set
            {
                server = value;
            }
        }

        public string Database
        {
            get
            {
                return database;
            }

            set
            {
                database = value;
            }
        }

        public string Uid
        {
            get
            {
                return uid;
            }

            set
            {
                uid = value;
            }
        }

        public string Pwd
        {
            get
            {
                return pwd;
            }

            set
            {
                pwd = value;
            }
        }
    }
}

#endif