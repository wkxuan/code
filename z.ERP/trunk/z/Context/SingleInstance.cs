using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.Context
{
    /// <summary>
    /// 安全单例
    /// </summary>
    public static class SingleInstance
    {
        static object lockObj = new object();
        public static T Resolve<T>(this T t, Func<T> func)
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = func();
                    }
                }
            }
            return (T)instance;
        }

        static object instance;

    }
}
