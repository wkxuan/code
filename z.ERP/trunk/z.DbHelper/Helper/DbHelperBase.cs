using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using z;
using z.Exceptions;
using z.DBHelper.Connection;
using z.DBHelper.Info;
using z.DbHelper.DbDomain;
using z.Extensions;
using z.Extensiont;
using z.DBHelper.DbDomain;
using System.Text.RegularExpressions;

namespace z.DBHelper.Helper
{
    /// <summary>
    /// 数据操作类
    /// </summary>
    public abstract class DbHelperBase
    {
        #region 私有变量
        /// <summary>
        /// 配置文件字符串
        /// </summary>
        protected string _dbConnectionInfoStr;
        /// <summary>
        /// 配置文件类
        /// </summary>
        protected IDbConnectionInfo _dbConnectionInfo;
        /// <summary>
        /// 数据库链接
        /// </summary>
        protected DbConnection _dbConnection;

        [ThreadStatic]
        int TransactionCount = 0;
        /// <summary>
        /// 事务对象
        /// </summary>
        protected zDbTransaction _zTransaction = null;

        string _select = "SELECT {0} FROM {1} WHERE {2}";
        string _insert = "INSERT INTO {0}({1}) VALUES({2})";
        string _update = "UPDATE {0} SET {1} WHERE {2}";
        string _delete = "DELETE {0} WHERE {1}";

        string _selectcount = "SELECT COUNT(1) from {0} WHERE {1}";
        #endregion
        #region 抽象

        /// <summary>
        /// 取总数sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected abstract string GetCountSql(string sql);

        /// <summary>
        /// 取分页sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        protected abstract string GetPageSql(string sql, int pageSize = 0, int pageIndex = 0);

        /// <summary>
        /// 获取字段名对应的参数表示字符串
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        protected abstract string GetPramCols(string cols);

        /// <summary>
        /// 获取读写sql
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract string GetMergerSql(MergeInfo info);

        /// <summary>
        /// 取数据库链接
        /// </summary>
        /// <param name="_dbConnectionInfoStr"></param>
        /// <returns></returns>
        protected abstract DbConnection GetDbConnection(string _dbConnectionInfoStr);

        /// <summary>
        /// 获取一个参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="DbType"></param>
        /// <returns></returns>
        protected abstract DbParameter GetParameter(string name, object value, DbType? DbType = null);

        /// <summary>
        /// 获取select中字段的名称
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        protected virtual string GetFieldName(string FieldName)
        {
            return FieldName;
        }

        /// <summary>
        /// 快速插入一张表
        /// </summary>
        /// <param name="dt"></param>
        protected abstract void FastInsertTable(DataTable dt);

        #endregion
        #region 构造

        public DbHelperBase(string ConnectionStr)
        {
            _dbConnectionInfoStr = ConnectionStr;
        }

        public DbHelperBase(IDbConnectionInfo conn)
            : this(conn.ToConnectionString())
        {
            _dbConnectionInfo = conn;
        }

        #endregion
        #region 数据操作
        #region 查表
        public DataTable ExecuteTable(string sql, DbParameter[] parameters)
        {
            return ExecuteTable(sql, 0, 0, parameters);
        }

        public DataTable ExecuteTable(string sql, PageInfo pageinfo, DbParameter[] parameters)
        {
            return ExecuteTable(sql, pageinfo.PageSize, pageinfo.PageIndex, parameters);
        }

        public DataTable ExecuteTable(string sql, PageInfo pageinfo, out int allCount, DbParameter[] parameters)
        {
            return ExecuteTable(sql, pageinfo.PageSize, pageinfo.PageIndex, out allCount, parameters);
        }

