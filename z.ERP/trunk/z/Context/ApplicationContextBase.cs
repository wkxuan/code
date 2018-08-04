using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace z.Context
{
    /// <summary>
    /// 为应用程序的公共上下文提供基类
    /// </summary>
    public abstract class ApplicationContextBase
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract T GetData<T>(string name) where T : class;

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="Data">如果没有数据,则设置数据</param>
        /// <returns></returns>
        public virtual T GetData<T>(string name, Func<T> Data) where T : class
        {
            var t = GetData<T>(name);
            if (t == null)
            {
                t = Data();
                SetData(name, t);
            }
            return t;
        }

        /// <summary>
        /// 放置数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public abstract void SetData<T>(string name, T data) where T : class;

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="name"></param>
        public abstract void RemoveData(string name);

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public abstract IPrincipal principal
        {
            get;
            set;
        }

        [ThreadStatic]
        static ApplicationContextBase CurrentContext;
        /// <summary>
        /// 获取合适的应用程序上下文
        /// </summary>
        /// <returns></returns>
        public static ApplicationContextBase GetContext()
        {
            if (CurrentContext == null)
            {
                if (OperationContext.Current != null)
                {
                    CurrentContext = new WcfApplicationContext();
                }
                else if (HttpContext.Current != null)
                {
                    CurrentContext = new HttpApplicationContext();
                }
                else
                {
                    CurrentContext = new ThreadApplicationContext();
                }
            }
            return CurrentContext;
        }
    }
}
