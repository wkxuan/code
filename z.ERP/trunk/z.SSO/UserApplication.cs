﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using z;
using z.Context;
using z.Exceptions;
using z.Extensions;
using z.SSO.Model;

namespace z.SSO
{
    public static class UserApplication
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user"></param>
        public static void Login(string username, string password)
        {
            _userHelper.Login(username, password);
        }

        public static int Logins(string username, string password)
        {
            return _userHelper.Logins(username, password);
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void LogOut()
        {
            _userHelper.LogOut();
        }

        public static T GetUser<T>(bool throwError = true) where T : User
        {
            return _userHelper.GetUser<T>(throwError);
        }

        public static bool HasLogin
        {
            get
            {
                return _userHelper.HasLogin;
            }
        }

        public static SSOSettings settings
        {
            get
            {
                SSOSettings set= ConfigExtension.GetSection<SSOSettings>(SSOSettings.Name);
                if (set == null)
                    throw new Exception("没有找到SSOSettings配置");
                return set;
            }
        }

        static UserHelper _userHelper
        {
            get
            {
                switch (settings.Type.ToUpper())
                {
                    case "ERP":
                        return new ERPUserHelper(settings);
                    case "PORTAL":
                        return new PortalUserHelper(settings);
                    case "SERVICE":
                        return new ServiceUserHelper(settings);
                    case "THREAD":
                        return new ThreadUserHelper(settings);
                    default:
                        throw new Exception($"配置节点{SSOSettings.Name}的类型{settings.Type}未知");
                }
            }
        }
    }
}