        public DataTable ExecuteTable(string sql, int pageSize, int pageIndex, DbParameter[] parameters)
        {
            DataTable dt = new DataTable();
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.AddRange(parameters);
                _dbCommand.CommandText = GetPageSql(sql, pageSize, pageIndex);
                lock (ObjectExtension.Locker)
                {
                    using (IDataReader reader = _dbCommand.ExecuteReader(CommandBehavior.Default))
                    {
                        dt = this.ReaderToTable(reader);
                    }
                }
            });
            return dt;
        }

        public DataTable ExecuteTable(string sql, int pageSize, int pageIndex, out int allCount, DbParameter[] parameters)
        {
            int outall = 0;
            DataTable dt = ExecuteTable(sql, pageSize, pageIndex, parameters);
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.AddRange(parameters);
                _dbCommand.CommandText = GetCountSql(sql);
                lock (ObjectExtension.Locker)
                {
                    using (IDataReader reader = _dbCommand.ExecuteReader())
                    {
                        DataTable dtcount = this.ReaderToTable(reader);
                        if (dtcount.IsOneLine())
                        {
                            int.TryParse(dtcount.Rows[0][0].ToString(), out outall);
                        }
                        else
                        {
                            outall = 0;
                        }
                    }
                }
            });
            allCount = outall;
            return dt;
        }

        public DataTable ExecuteTable(string sql, params zParameter[] parameters)
        {
            string _sql = sql;
            DbParameter[] ps = RenderSql(ref _sql, parameters);
            return ExecuteTable(_sql, ps);
        }

        public DataTable ExecuteTable(string sql, PageInfo pageinfo, params zParameter[] parameters)
        {
            string _sql = sql;
            DbParameter[] ps = RenderSql(ref _sql, parameters);
            return ExecuteTable(_sql, pageinfo, ps);
        }

        public DataTable ExecuteTable(string sql, PageInfo pageinfo, out int allCount, params zParameter[] parameters)
        {
            string _sql = sql;
            DbParameter[] ps = RenderSql(ref _sql, parameters);
            return ExecuteTable(_sql, pageinfo, out allCount, ps);
        }

        public DataTable ExecuteTable(string sql, int pageSize, int pageIndex, params zParameter[] parameters)
        {
            string _sql = sql;
            DbParameter[] ps = RenderSql(ref _sql, parameters);
            return ExecuteTable(_sql, pageSize, pageIndex, ps);
        }

        public DataTable ExecuteTable(string sql, int pageSize, int pageIndex, out int allCount, params zParameter[] parameters)
        {
            string _sql = sql;
            DbParameter[] ps = RenderSql(ref _sql, parameters);
            return ExecuteTable(_sql, pageSize, pageIndex, out allCount, ps);
        }

        #endregion
        #region 查对象
        public T ExecuteOneObject<T>(string sql, params DbParameter[] parameters) where T : new()
        {
            return ExecuteObject<T>(sql, 0, 0, parameters).FirstOrDefault();
        }

        public List<T> ExecuteObject<T>(string sql, params DbParameter[] parameters) where T : new()
        {
            return ExecuteObject<T>(sql, 0, 0, parameters);
        }

        public List<T> ExecuteObject<T>(string sql, PageInfo pageinfo, params DbParameter[] parameters) where T : new()
        {
            return ExecuteObject<T>(sql, pageinfo.PageSize, pageinfo.PageIndex, parameters);
        }

        public List<T> ExecuteObject<T>(string sql, PageInfo pageinfo, out int allCount, params DbParameter[] parameters) where T : new()
        {
            return ExecuteObject<T>(sql, pageinfo.PageSize, pageinfo.PageIndex, out allCount, parameters);
        }

        public List<T> ExecuteObject<T>(string sql, int pageSize, int pageIndex, params DbParameter[] parameters) where T : new()
        {
            List<T> list = new List<T>();
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.AddRange(parameters);
                _dbCommand.CommandText = GetPageSql(sql, pageSize, pageIndex);
                lock (ObjectExtension.Locker)
                {
                    using (IDataReader reader = _dbCommand.ExecuteReader())
                    {
                        list = this.ReaderToObject<T>(reader);
                    }
                }
            });
            return list;
        }

        public List<T> ExecuteObject<T>(string sql, int pageSize, int pageIndex, out int allCount, params DbParameter[] parameters) where T : new()
        {
            int resall = 0;
            List<T> list = ExecuteObject<T>(sql, pageSize, pageIndex, parameters);
            RunSql(_dbCommand =>
            {
                _dbCommand.CommandText = GetCountSql(sql);
                lock (ObjectExtension.Locker)
                {
                    using (IDataReader reader = _dbCommand.ExecuteReader())
                    {
                        DataTable dtcount = this.ReaderToTable(reader);
                        if (dtcount.IsOneLine())
                        {
                            int.TryParse(dtcount.Rows[0][0].ToString(), out resall);
                        }
                        else
                        {
                            resall = 0;
                        }
                    }
                }
            });
            allCount = resall;
            return list;
        }
        #endregion
        #region 增删改
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="fun">大量sql时每执行一条sql的回调(当前行索引,当前行发生的数据变更,总数据变更)</param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(List<string> sql, Action<int, int, int> fun = null)
        {
            if (sql == null)
                return 0;
            string tmpStr = "";
            int influenceRowCount = 0;
            RunSql(_dbCommand =>
            {
                for (int i = 0; i < sql.Count; i++)
                {
                    tmpStr = sql[i];
                    _dbCommand.CommandText = tmpStr;
                    int cntnow = _dbCommand.ExecuteNonQuery();
                    influenceRowCount += cntnow;
                    if (fun != null)
                    {
                        fun(i, cntnow, influenceRowCount);
                    }
                }
            });
            return influenceRowCount;
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql">执行多条sql</param>
        /// <returns></returns>
        public int ExecuteNonQuery(params string[] sql)
        {
            if (sql == null || sql.Length == 0)
                return 0;
            return ExecuteNonQuery(sql.ToList());
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql">执行多条sql</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(new List<string>() { sql });
        }


        /// <summary>
        /// 批量插入表
        /// </summary>
        /// <param name="dt"></param>
        public void InsertTable(DataTable dt)
        {
            FastInsertTable(dt);
        }
        #endregion
        #region 对象增删改查
        public int Save(TableEntityBase info)
        {
            IDbDataParameter[] dbprams = info.GetPrimaryKey().Select(a =>
            {
                if (a.GetAttribute<PrimaryKeyAttribute>() != null)
                {
                    if (string.IsNullOrEmpty(a.GetValue(info, null)?.ToString()))
                    {
                        throw new DataBaseException("字段:" + a.Name + "是主键,在保存时不能为空");
                    }
                }
                IDbDataParameter p = GetDbDataParameter(a, info);
                object value = a.GetValue(info, null);
                p.Value = (value == null || string.IsNullOrEmpty(value.ToString()) ? DBNull.Value : value);
                return p;
            }).ToArray();
            int i = 0;
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                string tablename = info.GetTableName();
                string where = string.Join(" and ", info.GetPrimaryKey().Select(a => a.Name + "=" + GetPramCols(a.Name)));
                _dbCommand.CommandText = string.Format(_selectcount, tablename, where);
                IDataReader reader = _dbCommand.ExecuteReader();
                if (reader.Read())
                    i = reader.GetValue(0).ToString().ToInt();
            });
            if (i == 0)
                return Insert(info);
            else
                return Update(info);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Insert(TableEntityBase info)
        {
            int res = 0;
            IDbDataParameter[] dbprams = info.GetAllField().Select(a =>
            {
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray();
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                _dbCommand.CommandText = string.Format(_insert, info.GetTableName(),
                    string.Join(",", dbprams.Select(a => a.ParameterName)),
                    string.Join(",", dbprams.Select(a => GetPramCols(a.ParameterName)))
                     );
                res = _dbCommand.ExecuteNonQuery();
            });
            #region 处理子表
            _InsertChildren(info);
            #endregion
            return res;
        }

        /// <summary>
        /// 更新，按照主键进行更新
        /// </summary>
        /// <exception cref="DataBaseException"></exception>
        /// 没有主键
        /// 主键不是全部都有值
        /// <param name="info"></param>
        /// <returns></returns>
        public int Update(TableEntityBase info)
        {
            int res = 0;
            IDbDataParameter[] dbprams = info.GetFieldWithoutPrimaryKey().Select(a =>
            {
                if (a.GetAttribute<PrimaryKeyAttribute>() != null)
                {
                    if (string.IsNullOrEmpty(a.GetValue(info, null)?.ToString()))
                    {
                        throw new DataBaseException("字段:" + a.Name + "是主键,在更新时不能为空");
                    }
                }
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray().Concat(info.GetPrimaryKey().Select(a =>
            {
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            })).ToArray();
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                string tablename = info.GetTableName();
                string set = string.Join(",", info.GetFieldWithoutPrimaryKey().Select(a => a.Name + "=" + GetPramCols(a.Name)));
                string where = string.Join(" and ", info.GetPrimaryKey().Select(a => a.Name + "=" + GetPramCols(a.Name)));
                if (string.IsNullOrEmpty(where))
                {
                    throw new DataBaseException("没有主键,不能更新");
                }
                _dbCommand.CommandText = string.Format(_update, tablename, set, where);
                res = _dbCommand.ExecuteNonQuery();
            });
            #region 处理子表
            _DeleteChildren(info);
            _InsertChildren(info);
            return res;
            #endregion
        }

        /// <summary>
        /// 删除,按照主键进行删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Delete(TableEntityBase info)
        {
            int res = 0;
            _DeleteChildren(info);
            IDbDataParameter[] dbprams = info.GetPrimaryKey().Select(a =>
            {
                if (a.GetAttribute<PrimaryKeyAttribute>() != null)
                {
                    if (string.IsNullOrEmpty(a.GetValue(info, null)?.ToString()))
                    {
                        throw new DataBaseException("字段:" + a.Name + "是主键,在删除时不能为空");
                    }
                }
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray();
            string tablename = info.GetTableName();
            string where = string.Join(" and ", info.GetPrimaryKey().Select(a => a.Name + "=" + GetPramCols(a.Name)));
            if (string.IsNullOrEmpty(where))
            {
                throw new DataBaseException("没有主键,不能删除");
            }
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                _dbCommand.CommandText = string.Format(_delete, tablename, where);
                res = _dbCommand.ExecuteNonQuery();
            });
            return res;
        }

        /// <summary>
        /// 删除，按照非null的进行删除，全null抛出异常
        /// </summary>
        /// <exception cref="DataBaseException"></exception>
        /// 没有删除条件
        /// <param name="info"></param>
        /// <returns></returns>
        public int DeleteList(TableEntityBase info)
        {
            int res = 0;
            PropertyInfo[] Allprop = info.GetAllField().Where(a =>
            {
                return a.GetValue(info, null) != null;
            }).ToArray();
            IDbDataParameter[] dbprams = Allprop.Select(a =>
            {
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray();
            string tablename = info.GetTableName();
            string where = string.Join(" and ", Allprop.Select(a => a.Name + "=" + GetPramCols(a.Name)));
            if (string.IsNullOrEmpty(where))
            {
                throw new DataBaseException("没有删除条件，不能删除");
            }
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                _dbCommand.CommandText = string.Format(_delete, tablename, where);
                res = _dbCommand.ExecuteNonQuery();
            });
            return res;
        }

        /// <summary>
        /// 查询,按主键查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public T Select<T>(T info) where T : TableEntityBase
        {
            T res = default(T);
            IDbDataParameter[] dbprams = info.GetPrimaryKey().Select(a =>
            {
                if (a.GetAttribute<PrimaryKeyAttribute>() != null)
                {
                    if (string.IsNullOrEmpty(a.GetValue(info, null)?.ToString()))
                    {
                        throw new DataBaseException("字段:" + a.Name + "是主键,在查询时不能为空");
                    }
                }
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray();
            if (dbprams.IsEmpty())
            {
                throw new DataBaseException($"表{info.GetTableName()}没有主键,不能使用select方法");
            }
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                string tablename = info.GetTableName();
                string select = string.Join(",", info.GetAllField().Select(a => GetFieldName(a.Name)));
                string where = string.Join(" and ", info.GetPrimaryKey().Select(a => a.Name + "=" + GetPramCols(a.Name)));
                _dbCommand.CommandText = string.Format(_select, select, tablename, where);
                IDataReader reader = _dbCommand.ExecuteReader();
                res = this.ReaderToEntity(info.GetType(), reader).FirstOrDefault() as T;
            });
            res = _SelectChildren(res);
            return res;
        }

        /// <summary>
        /// 查询,按照所有不为空的条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<T> SelectList<T>(T info) where T : TableEntityBase, new()
        {
            List<T> res = new List<T>();
            PropertyInfo[] Allprop = info.GetAllField().Where(a =>
            {
                return a.GetValue(info, null) != null;
            }).ToArray();
            IDbDataParameter[] dbprams = Allprop.Select(a =>
            {
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray();
            RunSql(_dbCommand =>
            {
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                string tablename = info.GetTableName();
                string select = string.Join(",", info.GetAllField().Select(a => GetFieldName(a.Name)));
                string where = string.Join(" and ", Allprop.Select(a => a.Name + "=" + GetPramCols(a.Name)));
                _dbCommand.CommandText = string.Format(_select, select, tablename, where.IsEmpty(" 1=1 "));
                IDataReader reader = _dbCommand.ExecuteReader();
                res = this.ReaderToEntity(info.GetType(), reader).Select(a => a as T).ToList();
            });
            res.ForEach(a => a = _SelectChildren(a));
            return res;
        }
        #endregion
        #region 存储过程
        public T ExecuteProcedure<T>(T info) where T : ProcedureEntityBase
        {
            T res = default(T);
            RunSql(_dbCommand =>
            {
                _dbCommand.CommandType = CommandType.StoredProcedure;
                IDbDataParameter[] dbprams = info.GetAllProcedureField().Select(a =>
                {
                    ProcedureFieldAttribute attr = a.GetAttribute<ProcedureFieldAttribute>();
                    IDbDataParameter p = GetDbDataParameter(a, info);
                    object value = a.GetValue(info, null);
                    p.Value = (value == null || string.IsNullOrEmpty(value.ToString()) ? DBNull.Value : value);
                    p.Direction = attr.Direction;
                    p.ParameterName = attr.Fieldname;
                    p.Size = attr.Size;
                    return p;
                }).ToArray();
                _dbCommand.Parameters.Clear();
                _dbCommand.Parameters.AddRange(dbprams);
                _dbCommand.CommandText = info.GetProcedureName();
                _dbCommand.ExecuteNonQuery();
                res = ReadDataParameter(dbprams, info);
            });
            return res;
        }

        #endregion
        #endregion
        #region 事务操作


        /// <summary>
        /// 判断是否在事务中
        /// </summary>
        /// <returns></returns>
        public virtual bool HasTransaction()
        {
            return _zTransaction != null && _zTransaction.Transaction != null;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public virtual zDbTransaction BeginTransaction(IsolationLevel? iso = null)
        {
            if (_zTransaction != null)
                return new zDbTransaction();
            OpenConnection();
            _zTransaction = new zDbTransaction(iso.HasValue ? _dbConnection.BeginTransaction(iso.Value) : _dbConnection.BeginTransaction());
            Action done = () =>
            {
                if (this._dbConnection.State != ConnectionState.Closed)
                {
                    this._dbConnection.Close();
                }
                this._dbConnection.Dispose();
                this._dbConnection = null;
            };
            _zTransaction.AfterCommit = done;
            _zTransaction.AfterRollback = done;
            return _zTransaction;
        }


        #endregion
        #region 链接操作
        //public virtual void Open()
        //{
        //    //连接数据库
        //    if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
        //    {
        //        _dbConnection = GetDbConnection(_dbConnectionInfoStr);
        //        _dbConnection.Open();
        //    }
        //    if (_dbConnection.State == ConnectionState.Closed)
        //    {
        //        _dbConnection.Open();
        //    }
        //    else if (_dbConnection.State == ConnectionState.Broken)
        //    {
        //        _dbConnection.Close();
        //        _dbConnection.Open();
        //    }
        //}

        void OpenConnection()
        {
            if (_dbConnection == null)
            {
                _dbConnection = GetDbConnection(_dbConnectionInfoStr);
                _dbConnection.Open();
            }
            else if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            else if (_dbConnection.State == ConnectionState.Broken)
            {
                _dbConnection.Close();
                _dbConnection.Open();
            }
        }

        protected void RunSql(Action<DbCommand> comm, Action done = null)
        {
            Action Done = () =>
            {
                if (this._dbConnection.State != ConnectionState.Closed)
                {
                    this._dbConnection.Close();
                }
                this._dbConnection.Dispose();
                this._dbConnection = null;
            };
            DbCommand _dbCommand = null;
            try
            {
                OpenConnection();
                _dbCommand = _dbConnection.CreateCommand();
                _dbCommand.CommandTimeout = 3600;
                if (HasTransaction())
                {
                    _dbCommand.Transaction = _zTransaction.Transaction;
                }
                comm?.Invoke(_dbCommand);
                _dbCommand.Dispose();
                if (!HasTransaction())
                {
                    Done();
                }
            }
            catch (Exception ex)
            {
                Done();
                //if (ex.InnerMessage().Equals("池式连接请求超时"))
                //    throw new FailException("池式连接请求超时");
                string sql = _dbCommand?.CommandText;
                object obj = _dbCommand?.Parameters;
                if (sql.IsEmpty())
                    throw new DataBaseException(ex.InnerMessage());
                else if (obj == null)
                    throw new DataBaseException(ex.InnerMessage(), sql);
                else
                    throw new DataBaseException(ex.InnerMessage(), sql, obj);
            }
            finally
            {
                done?.Invoke();
            }
        }


        #endregion
        #region 辅助方法
        private List<T> ReaderToObject<T>(IDataReader reader) where T : new()
        {
            List<T> res = new List<T>();
            while (reader.Read())
            {
                T t = new T();
                reader.FieldCount.ForEach(i =>
                {
                    t.SetPropertyValue(reader.GetName(i), reader.GetValue(i));
                });
                res.Add(t);
            }
            reader.Close();
            return res;
        }

        /// <summary>
        /// 读取的内容转换为datatable
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// 
        private DataTable ReaderToTable(IDataReader reader)
        {
            DataTable dt = new DataTable();
            for (int count = 0; count < reader.FieldCount; count++)
            {
                dt.Columns.Add(reader.GetName(count), reader.GetFieldType(count));
            }
            while (reader.Read())
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader.GetValue(i);
                }
                dt.Rows.Add(row);
            }
            reader.Close();
            return dt;
        }

        /// <summary>
        /// 读取的内容转换为Entity
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// 
        private List<TableEntityBase> ReaderToEntity(Type t, IDataReader reader)
        {
            List<TableEntityBase> res = new List<TableEntityBase>();
            int fieldCount = reader.FieldCount;
            while (reader.Read())
            {
                TableEntityBase entity = (TableEntityBase)Activator.CreateInstance(
                                          t,
                                          BindingFlags.Instance | BindingFlags.Public,
                                          null,
                                          new object[] { },
                                          null);
                for (int i = 0; i < fieldCount; i++)
                {
                    entity.SetPropertyValue(reader.GetName(i), reader.GetValue(i).ToString());
                }
                res.Add(entity);
            }
            reader.Close();
            return res;
        }

        /// <summary>
        /// 读取存储过程的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbprams"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private T ReadDataParameter<T>(IDbDataParameter[] dbprams, T t) where T : ProcedureEntityBase
        {
            dbprams.ForEach(p =>
            {
                var field = t.GetProcedureField(p.ParameterName);
                if (field == null)
                    return;
                var attr = field.GetAttribute<ProcedureFieldAttribute>();
                if (attr.Direction != ParameterDirection.Input)
                {
                    field.SetValue(t, GetParameterValue(p, field), null);
                }
            });
            return t;
        }

        DbParameter GetDbDataParameter(PropertyInfo p, EntityBase info)
        {
            DbTypeAttribute dba = p.GetAttribute<DbTypeAttribute>();
            return GetParameter(p.Name, p.GetValue(info, null), dba?.DbType);
        }

        /// <summary>
        /// 获取一个参数的值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        object GetParameterValue(IDbDataParameter p, PropertyInfo pinfo)
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

        public override string ToString()
        {
            return _dbConnectionInfoStr.ToString();
        }

        void _DeleteChildren(TableEntityBase info)
        {
            PropertyInfo[] ForeignKeys = info.GetForeignKey();
            if (!ForeignKeys.IsEmpty()) //有子表,处理子表
            {
                ForeignKeys.ForEach(key =>  //主表里记录子表的列表
                {
                    if (key.IsArray() && key.GetChildren().BaseOn<TableEntityBase>())
                    {
                        List<ForeignKeyAttribute> attrs = key.GetAttributes<ForeignKeyAttribute>();
                        TableEntityBase edel = Activator.CreateInstance(key.GetChildren()) as TableEntityBase;
                        attrs.ForEach(attr =>
                        {
                            edel.GetType().GetProperty(attr.ChildrenKey).SetValue(edel, info.GetPropertyValue(attr.ParentKey), null);
                        });
                        _DeleteChildren(edel);
                        DeleteList(edel);
                    }
                    else
                    {
                        throw new Exception($"属性{key.Name}必须是EntityBase的广义数组");
                    }
                });
            }
        }

        void _InsertChildren(TableEntityBase info)
        {
            PropertyInfo[] ForeignKeys = info.GetForeignKey();
            if (!ForeignKeys.IsEmpty()) //有子表,处理子表
            {
                ForeignKeys.ForEach(key =>  //主表里记录子表的列表
                {
                    if (key.IsArray() && key.GetChildren().BaseOn<TableEntityBase>())
                    {
                        key.ForEach(info, (TableEntityBase item) =>
                        {
                            List<ForeignKeyAttribute> attrs = key.GetAttributes<ForeignKeyAttribute>();
                            attrs.ForEach(a =>
                            {
                                item.SetPropertyValue(a.ChildrenKey, info.GetPropertyValue(a.ParentKey));
                            });
                            Insert(item);
                        });
                    }
                    else
                    {
                        throw new Exception($"属性{key.Name}必须是EntityBase的广义数组");
                    }
                });
            }
        }

        T _SelectChildren<T>(T info) where T : TableEntityBase
        {
            if (info == null)
                return info;
            PropertyInfo[] ForeignKeys = info.GetForeignKey();
            if (!ForeignKeys.IsEmpty()) //有子表,处理子表
            {
                ForeignKeys.ForEach(key =>  //主表里记录子表的列表
                {
                    if (key.IsArray() && key.GetChildren().BaseOn<TableEntityBase>())
                    {
                        Type t = key.GetChildren();
                        var item = (TableEntityBase)Activator.CreateInstance(
                                          t,
                                          BindingFlags.Instance | BindingFlags.Public,
                                          null,
                                          new object[] { },
                                          null);
                        List<ForeignKeyAttribute> attrs = key.GetAttributes<ForeignKeyAttribute>();
                        attrs.ForEach(a =>
                        {
                            item.SetPropertyValue(a.ChildrenKey, info.GetPropertyValue(a.ParentKey));
                        });
                        List<TableEntityBase> res = SelectList(item);
                        key.SetArrValue(info, res);
                    }
                    else
                    {
                        throw new Exception($"属性{key.Name}必须是EntityBase的广义数组");
                    }
                });
            }
            return info;
        }

        /// <summary>
        /// 渲染参数化sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DbParameter[] RenderSql(ref string sql, params zParameter[] parameters)
        {
            List<DbParameter> res = new List<DbParameter>();
            if (!sql.Contains("{{"))  //没有参数符号的,跳过
            {
                return parameters?.Select(p => GetParameter(p.Name, p.Value, p.Type)).ToArray();
            }
            int pnameinx = 1;
            Func<string> newp = () =>
            {
                return $"p{pnameinx++}";
            };
            Regex rin = new Regex(@"{{[^@]([^@]+?)}}");
            Regex rin0 = new Regex(@"(?<={{)[^@]([^@]+?)(?=}})");
            Regex errcs = new Regex(@"{{| |}}|@");
            while (rin0.IsMatch(sql))
            {
                string cs = rin0.Match(sql).Value;
                if (errcs.IsMatch(cs))
                    throw new Exception("sql匹配失败,请检查占位符是否有特殊字符或{{标签没有闭合");
                zParameter pram = parameters?.FirstOrDefault(p => p.Name == cs);
                if (pram != null) //参数存在,则拼接进去
                {
                    //先替换掉可空标记
                    Regex regex = new Regex(@"{{@([^@]*?){{" + cs + @"}}([^@]*?)@}}");   //可空标记替换符
                    sql = regex.Replace(sql, m =>
                    {
                        return $" {m.Value.Substring(3, m.Value.Length - 6)} ";
                    });
                    //替换占位符,产生参数数组
                    string zstr = "{{" + cs + "}}";
                    if (pram.IsArray)  //数组参数
                    {
                        var arrv = (pram.Value as IEnumerable<object>).ToArray();
                        if (arrv == null)
                            throw new Exception("数组参数类型错误");
                        if (arrv.Count() == 0)
                            throw new Exception("数组参数个数不能为0");
                        string[] pnames = arrv.Select(p =>
                          {
                              string pname = newp();
                              res.Add(GetParameter(pname, p, pram.Type));
                              return GetPramCols(pname);
                          }).ToArray();
                        sql = sql.ReplaceOne(zstr, string.Join(",", pnames));
                    }
                    else  //单个参数
                    {
                        string pname = newp();
                        sql = sql.ReplaceOne(zstr, $" {GetPramCols(pname)} ");
                        res.Add(GetParameter(pname, pram.Value, pram.Type));
                    }
                }
                else //参数不存在,删除可空标记占位符
                {
                    Regex rnull = new Regex(@"{{@([^@]+?){{" + cs + @"}}([^@]+?)@}}");
                    sql = rnull.Replace(sql, " ");
                    //参数不存在,但是sql不可空,则报错
                    Regex rnotnull = new Regex(@"{{" + cs + @"}}");
                    if (rnotnull.IsMatch(sql))
                        throw new Exception($"参数{cs}不可为空");
                }
            }
            //如果还有残余的占位符,说明sql有问题了
            {
                Regex regex = new Regex(@"[{{|}}]");
                if (regex.IsMatch(sql))
                    throw new Exception("占位符异常");
            }
            return res.ToArray();
        }

        #endregion
    }
}
