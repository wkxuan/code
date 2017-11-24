#if MYSQL
using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using z.DBHelper.Connection;
using z.DBHelper.Info;

namespace z.DBHelper.Helper
{
    public class MysqlDbHelper : DbHelperBase
    {
        #region 构造
        /// <summary>
        /// 使用链接类初始化sqlite的链接
        /// </summary>
        /// <param name="Connection"></param>
        public MysqlDbHelper(SybaseConnection Connection)
            : base(Connection)
        {

        }

        /// <summary>
        /// 使用链接字符串初始化sqlite的链接
        /// </summary>
        /// <param name="Connection"></param>
        public MysqlDbHelper(string Connection)
            : base(Connection)
        {

        }
        #endregion
        #region sql方法
        public override string GetMergerSql(MergeInfo info)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 链接操作
        protected override DbConnection GetDbConnection(string _dbConnectionInfoStr)
        {
            return new MySqlConnection(_dbConnectionInfoStr);
        }

        protected override DbCommand GetDbCommand(DbConnection dbconnection)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbconnection as MySqlConnection ;
            return cmd;
        }
        #endregion
    }
}
#endif