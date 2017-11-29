#if SQLLITE
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using z.DBHelper.Connection;
using z.DBHelper.Info;

namespace z.DBHelper.Helper
{
    public class sqliteDbHelper : DbHelperBase
    {
        #region 构造
        /// <summary>
        /// 使用链接类初始化sqlite的链接
        /// </summary>
        /// <param name="Connection"></param>
        public sqliteDbHelper(SqliteConnection Connection)
            : base(Connection)
        {

        }

        /// <summary>
        /// 使用链接字符串初始化sqlite的链接
        /// </summary>
        /// <param name="Connection"></param>
        public sqliteDbHelper(string Connection)
            : base(Connection)
        {

        }
        #endregion
        #region sql方法
        public override string GetMergerSql(MergeInfo info)
        {
            string sql = "INSERT OR REPLACE INTO " + info.TableName + " ({0}) VALUES({1})";
            return string.Format(sql, Getpram(info.UpdatePram), GetpramValue(info.UpdatePram));
        }

        private string GetpramValue(Dictionary<string, MergePramInfo> dictionary)
        {
            List<string> keylist = new List<string>();
            foreach (KeyValuePair<string, MergePramInfo> item in dictionary)
            {
                if (item.Value.Type == typeof(string))
                {
                    keylist.Add("'" + item.Value.Value.Replace("'", "''") + "'");
                }
                else
                {
                    keylist.Add("'" + item.Value.Value.Replace("'", "''") + "'");
                }
            }
            return string.Join(",", keylist);
        }

        private string Getpram(Dictionary<string, MergePramInfo> dictionary)
        {
            return string.Join(",", dictionary.Keys);
        }
        #endregion
        #region 链接操作
        protected override DbConnection GetDbConnection(string _dbConnectionInfoStr)
        {
            return new System.Data.SQLite.SQLiteConnection(_dbConnectionInfoStr);
        }

        protected override DbCommand GetDbCommand(DbConnection dbconnection)
        {
            return new SQLiteCommand(_dbConnection as SQLiteConnection);
        }
        #endregion
    }
}
#endif