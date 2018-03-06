using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.SSO;

namespace z.ERP.Portal.Areas.Home.Login
{
    public class LoginController : Controller
    {
        // GET: Home/Login
        public ActionResult Login()
        {
            UserApplication.LogOut();
            return View();
        }

        public void Start(string LoginName, string PassWord)
        {
            UserApplication.Login(LoginName, PassWord);
        }
    }
}