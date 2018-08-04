using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;
using z.Extensiont;

namespace z.SSO.Model
{
    public class Employee : User
    {
        /// <summary>
        /// 调用方id
        /// 如果是web项目,则为当前登陆人,如果是接口项目,则为接口调用方配置的登陆id
        /// </summary>
        public string PlatformId
        {
            get;
            set;
        }

        internal Func<string, string, PermissionType, bool> PermissionHandle
        {
            get;
            set;
        }

        public bool HasPermission(string key, PermissionType type = PermissionType.Menu)
        {
            return PermissionHandle == null ? false : PermissionHandle(Id, key, type);
        }



    }
}
