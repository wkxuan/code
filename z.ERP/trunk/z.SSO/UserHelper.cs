using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using z.Context;
using z.Exceptions;
using z.SSO.Model;

namespace z.SSO
{
    public abstract class UserHelper
    {
        protected const string LoginKey = "z.SSO.loginKey.1";
        protected const string LoginSalt = "z.SSO.LoginSalt.1";
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public abstract void Login(string username, string password);

        /// <summary>
        /// 登出
        /// </summary>
        public abstract void LogOut();

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T GetUser<T>() where T : User;

        /// <summary>
        /// 是否已经登陆
        /// </summary>
        /// <returns></returns>
        public abstract bool HasLogin
        {
            get;
        }
        
    }
}
