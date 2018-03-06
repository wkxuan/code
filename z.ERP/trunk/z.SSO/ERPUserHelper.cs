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
        public override T GetUser<T>()
        {
            string key = ApplicationContextBase.GetContext()?.principal?.Identity.Name;
            T e = ApplicationContextBase.GetContext().GetData<T>(LoginKey + key);
            if (!key.IsEmpty() && e == null && ApplicationContextBase.GetContext().principal != null)
            {
                USERSEntity user = _db.Select(new USERSEntity(key));
                if (user == null)
                    throw new NoLoginException();
                return new Employee()
                {
                    Id = user.USERID.ToString(),
                    Name = user.USERNAME,
                    PlatformId = -1
                } as T;
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

        public override void Login(string username, string password)
        {
            List<USERSEntity> users = _db.SelectList(new USERSEntity()
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
                USERSEntity user = users.First();
                if (Verify(user, password))
                {
                    Employee e = new Employee()
                    {
                        Id = user.USERID,
                        Name = user.USERNAME,
                        PlatformId = -1
                    };
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

        bool Verify(USERSEntity user, string password)
        {
            Func<USERSEntity, string, string> salt = (u, p) =>
              {
                  return (u.USERID + LoginSalt + p).ToMD5();
              };
            return user.PASSWORD == salt(user, password);
        }

        public override void LogOut()
        {
            {
                ApplicationContextBase.GetContext().RemoveData(LoginKey + GetUser<User>().Id);
                FormsAuthentication.SignOut();
            }
        }
    }
}