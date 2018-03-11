using System;
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

        /// <summary>
        /// 登出
        /// </summary>
        public static void LogOut()
        {
            _userHelper.LogOut();
        }

        public static T GetUser<T>() where T : User
        {
            return _userHelper.GetUser<T>();
        }

        static UserHelper _userHelper
        {
            get
            {
                return new ERPUserHelper();
            }
        }
    }
}
