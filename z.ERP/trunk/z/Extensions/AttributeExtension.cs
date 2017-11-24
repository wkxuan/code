using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace z.Extensions
{
    public static class AttributeExtension
    {

        /// <summary>
        /// 取一个属性的一个自定义特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this PropertyInfo obj) where T : Attribute
        {
            return obj.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
        }

        /// <summary>
        /// 取一个类的一个自定义特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this object obj) where T : Attribute
        {
            return obj.GetAttributes<T>()?.FirstOrDefault();
        }

        /// <summary>
        /// 取一个类的一个自定义特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> GetAttributes<T>(this object obj) where T : Attribute
        {
            if (obj is MethodBase)
            {
                return (obj as MethodBase).GetCustomAttributes(typeof(T), true)?.Select(a => a as T).ToList();
            }
            else
            {
                return obj.GetType().GetCustomAttributes(typeof(T), true)?.Select(a => a as T).ToList();
            }
        }
    }
}
