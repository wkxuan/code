using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.Context;
using z.DbHelper.DbDomain;
using z.DBHelper.Helper;
using z.ERP.Services;
using z.Extensions;
using z.LogFactory;
using z.SSO;
using z.SSO.Model;
using z.Verify;

namespace z.ERP.Web.Areas.Base
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            //_db = new OracleDbHelper(ConfigExtension.GetConfig("connection"));
            service = new ServiceBase();
            //service.SetDb(_db);
        }

        //DbHelperBase _db;

        /// <summary>
        /// 快速保存
        /// </summary>
        /// <param name="infos"></param>
        public IEnumerable<string> CommonSave(IEnumerable<EntityBase> infos)
        {
            return service.CommonService.CommonSave(infos);
        }

        /// <summary>
        /// 快速保存
        /// </summary>
        /// <param name="info"></param>
        public string CommonSave(EntityBase info)
        {
            return service.CommonService.CommonSave(info);
        }

        /// <summary>
        /// 快速删除
        /// </summary>
        /// <param name="infos"></param>
        public void CommenDelete(IEnumerable<EntityBase> infos)
        {
            service.CommonService.CommenDelete(infos);
        }

        /// <summary>
        /// 快速删除
        /// </summary>
        /// <param name="info"></param>
        public void CommenDelete(EntityBase info)
        {
            service.CommonService.CommenDelete(info);
        }

        public T Select<T>(T t) where T : EntityBase
        {
            return service.CommonService.Select(t);
        }


        public List<T> SelectList<T>(T t) where T : EntityBase, new()
        {
            return service.CommonService.SelectList(t);
        }

        /// <summary>
        /// 获取对象的验证类
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityVerify<TEntity> GetVerify<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            return service.GetVerify(entity);
        }
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
        protected ServiceBase service
        {
            get;
            set;
        }

        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Controller");
            }
        }
    }
}