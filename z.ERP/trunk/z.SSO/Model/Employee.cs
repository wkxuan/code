using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensiont;

namespace z.SSO.Model
{
    public class Employee : User
    {
        /// <summary>
        /// 调用方id
        /// 如果是web项目,则为当前登陆人,如果是接口项目,则为接口调用方配置的登陆id
        /// </summary>
        public int PlatformId
        {
            get;
            set;
        }

        /// <summary>
        /// 可用权限列表
        /// </summary>
        public string[] Permissions
        {
            get;
            set;
        }

        public bool HasPermission(string key)
        {
            return !Permissions.IsEmpty() && Permissions.Contains(key);
        }

    }
}
