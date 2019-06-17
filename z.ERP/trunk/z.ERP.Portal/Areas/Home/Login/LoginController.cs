using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.MVC5.Attributes;
using z.SSO;

namespace z.ERP.Portal.Areas.Home.Login
{
    public class LoginController : Controller
    {
        [IgnoreLogin]
        public ActionResult Login()
        {
            UserApplication.LogOut();
            return View();
        }

        [IgnoreLogin]
        public void Start(string LoginName, string PassWord)
        {
            UserApplication.Login(LoginName, PassWord);
        }
        /// <summary>
        /// 0.登陆成功 1.账号错误 2.密码错误 。3.停用    by:dzk 20190614
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        [IgnoreLogin]       
        public int Starts(string LoginName, string PassWord)
        {            
            return UserApplication.Logins(LoginName, PassWord);
        }

    }
}