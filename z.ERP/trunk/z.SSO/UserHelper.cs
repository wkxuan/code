﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using z.Context;
using z.Exceptions;
using z.Extensions;
using z.LogFactory;
using z.SSO.Model;

namespace z.SSO
{
    public abstract class UserHelper
    {
        public UserHelper(SSOSettings _settings)
        {
            settings = _settings;
        }
        protected SSOSettings settings;
        protected const string LoginKey = "z.SSO.loginKey.1";
        protected const string LoginSalt = "z.SSO.LoginSalt.1";
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public abstract void Login(string username, string password);
        public abstract int Logins(string username, string password);
        /// <summary>
        /// 登出
        /// </summary>
        public abstract void LogOut();

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T GetUser<T>(bool throwError = true) where T : User;

        /// <summary>
        /// 是否已经登陆
        /// </summary>
        /// <returns></returns>
        public virtual bool HasLogin
        {
            get
            {
                return ApplicationContextBase.GetContext()?.principal?.Identity?.Name.IsNotEmpty() ?? false;
            }
        }

        protected LogWriter log
        {
            get
            {
                return new LogWriter("Login");
            }
        }
    }
}
