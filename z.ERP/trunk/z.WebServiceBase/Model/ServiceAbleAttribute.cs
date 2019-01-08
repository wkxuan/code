using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.WebServiceBase.Model
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ServiceAbleAttribute : Attribute
    {
        public ServiceAbleAttribute(string key, string menuid = null)
        {
            Key = key;
            MenuId = menuid;
        }

        public string Key
        {
            get;
            set;
        }

        public string MenuId
        {
            get;
            set;
        }
    }
}