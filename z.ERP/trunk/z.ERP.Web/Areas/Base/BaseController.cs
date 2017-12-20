using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.DbHelper.DbDomain;
using z.ERP.Services;
using z.Verify;

namespace z.ERP.Web.Areas.Base
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            service = new ServiceBase();
        }

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

        public ServiceBase service
        {
            get;
            set;
        }
    }
}