#if SqlServer
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using z.DbHelper.DbDomain;
using z.DBHelper.Connection;
using z.DBHelper.Info;
using z.Exceptions;
using z.Extensions;

namespace z.DBHelper.Helper
{
    public class SqlServerDbHelper : DbHelperBase
    {
        /// <summary>
        /// 使用链接类初始化oracle的链接
        /// </summary>
        /// <param name="Connection"></param>
        public SqlServerDbHelper(SqlServerDbConnection Connection)
            : base(Connection)
        {

        }

        /// <summary>
        /// 使用链接字符串初始化oracle的链接
        /// </summary>
        /// <param name="Connection"></param>
        public SqlServerDbHelper(string Connection)
            : base(Connection)
        {

        }

        public override string GetMergerSql(MergeInfo info)
        {
            if (info.Pk == null || info.Pk.Count == 0)
            {
                throw new DataBaseException("Merger方法不能没有主键", info.ToJson());
            }
            bool needinsert = info.InsertPram != null && info.InsertPram.Count != 0;
            bool needupdate = info.UpdatePram != null && info.UpdatePram.Count != 0;
            if (!needinsert && !needupdate)
            {
                throw new DataBaseException("Merger方法的insert和update参数不能同时为空", info.ToJson());
            }
            string mainsql = @"  merge into {0} a
                            using (select {1}) b
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
            return mainsql + ";";
        }
        string SetValueToStr(MergePramInfo info)
        {
            if (info.Type == typeof(string))
            {
                return "'" + info.Value?.Replace("'", "''") + "'";
            }
            else if (info.Type == typeof(DateTime))
            {
                return "( SELECT CASE ISDATE('" + info.Value + "') WHEN 1 THEN cast('" + info.Value + "' as DATETIME) ELSE cast('1900-1-1' as DATETIME) END )";
            }
            else
            {
                return "'" + info.Value?.Replace("'", "''") + "'";
            }
        }

        protected override string GetCountSql(string sql)
        {
            return string.Format("select count(1) from({0})", sql);
        }

        protected override DbConnection GetDbConnection(string _dbConnectionInfoStr)
        {
            return new SqlConnection(_dbConnectionInfoStr);
        }

        protected override IDbDataParameter GetDbDataParameter(PropertyInfo p, EntityBase info)
        {
            SqlParameter resp;
            DbTypeAttribute dba = p.GetAttribute<DbTypeAttribute>();
            if (dba == null)
            {
                resp = new SqlParameter(p.Name, SqlDbType.VarChar);
                resp.Value = p.GetValue(info, null);
            }
            else
            {
                switch (dba.DbType)
                {
                    case DbType.Time:
                    case DbType.DateTime:
                    case DbType.Date:
                    case DbType.DateTime2:
                    case DbType.DateTimeOffset:
                        {
                            resp = new SqlParameter(p.Name, SqlDbType.Date);
                            string value = p.GetValue(info, null)?.ToString();
                            if (value == null || string.IsNullOrEmpty(value.ToString()))
                            {
                                resp.Value = DBNull.Value;
                            }
                            else
                            {
                                resp.Value = value.ToDateTime(true);
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
                            resp = new SqlParameter(p.Name, SqlDbType.Int);
                            resp.Value = p.GetValue(info, null);
                            break;
                        }
                    case DbType.Decimal:
                    case DbType.Double:
                        {
                            resp = new SqlParameter(p.Name, SqlDbType.Decimal);
                            resp.Value = p.GetValue(info, null);
                            break;
                        }
                    default:
                        {
                            throw new DataBaseException("字段类型" + dba.DbType + "还没有对应处理程序");
                        }
                }
            }
            return resp;
        }

        protected override string GetPageSql(string sql, int pageSize = 0, int pageIndex = 0)
        {
            throw new NotImplementedException();
        }

        protected override object GetParameterValue(IDbDataParameter p, PropertyInfo pinfo)
        {
            DbTypeAttribute dba = pinfo.GetAttribute<DbTypeAttribute>();
            if (dba == null)
            {
                return p.Value.ToString();
            }
            switch (dba.DbType)
            {
                case DbType.Time:
                case DbType.DateTime:
                case DbType.Date:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    {
                        return p.Value.ToString().ToDateTime();
                    }
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.Byte:
                    {
                        return p.Value.ToString().ToInt();
                    }
                case DbType.Decimal:
                case DbType.Double:
                    {
                        return p.Value.ToString().ToDouble();
                    }
                default:
                    {
                        throw new DataBaseException("字段类型" + dba.DbType + "还没有对应处理程序");
                    }
            }
        }

        protected override string GetPramCols(string cols)
        {
            return ":" + cols;
        }
    }
}
#endif