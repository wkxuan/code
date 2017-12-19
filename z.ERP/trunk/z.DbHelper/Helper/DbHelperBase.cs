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
        /// <summary>
        /// 数据库命令对象
        /// </summary>
        protected DbCommand _dbCommand;

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
        /// 取数据库操作对象
        /// </summary>
        /// <param name="dbconnection"></param>
        /// <returns></returns>
        protected abstract DbCommand GetDbCommand(DbConnection dbconnection);

        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="p"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected abstract IDbDataParameter GetDbDataParameter(PropertyInfo p, EntityBase info);
        #endregion
        #region 构造

        public DbHelperBase(string ConnectionStr)
        {
            _dbConnectionInfoStr = ConnectionStr;
            Open();
        }

        public DbHelperBase(IDbConnectionInfo conn)
            : this(conn.ToConnectionString())
        {
            _dbConnectionInfo = conn;
        }


        ~DbHelperBase()
        {
            Close();
        }
        #endregion
        #region 数据操作
        #region 查表

        public DataTable ExecuteTable(string sql)
        {
            return ExecuteTable(sql, 0, 0);
        }

        public DataTable ExecuteTable(string sql, PageInfo pageinfo)
        {
            return ExecuteTable(sql, pageinfo.PageSize, pageinfo.PageIndex);
        }

        public DataTable ExecuteTable(string sql, PageInfo pageinfo, out int allCount)
        {
            return ExecuteTable(sql, pageinfo.PageSize, pageinfo.PageIndex, out allCount);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sql, int pageSize, int pageIndex)
        {
            DataTable dt = new DataTable();
            try
            {
                if (_dbCommand.Connection.State != ConnectionState.Open)
                    Open();
                _dbCommand.CommandText = GetPageSql(sql, pageSize, pageIndex);
                lock (ObjectExtension.Locker)
                {
                    using (IDataReader reader = _dbCommand.ExecuteReader())
                    {
                        dt = this.ReaderToTable(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, sql);
            }
            return dt;
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="allCount"></param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sql, int pageSize, int pageIndex, out int allCount)
        {
            DataTable dt = ExecuteTable(sql, pageSize, pageIndex);
            try
            {
                if (_dbCommand.Connection.State != ConnectionState.Open)
                    Open();
                _dbCommand.CommandText = GetCountSql(sql);
                lock (ObjectExtension.Locker)
                {
                    using (IDataReader reader = _dbCommand.ExecuteReader())
                    {
                        DataTable dtcount = this.ReaderToTable(reader);
                        if (dtcount.IsOneLine())
                        {
                            int.TryParse(dtcount.Rows[0][0].ToString(), out allCount);
                        }
                        else
                        {
                            allCount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, sql);
            }
            return dt;
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
            try
            {
                if (_dbCommand.Connection.State != ConnectionState.Open)
                    Open();
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
                return influenceRowCount;
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, _dbCommand.CommandText);
            }
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql">执行多条sql</param>
        /// <returns></returns>
        public int ExecuteNonQuery(params string[] sql)
        {
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
        #endregion
        #region 对象增删改查
        public int Save(EntityBase info)
        {
            if (_dbCommand.Connection.State != ConnectionState.Open)
                Open();
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
            _dbCommand.Parameters.Clear();
            _dbCommand.Parameters.AddRange(dbprams);
            string tablename = info.GetTableName();
            string where = string.Join(" and ", info.GetPrimaryKey().Select(a => a.Name + "=" + GetPramCols(a.Name)));
            _dbCommand.CommandText = string.Format(_selectcount, tablename, where);
            IDataReader reader = _dbCommand.ExecuteReader();
            int i = 0;
            if (reader.Read())
                i = reader.GetValue(0).ToString().ToInt();
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
        public int Insert(EntityBase info)
        {
            int res;
            if (_dbCommand.Connection.State != ConnectionState.Open)
                Open();
            IDbDataParameter[] dbprams = info.GetAllField().Select(a =>
            {
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray();
            _dbCommand.Parameters.Clear();
            _dbCommand.Parameters.AddRange(dbprams);
            _dbCommand.CommandText = string.Format(_insert, info.GetTableName(),
                string.Join(",", dbprams.Select(a => a.ParameterName)),
                string.Join(",", dbprams.Select(a => ":" + a.ParameterName))
                );
            try
            {
                res = _dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, _dbCommand.CommandText, info);
            }
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
        public int Update(EntityBase info)
        {
            int res;
            if (_dbCommand.Connection.State != ConnectionState.Open)
                Open();
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
            try
            {
                res = _dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, _dbCommand.CommandText, info);
            }
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
        public int Delete(EntityBase info)
        {
            int res;
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
            _dbCommand.Parameters.Clear();
            _dbCommand.Parameters.AddRange(dbprams);
            _dbCommand.CommandText = string.Format(_delete, tablename, where);
            try
            {
                res = _dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, _dbCommand.CommandText, info);
            }
            _DeleteChildren(info);
            return res;
        }

        /// <summary>
        /// 删除，按照非null的进行删除，全null抛出异常
        /// </summary>
        /// <exception cref="DataBaseException"></exception>
        /// 没有删除条件
        /// <param name="info"></param>
        /// <returns></returns>
        public int DeleteList(EntityBase info)
        {
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
            _dbCommand.Parameters.Clear();
            _dbCommand.Parameters.AddRange(dbprams);
            _dbCommand.CommandText = string.Format(_delete, tablename, where);
            try
            {
                return _dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, _dbCommand.CommandText, info);
            }
        }

        /// <summary>
        /// 查询,按主键查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public T Select<T>(T info) where T : EntityBase
        {
            T res;
            if (_dbCommand.Connection.State != ConnectionState.Open)
                Open();
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
            _dbCommand.Parameters.Clear();
            _dbCommand.Parameters.AddRange(dbprams);
            string tablename = info.GetTableName();
            string select = string.Join(",", info.GetAllField().Select(a => a.Name));
            string where = string.Join(" and ", info.GetPrimaryKey().Select(a => a.Name + "=" + GetPramCols(a.Name)));
            _dbCommand.CommandText = string.Format(_select, select, tablename, where);
            try
            {
                IDataReader reader = _dbCommand.ExecuteReader();
                res = this.ReaderToEntity(info.GetType(), reader).FirstOrDefault() as T;
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, _dbCommand.CommandText, info);
            }
            res = _SelectChildren(res);
            return res;
        }

        /// <summary>
        /// 查询,按照所有不为空的条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<T> SelectList<T>(T info) where T : EntityBase, new()
        {
            List<T> res;
            if (_dbCommand.Connection.State != ConnectionState.Open)
                Open();
            PropertyInfo[] Allprop = info.GetAllField().Where(a =>
            {
                return a.GetValue(info, null) != null;
            }).ToArray();
            IDbDataParameter[] dbprams = Allprop.Select(a =>
            {
                IDbDataParameter p = GetDbDataParameter(a, info);
                return p;
            }).ToArray();
            _dbCommand.Parameters.Clear();
            _dbCommand.Parameters.AddRange(dbprams);
            string tablename = info.GetTableName();
            string select = string.Join(",", info.GetAllField().Select(a => a.Name));
            string where = string.Join(" and ", Allprop.Select(a => a.Name + "=" + GetPramCols(a.Name)));
            _dbCommand.CommandText = string.Format(_select, select, tablename, where);
            try
            {
                IDataReader reader = _dbCommand.ExecuteReader();
                res = this.ReaderToEntity(info.GetType(), reader).Select(a => a as T).ToList();
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, _dbCommand.CommandText, info);
            }
            res.ForEach(a => a = _SelectChildren(a));
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
            return _dbCommand != null && _dbCommand.Transaction != null;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public virtual DbTransaction BeginTransaction()
        {
            if (_dbCommand == null || _dbCommand.Transaction == null)
            {
                if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                    Open();
                _dbCommand.Transaction = _dbConnection.BeginTransaction();
            }
            return _dbCommand.Transaction;
        }


        #endregion
        #region 链接操作
        public virtual void Open()
        {
            //连接数据库
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
            {
                _dbConnection = GetDbConnection(_dbConnectionInfoStr);
                _dbConnection.Open();
            }
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            else if (_dbConnection.State == ConnectionState.Broken)
            {
                _dbConnection.Close();
                _dbConnection.Open();
            }
            _dbCommand = GetDbCommand(_dbConnection);
        }

        public virtual void Close()
        {
            try
            {
                if (_dbConnection != null)
                {
                    if (_dbConnection.State != ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                        _dbCommand.Dispose();
                    }
                }
            }
            catch
            {

            }
        }
        #endregion
        #region 辅助方法

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
                dt.Columns.Add(reader.GetName(count).ToUpper(), reader.GetFieldType(count));
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
        /// 读取的内容转换为datatable
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// 
        private List<EntityBase> ReaderToEntity(Type t, IDataReader reader)
        {
            List<EntityBase> res = new List<EntityBase>();
            int fieldCount = reader.FieldCount;
            while (reader.Read())
            {
                EntityBase entity = (EntityBase)Activator.CreateInstance(
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

        public override string ToString()
        {
            return _dbConnectionInfoStr.ToString();
        }

        void _DeleteChildren(EntityBase info)
        {
            PropertyInfo[] ForeignKeys = info.GetForeignKey();
            if (!ForeignKeys.IsEmpty()) //有子表,处理子表
            {
                ForeignKeys.ForEach(key =>  //主表里记录子表的列表
                {
                    if (key.IsArray() && key.GetChildren().BaseOn<EntityBase>())
                    {
                        List<ForeignKeyAttribute> attrs = key.GetAttributes<ForeignKeyAttribute>();
                        EntityBase edel = Activator.CreateInstance(key.GetChildren()) as EntityBase;
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

        void _InsertChildren(EntityBase info)
        {
            PropertyInfo[] ForeignKeys = info.GetForeignKey();
            if (!ForeignKeys.IsEmpty()) //有子表,处理子表
            {
                ForeignKeys.ForEach(key =>  //主表里记录子表的列表
                {
                    if (key.IsArray() && key.GetChildren().BaseOn<EntityBase>())
                    {
                        key.ForEach(info, (EntityBase item) =>
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

        T _SelectChildren<T>(T info) where T : EntityBase
        {
            if (info == null)
                return info;
            PropertyInfo[] ForeignKeys = info.GetForeignKey();
            if (!ForeignKeys.IsEmpty()) //有子表,处理子表
            {
                ForeignKeys.ForEach(key =>  //主表里记录子表的列表
                {
                    if (key.IsArray() && key.GetChildren().BaseOn<EntityBase>())
                    {
                        Type t = key.GetChildren();
                        var item = (EntityBase)Activator.CreateInstance(
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
                        List<EntityBase> res = SelectList(item);
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
        #endregion
    }
}
