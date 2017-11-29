#if SYBASE
using System;
using System.Data.Common;
using Sybase.Data.AseClient;
using z.DBHelper.Connection;
using z.DBHelper.Info;

namespace z.DBHelper.Helper
{
    public class SybaseDbHelper : DbHelperBase
    {
#region 构造
        /// <summary>
        /// 使用链接类初始化sqlite的链接
        /// </summary>
        /// <param name="Connection"></param>
        public SybaseDbHelper(SybaseConnection Connection)
            : base(Connection)
        {

        }

        /// <summary>
        /// 使用链接字符串初始化sqlite的链接
        /// </summary>
        /// <param name="Connection"></param>
        public SybaseDbHelper(string Connection)
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
            return new AseConnection(_dbConnectionInfoStr);
        }

        protected override DbCommand GetDbCommand(DbConnection dbconnection)
        {
            AseCommand cmd = new AseCommand();
            cmd.Connection = dbconnection as AseConnection;
            return cmd;
        }
#endregion
    }
}
#endif