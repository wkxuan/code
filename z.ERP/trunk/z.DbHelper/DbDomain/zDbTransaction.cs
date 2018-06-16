using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace z.DBHelper.DbDomain
{
    public class zDbTransaction : IDisposable
    {
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
            BeforeCommit?.Invoke();
            Transaction?.Commit();
            AfterCommit?.Invoke();
        }
        public void Rollback()
        {
            BeforeRollback?.Invoke();
            Transaction?.Rollback();
            AfterRollback?.Invoke();
        }

        public void Dispose()
        {
            Transaction.Dispose();
        }
    }
}
