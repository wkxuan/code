﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Security;
using z.CacheBox;
using z.Context;
using z.Exceptions;
using z.Extensions;
using z.Extensions;
using z.ServiceHelper;
using z.SSO.Model;

namespace z.SSO
{
    public class ERPUserHelper : UserHelper
    {
        public ERPUserHelper(SSOSettings _settings) : base(_settings)
        {
        }

        public UserService.ISSOService service
        {
            get
            {
                return ApplicationContextBase.GetContext().GetData("SSOServiceUrl", () =>
                {
                    return WCF.CreateWCFServiceByURL<UserService.ISSOService>(settings.WcfUrl);
                });
            }
        }

        public override T GetUser<T>(bool throwError = true)
        {
            if (ConfigExtension.TestModel)//测试模式
            {
                Employee teste = new Employee()
                {
                    Id = ConfigExtension.TestModel_User,
                    Name = $"测试模式:{ConfigExtension.TestModel_User}",
                    PlatformId = ""
                };
                teste.PermissionHandle = HasPermission;
                return teste as T;
            }
            string key = ApplicationContextBase.GetContext()?.principal?.Identity.Name;
            T e = ApplicationContextBase.GetContext().GetData<T>(LoginKey + key);
            if (!key.IsEmpty() && e == null && ApplicationContextBase.GetContext().principal != null)
            {
                User user = service.GetUserById(key)?.ToObj(a => new User() { Id = a.Id, Name = a.Name });
                if (user == null)
                    throw new NoLoginException();
                Employee emp = new Employee()
                {
                    Id = user.Id,
                    Name = user.Name,
                    PlatformId = ""
                };
                emp.PermissionHandle = HasPermission;
                return emp as T;
            }
            if (e == null && throwError)
            {
                throw new NoLoginException();
            }
            return e;
        }


        public override void Login(string username, string password)
        {
            Model.User user = service.GetUserByCode(username, password).ToObj(a => new User() { Id = a.Id, Name = a.Name });
            ApplicationContextBase.GetContext().SetData(LoginKey + user.Id, user);
            ApplicationContextBase.GetContext().principal = new GenericPrincipal(new GenericIdentity(user.Id), null);
            FormsAuthentication.SetAuthCookie(user.Id, true);
        }


        public override void LogOut()
        {
            if (HasLogin)
            {
                ApplicationContextBase.GetContext().RemoveData(LoginKey + GetUser<User>().Id);
                FormsAuthentication.SignOut();
            }
        }
        public override int Logins(string username, string password) {
            Model.User user = service.GetUserByCode(username, password).ToObj(a => new User() { Id = a.Id, Name = a.Name });
            ApplicationContextBase.GetContext().SetData(LoginKey + user.Id, user);
            ApplicationContextBase.GetContext().principal = new GenericPrincipal(new GenericIdentity(user.Id), null);
            FormsAuthentication.SetAuthCookie(user.Id, true);
            return 0;
        }
        bool HasPermission(string UserId, string Key, PermissionType Type = PermissionType.Menu)
        {
            log.Info("ERPHasPermission", UserId, Key, Type);
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
                            () => service.GetPermissionByUserId(UserId))
                            .Contains(Key);
                    }
                default:
                    {
                        throw new Exception("未知的权限类型");
                    }
            }
        }
    }
}



