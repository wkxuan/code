using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace z.CacheBox
{
    public class WebCache : ICache
    {
        public T Get<T>(string key)
        {
            Cache c = new Cache();
            object obj = c.Get(key);
            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        public void Set<T>(string key, T data)
        {
            Cache c = new Cache();
            c.Insert(key, data);
        }

        public T Simple<T>(string key, Func<T> IsNull)
        {
            T t = Get<T>(key);
            if (t == null)
            {
                t = IsNull();
                Set(key, t);
            }
            return t;
        }
    }
}
