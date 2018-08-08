using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Security;
using z.CacheBox;
using z.Context;
using z.DBHelper.Helper;
using z.Encryption;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Exceptions;
using z.Extensions;
using z.Extensiont;
using z.SSO.Model;

namespace z.SSO
{
    public class PortalUserHelper : UserHelper
    {
        static readonly DbHelperBase _db = new OracleDbHelper(ConfigExtension.GetConfig("connection"));

        public PortalUserHelper(SSOSettings _settings) : base(_settings)
        {
        }

        public override T GetUser<T>(bool throwError = true)
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
                        Id = ConfigExtension.TestModel_User,
                        Name = $"测试模式:{ConfigExtension.TestModel_User}",
                        PlatformId = ""
                    } as T;
                    return teste;
                }
                if (throwError)
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
            Employee e = new Employee()
            {
                Id = user.USERID,
                Name = user.USERNAME,
                PlatformId = ""
            };
            e.PermissionHandle = HasPermission;
            return e;
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
            return user.PASSWORD == salt(user, password);
        }

        public string salt(SYSUSEREntity user, string psw)
        {
            return MD5Encryption.Encrypt(user.USERID + LoginSalt + psw);
        }

        public override void LogOut()
        {
            if (HasLogin)
            {
                ApplicationContextBase.GetContext().RemoveData(LoginKey + GetUser<User>().Id);
                FormsAuthentication.SignOut();
            }
        }

        bool HasPermission(string UserId, string Key, PermissionType Type = PermissionType.Menu)
        {
            log.Info("ProtalHasPermission", UserId, Key, Type);
            if (UserId.IsEmpty())
                return false;
            if (Key.IsEmpty())
                return true;
            if (UserId.ToInt() < 0)
                return true;
            switch (Type)
            {
                case PermissionType.Menu:
                    {
                        ICache wc = new WebCache();
                        return wc.Simple($"Permission_{Type.ToString()}_{UserId}",
                      () => _db.ExecuteTable($@"SELECT A.MENUID FROM USERMODULE A,MENU B where A.MENUID = B.ID
                                                and exists(select 1 from USER_ROLE A1, ROLE_MENU B1,USERMODULE C1 where A1.USERID= '{UserId}'
                                                and A1.ROLEID = B1.ROLEID and B1.MENUID=C1.MENUID and C1.MENUID = A.MENUID )")
                                .ToList<string>().ToArray())
                                .Contains(Type.ToString() + Key);
                    }
                default:
                    {
                        throw new Exception("未知的权限类型");
                    }
            }
        }
    }
}



