using System.Collections.Generic;
using System.Web.Mvc;
using z.DBHelper.DBDomain;
using z.ERP.Services;
using z.LogFactory;
using z.MVC5.Results;
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
        public IEnumerable<string> CommonSave(IEnumerable<TableEntityBase> infos)
        {
            return service.CommonService.CommonSave(infos);
        }

        /// <summary>
        /// 快速保存
        /// </summary>
        /// <param name="info"></param>
        public string CommonSave(TableEntityBase info)
        {
            return service.CommonService.CommonSave(info);
        }

        /// <summary>
        /// 快速删除
        /// </summary>
        /// <param name="infos"></param>
        public void CommenDelete(IEnumerable<TableEntityBase> infos)
        {
            service.CommonService.CommenDelete(infos);
        }

        /// <summary>
        /// 快速删除
        /// </summary>
        /// <param name="info"></param>
        public void CommenDelete(TableEntityBase info)
        {
            service.CommonService.CommenDelete(info);
        }

        public T Select<T>(T t) where T : TableEntityBase, new()
        {
            return service.CommonService.Select(t);
        }


        public List<T> SelectList<T>(T t) where T : TableEntityBase, new()
        {
            return service.CommonService.SelectList(t);
        }

        /// <summary>
        /// 获取对象的验证类
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityVerify<TEntity> GetVerify<TEntity>(TEntity entity) where TEntity : TableEntityBase
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
        //验证功能按钮权限
        public UIResult checkMenu(List<MenuAuthority> MenuAuthority)
        {
            var data = new List<MenuAuthority>();
            for (var i = 0; i < MenuAuthority.Count; i++)
            {
                var obj = new MenuAuthority();
                obj.id = MenuAuthority[i].id;
                obj.authority = MenuAuthority[i].authority;
                if (string.IsNullOrEmpty(MenuAuthority[i].authority)|| employee.HasPermission(MenuAuthority[i].authority))
                {
                    obj.enable = true;
                }
                else
                {
                    obj.enable = false;
                }
                data.Add(obj);
            }
            return new UIResult(data);
        }
    }
}