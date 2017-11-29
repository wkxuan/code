using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using z.Exceptions;
using z.Extensions;

namespace z.Context
{
    public class LoginHelper
    {
        static string SessionKey = "asdafsdsdfgsdfsdf2";

        public static Employee GetLogin()
        {
            Employee e = HttpContext.Current.Session[SessionKey] as Employee;
            if (e == null)
            {
                if (ConfigExtension.TestModel)//测试模式
                {
                    return new Employee("0", "super");
                }
                throw new NoLoginException();
            }
            return e;
        }

        public static void Login(Employee e)
        {
            HttpContext.Current.Session.Remove(SessionKey);
            HttpContext.Current.Session.Add(SessionKey, e);
        }

        //登出
        public static void LogOut()
        {
            HttpContext.Current.Session.Remove(SessionKey);
        }

        public static bool HasLogin()
        {
            return HttpContext.Current.Session[SessionKey] != null;
        }

    }
}
