using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    public class zDbTransaction : IDisposable
    {
        /// <summary>
        /// 假事务
        /// </summary>
        public zDbTransaction()
        {
        }

        /// <summary>
        /// 真事务
        /// </summary>
        /// <param name="t"></param>
        public zDbTransaction(DbTransaction t)
        {
            Transaction = t;
        }

        public DbTransaction Transaction
        {
            get;
            set;
        }

        public Action BeforeCommit
        {
            get;
            set;
        }
        public Action AfterCommit
        {
            get;
            set;
        }
        public Action BeforeRollback
        {
            get;
            set;
        }
        public Action AfterRollback
        {
            get;
            set;
        }


        public void Commit()
        {
            if (Transaction != null)
            {
                BeforeCommit?.Invoke();
                Transaction?.Commit();
                AfterCommit?.Invoke();
            }
        }
        public void Rollback()
        {
            if (Transaction != null)
            {
                BeforeRollback?.Invoke();
                Transaction?.Rollback();
                AfterRollback?.Invoke();
            }
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
        }
    }
}
