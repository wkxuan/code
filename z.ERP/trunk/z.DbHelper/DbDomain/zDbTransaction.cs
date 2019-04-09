using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using z.Extensions;

namespace z.DBHelper.DBDomain
{
    public class zDbTransaction : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="isReal">这次是真事务还是假事务</param>
        public zDbTransaction(DbTransaction t, bool isReal)
        {
            isRealTransaction = isReal;
            Transaction = t;
        }

        /// <summary>
        /// 事务标记
        /// 假事务时不执行提交,回滚操作,销毁对象时保留事务对象
        /// </summary>
        bool isRealTransaction;

        /// <summary>
        /// 本次事务对象
        /// </summary>
        internal DbTransaction Transaction
        {
            get;
            set;
        }

        internal Action BeforeCommit
        {
            get;
            set;
        }
        internal Action AfterCommit
        {
            get;
            set;
        }
        internal Action BeforeRollback
        {
            get;
            set;
        }
        internal Action AfterRollback
        {
            get;
            set;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (!isRealTransaction)
                return;
            BeforeCommit?.Invoke();
            Transaction?.Commit();
            AfterCommit?.Invoke();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="Point">事务记录点</param>
        public void Rollback(string Point = null)
        {
            //部分回滚
            if (!Point.IsEmpty())
            {
                if (Point.IsEmpty())
                {
                    throw new Exception("事务保存点为空");
                }
#if SqlServer
                else if (Transaction is SqlTransaction)
                {
                    (Transaction as SqlTransaction)?.Rollback(Point);
                }
#endif
#if ORACLE
                else if (Transaction is OracleTransaction)
                {
                    (Transaction as OracleTransaction)?.Rollback(Point);
                }
#endif
                else
                {
                    throw new Exception("当前数据库不支持事务保存");
                }
                return;
            }
            if (!isRealTransaction)
                return;
            BeforeRollback?.Invoke();
            Transaction?.Rollback();
            AfterRollback?.Invoke();
        }

        /// <summary>
        /// 设置事务记录点
        /// </summary>
        /// <param name="Point"></param>
        public void Save(string Point)
        {
            if (Point.IsEmpty())
            {
                throw new Exception("事务保存点为空");
            }
#if SqlServer
            else if (Transaction is SqlTransaction)
            {
                (Transaction as SqlTransaction)?.Save(Point);
            }
#endif
#if ORACLE
            else if (Transaction is OracleTransaction)
            {
                (Transaction as OracleTransaction)?.Save(Point);
            }
#endif
            else
            {
                throw new Exception("当前数据库不支持事务保存");
            }
        }

        public void Dispose()
        {
            if (!isRealTransaction)
                return;
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
        }
    }
}
