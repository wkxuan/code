using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.IOC.Simple
{
    public class SimpleIOC
    {
        public SimpleIOC(List<Type> mrs)
        {
            _mrs = mrs;
        }
        List<Type> _mrs;
        public T Create<T>()
        {
            return (T)Activator.CreateInstance(
                                 _mrs.FirstOrDefault(a => a.BaseOn<T>()) ?? typeof(T),
                                 BindingFlags.Instance | BindingFlags.NonPublic,
                                 null,
                                 new object[] { },
                                 null);
        }
        public object Create(Type t)
        {
            return Activator.CreateInstance(
                                 _mrs.FirstOrDefault(a => a.BaseOn(t)) ?? t,
                                 BindingFlags.Instance | BindingFlags.NonPublic,
                                 null,
                                 new object[] { },
                                 null);
        }
    }
}
