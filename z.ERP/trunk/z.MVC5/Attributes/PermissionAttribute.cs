using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.MVC5.Attributes
{
    [AttributeUsage(AttributeTargets.Method,
       Inherited = false, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        /// <summary>
        /// 权限码
        /// </summary>
        /// <param name="_key"></param>
        public PermissionAttribute(string _key)
        {
            Key = _key;
        }

        public string Key
        {
            get;
            set;
        }
    }
}
