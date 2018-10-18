#if ORACLE
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using z;
using z.DbHelper.DbDomain;
using z.DBHelper.Connection;
using z.DBHelper.Info;
using z.Exceptions;
using z.Extensions;

namespace z.DBHelper.Helper
{
    public class OracleDbHelper : DbHelperBase
    {
        #region 构造
        /// <summary>
        /// 使用链接类初始化oracle的链接
        /// </summary>
        /// <param name="Connection"></param>
        public OracleDbHelper(OracleDbConnection Connection)
            : base(Connection)
        {

        }

        /// <summary>
        /// 使用链接字符串初始化oracle的链接
        /// </summary>
        /// <param name="Connection"></param>
        public OracleDbHelper(string Connection)
            : base(Connection)
        {

        }
        #endregion
        #region sql方法
        public override string GetMergerSql(MergeInfo info)
        {
            bool needinsert = info.InsertPram != null && info.InsertPram.Count != 0;
            bool needupdate = info.UpdatePram != null && info.UpdatePram.Count != 0;
            if (!needinsert && !needupdate)
            {
                throw new DataBaseException("Merger方法的insert和update参数不能同时为空");
            }
            string mainsql = @"  merge into {0} a
                            using (select {1} from dual) b
                            on (1 = 1 {2})";
            string sql0 = info.TableName;
            List<string> sql1list = new List<string>();
            foreach (KeyValuePair<string, MergePramInfo> item in info.Pk)
            {
                sql1list.Add(SetValueToStr(item.Value) + " " + item.Key);
            }
            List<string> sql2list = new List<string>();
            foreach (KeyValuePair<string, MergePramInfo> item in info.Pk)
            {
                sql2list.Add(" and a." + item.Key + "=b." + item.Key);
            }
            List<string> sql3list = new List<string>();

            mainsql = string.Format(mainsql,
                                    sql0,
                                    string.Join(",", sql1list),
                                    string.Join(" ", sql2list)
                                    );
            //更新字段
            if (needupdate)
            {
                foreach (KeyValuePair<string, MergePramInfo> item in info.UpdatePram)
                {
                    if (!info.Pk.ContainsKey(item.Key))
                    {
                        sql3list.Add(" a." + item.Key + "=" + SetValueToStr(item.Value) + " ");
                    }
                }
                string updatesql = @"when matched then
                                     update set " + string.Join(",", sql3list);
                mainsql += updatesql;
                if (sql3list.Count == 0)
                {
                    throw new DataBaseException("更新字段必须有主键之外的项");
                }
            }
            List<string> sql4list = new List<string>();
            List<string> sql5list = new List<string>();
            //插入字段
            if (needinsert)
            {
                //插入主键字段
                foreach (KeyValuePair<string, MergePramInfo> item in info.Pk)
                {
                    sql4list.Add(item.Key);
                    sql5list.Add(SetValueToStr(item.Value));
                }
                //插入其他字段
                foreach (KeyValuePair<string, MergePramInfo> item in info.InsertPram)
                {
                    if (!info.Pk.ContainsKey(item.Key))
                    {
                        sql4list.Add(item.Key);
                        sql5list.Add(SetValueToStr(item.Value));
                    }
                }
                string insertsql = @"when not matched then
                                      insert
                                        ({0})
                                      values
                                        ({1})";
                insertsql = string.Format(insertsql,
                                            string.Join(",", sql4list),
                                            string.Join(",", sql5list)
                                            );
                mainsql += insertsql;
            }
            return mainsql;
        }

        string SetValueToStr(MergePramInfo info)
        {
            if (info.Type == typeof(string))
            {
                return "'" + info.Value.Replace("'", "''") + "'";
            }
            else if (info.Type == typeof(DateTime))
            {
                return "to_date('" + info.Value + "','yyyy-mm-dd hh24:mi:ss')";
            }
            else
            {
                return "'" + info.Value.Replace("'", "''") + "'";
            }
        }

        protected override DbParameter GetParameter(string name, object value, DbType? Type = null)
        {
            OracleParameter resp;
            if (!Type.HasValue)
            {
                resp = new OracleParameter(name, OracleDbType.Varchar2);
                resp.Value = value;
            }
            else
            {
                switch (Type.Value)
                {
                    case DbType.Time:
                    case DbType.DateTime:
                    case DbType.Date:
                    case DbType.DateTime2:
                    case DbType.DateTimeOffset:
                        {
                            resp = new OracleParameter(name, OracleDbType.Date);
                            if (value == null || string.IsNullOrEmpty(value.ToString()))
                            {
                                resp.Value = DBNull.Value;
                            }
                            else
                            {
                                resp.Value = value.ToString().ToDateTime(true);
                            }
                            break;
                        }
                    case DbType.Int16:
                    case DbType.Int32:
                    case DbType.Int64:
                    case DbType.UInt16:
                    case DbType.UInt32:
                    case DbType.UInt64:
                    case DbType.Byte:
                        {
                            resp = new OracleParameter(name, OracleDbType.Int64);
                            resp.Value = value;
                            break;
                        }
                    case DbType.Decimal:
                    case DbType.Double:
                        {
                            resp = new OracleParameter(name, OracleDbType.Double);
                            resp.Value = value;
                            break;
                        }
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.Xml:
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                        {
                            resp = new OracleParameter(name, OracleDbType.Varchar2);
                            resp.Value = value;
                            break;
                        }
                    default:
                        {
                            throw new DataBaseException("字段类型" + Type.Value + "还没有对应处理程序");
                        }
                }
            }
            return resp;
        }

        /// <summary>
        /// 取分页sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        protected override string GetPageSql(string sql, int pageSize = 0, int pageIndex = 0)
        {
            if (pageSize < 1 || pageIndex < 0)
            {
                return sql;
            }
            int start = pageIndex * pageSize + 1;
            int end = start + pageSize;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM (SELECT V.*, ROWNUM AS N FROM (");
            builder.Append(sql);
            builder.Append(") V) WHERE N >= ");
            builder.Append(start);
            builder.Append(" AND N < ");
            builder.Append(end);
            string ret = builder.ToString();
            builder.Length = 0;
            builder = null;
            return ret;
        }

        /// <summary>
        /// 取总数sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected override string GetCountSql(string sql)
        {
            return string.Format("select count(1) from({0})", sql);
        }

        protected override string GetPramCols(string cols)
        {
            return ":" + cols;
        }

        protected override string GetFieldName(string FieldName)
        {
            return $"\"{FieldName}\"";
        }

        protected override void FastInsertTable(DataTable dt)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region 链接操作
        protected override DbConnection GetDbConnection(string _dbConnectionInfoStr)
        {
            return new OracleConnection(_dbConnectionInfoStr);
        }

        #endregion

    }
}
#endif