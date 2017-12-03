using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using z.Context;
using z.DbHelper.DbDomain;
using z.DBHelper.Helper;
using z.Extensions;
using z.IOC.RealProxyIOC;
using z.IOC.Simple;
using z.Verify;

namespace z.ERP.Services
{
    #region 基础实现
    public class ServiceBase
    {
        SimpleIOC ioc;
        public ServiceBase()
        {
            List<Type> mrs = new List<Type>();
            //mrs.Add(typeof(TestServiceOverride));
            ioc = new SimpleIOC(mrs);
        }

        protected DbHelperBase DbHelper
        {
            get
            {
                return _db;
            }
        }

        static readonly DbHelperBase _db = new OracleDbHelper(ConfigExtension.GetConfig("connection"));

        #region Service  
        public TestService TestService
        {
            get
            {
                return ioc.Create<TestService>();
            }
        }
        public CommonService CommonService
        {
            get
            {
                return ioc.Create<CommonService>();
            }
        }
        public DataService DataService
        {
            get
            {
                return ioc.Create<DataService>();
            }
        }
        #endregion

        #region 通用方法

        /// <summary>
        /// 获取新的记录编号
        /// </summary>
        /// <returns></returns>
        public string NewINC(EntityBase info)
        {
            return NewINC(info.GetTableName());
        }
        /// <summary>
        /// 获取新的记录编号
        /// </summary>
        /// <returns></returns>
        public string NewINC(string tableName)
        {
            string sql = $@"merge into INC_TAB t1
                                        using (select '{tableName}' TBLNAME from dual) t2
                                        on (t1.TBLNAME = t2.TBLNAME)
                                        when matched then
                                          update set t1.value = t1.value + 1
                                        when not matched then
                                          insert values (t2.TBLNAME, '1')";
            DbHelper.ExecuteNonQuery(sql);
            string selsql = $"select value from INC_TAB where TBLNAME='{tableName}'";
            return DbHelper.ExecuteTable(selsql).Rows[0][0].ToString();
        }

        /// <summary>
        /// 获取对象的验证类
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityVerify<TEntity> GetVerify<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            EntityVerify<TEntity> v = new EntityVerify<TEntity>(entity);
            v.SetDb(DbHelper);
            return v;
        }
        #endregion
    }
    #endregion
    #region 透明的实现

    //public class ServiceBase : RealProxyIOC
    //{
    //    public ServiceBase()
    //    {
    //        List<Type> mrs = new List<Type>();
    //        mrs.Add(typeof(TestServiceOverride));
    //        OverridePropertyType = mrs.ToArray();
    //    }

    //    protected DbHelperBase DbHelper
    //    {
    //        get
    //        {
    //            return _db;
    //        }
    //    }

    //    static readonly DbHelperBase _db = new OracleDbHelper(ConfigExtension.GetConfig("connection"));

    //    #region Service  
    //    public TestService TestService
    //    {
    //        [PropertyBuilder]
    //        get;
    //    }
    //    #endregion
    //}
    #endregion
}
