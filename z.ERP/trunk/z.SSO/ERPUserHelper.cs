using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using z.Context;
using z.DBHelper.Helper;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Exceptions;
using z.Extensions;
using z.Extensiont;
using z.SSO.Model;

namespace z.SSO
{
    public class ERPUserHelper : UserHelper
    {
        static readonly DbHelperBase _db = new OracleDbHelper(ConfigExtension.GetConfig("connection"));

        public override bool HasLogin
        {
            get
            {
                return ApplicationContextBase.GetContext()?.principal?.Identity?.Name.IsNotEmpty() ?? false;
            }
        }

        public override T GetUser<T>()
        {
            string key = ApplicationContextBase.GetContext()?.principal?.Identity.Name;
            T e = ApplicationContextBase.GetContext().GetData<T>(LoginKey + key);
            if (!key.IsEmpty() && e == null && ApplicationContextBase.GetContext().principal != null)
            {
                SYSUSEREntity user = _db.Select(new SYSUSEREntity(key));
                if (user == null)
                    throw new NoLoginException();
                return GetUser(user.USERID) as T;
            }
            if (e == null)
            {
                if (ConfigExtension.TestModel)//测试模式
                {
                    var teste = new Employee()
                    {
                        Id = "-1",
                        Name = "测试",
                        PlatformId = -1
                    } as T;
                    return teste;
                }
                throw new NoLoginException();
            }
            return e;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Employee GetUser(string Id)
        {
            SYSUSEREntity user = _db.Select(new SYSUSEREntity(Id));
            return new Employee()
            {
                Id = user.USERID,
                Name = user.USERNAME,
                PlatformId = -1
            };
        }

        public override void Login(string username, string password)
        {
            List<SYSUSEREntity> users = _db.SelectList(new SYSUSEREntity()
            {
                USERCODE = username
            });
            if (users.IsEmpty())
            {
                throw new Exception("找不到用户");
            }
            else if (users.Where(a => a.USER_FLAG.ToInt() == (int)用户标记.正常).IsEmpty())
            {
                throw new Exception($"用户已{users.First().USER_FLAG.ToEnum<用户标记>() }");
            }
            else
            {
                SYSUSEREntity user = users.First();
                if (Verify(user, password))
                {
                    Employee e = GetUser(user.USERID);
                    ApplicationContextBase.GetContext().SetData(LoginKey + e.Id, users);
                    ApplicationContextBase.GetContext().principal = new GenericPrincipal(new GenericIdentity(e.Id), null);
                    FormsAuthentication.SetAuthCookie(e.Id, true);
                }
                else
                {
                    throw new Exception("密码错误");
                }
            }
        }

        bool Verify(SYSUSEREntity user, string password)
        {
            Func<SYSUSEREntity, string, string> salt = (u, p) =>
              {
                  return (u.USERID + LoginSalt + p).ToMD5();
              };
            return user.PASSWORD == salt(user, password);
        }

        public override void LogOut()
        {
            if (HasLogin)
            {
                ApplicationContextBase.GetContext().RemoveData(LoginKey + GetUser<User>().Id);
                FormsAuthentication.SignOut();
            }
        }
    }
}