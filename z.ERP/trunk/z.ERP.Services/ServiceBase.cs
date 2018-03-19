using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using z.Context;
using z.DbHelper.DbDomain;
using z.DBHelper.Helper;
using z.ERP.Entities;
using z.Extensions;
using z.IOC.RealProxyIOC;
using z.IOC.Simple;
using z.LogFactory;
using z.SSO;
using z.SSO.Model;
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
                return new OracleDbHelper(ConfigExtension.GetConfig("connection"));
            }
        }

        //public void SetDb(DbHelperBase db)
        //{
        //    _db = db;
        //}
        //DbHelperBase _db;

        //public DbHelperBase DbHelper
        //{
        //    get
        //    {
        //        if (_db == null)
        //        {
        //            throw new Exception("初始化异常");
        //        }
        //        return _db;
        //    }
        //}
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

        public XtglService XtglService
        {
            get
            {
                return ioc.Create<XtglService>();
            }
        }

        public ShglService ShglService
        {
            get
            {
                return ioc.Create<ShglService>();
            }
        }
        public WyglService WyglService
        {
            get
            {
                return ioc.Create<WyglService>();
            }
        }
        public DpglService DpglService
        {
            get
            {
                return ioc.Create<DpglService>();
            }
        }
        public HomeService HomeService
        {
            get
            {
                return ioc.Create<HomeService>();
            }
        }
        public HtglService HtglService
        {
            get
            {
                return ioc.Create<HtglService>();
            }
        }
        public SpglService SpglService
        {
            get
            {
                return ioc.Create<SpglService>();
            }
        }
        public JsglService JsglService
        {
            get
            {
                return ioc.Create<JsglService>();
            }
        }
        public UserService UserService
        {
            get
            {
                return ioc.Create<UserService>();
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

        /// <summary>
        /// 记录用户可见的日志
        /// </summary>
        /// <param name="Key">记录类型,一般为涉及的Entity名</param>
        /// <param name="Value">记录的主键,一般为单据id</param>
        /// <param name="Context">内容</param>
        /// <param name="title">标题,选填</param>
        public void Notes(string Key, string Value, string Context, string title = "")
        {
            NOTESEntity note = new NOTESEntity()
            {
                ID = NewINC(nameof(NOTESEntity)),
                CREATED = DateTime.Now.ToLongString(),
                CREATER = employee.Id,
                CREATENAME = employee.Name,
                NOTES = Context,
                TITLE = title,
                TKEY = Key,
                TVALUE = Value
            };
            DbHelper.Insert(note);
        }

        #endregion

        #region 权限

        #endregion

        #region 属性
        /// <summary>
        /// 当前登录对象
        /// </summary>
        protected Employee employee
        {
            get
            {
                return UserApplication.GetUser<Employee>();
            }
        }

        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Controller");
            }
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
