using NewtonsoftCode.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using z.Extensions.Converters;

namespace z.Extensions
{
    public static class ObjectExtension
    {
        #region 序列化

        /// <summary>
        /// 公用线程锁
        /// </summary>
        public static readonly object Locker = new object();

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="simple">简单json,不带换行的</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool simple = false)
        {
            return JsonConvert.SerializeObject(obj, simple ? Formatting.Indented : Formatting.None);
        }

        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToObj<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 试图反序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool TryToObj<T>(this string str, out T t)
        {
            try
            {
                t = ToObj<T>(str);
                return true;
            }
            catch
            {
                t = default(T);
                return false;
            }
        }

        /// <summary>
        /// 试图反序列化json
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool TryToObj(this string str, Type type, out object obj)
        {
            try
            {
                obj = str.ToObj(type);
                return true;
            }
            catch
            {
                obj = null;
                return false;
            }
        }

        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObj(this string str, Type type)
        {
            return JsonConvert.DeserializeObject(str, type);
        }

        /// <summary>
        /// 按原类型,转换为新类型
        /// </summary>
        /// <typeparam name="Ts">原类型</typeparam>
        /// <typeparam name="Tt">新类型</typeparam>
        /// <param name="t"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Tt ToObj<Ts, Tt>(this Ts t, Func<Ts, Tt> func) where Ts : class where Tt : class
        {
            if (t == null)
                return null;
            return func?.Invoke(t);
        }

        /// <summary>
        /// 转化为另外一个类
        /// </summary>
        /// <typeparam name="Ts"></typeparam>
        /// <typeparam name="Tt"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Tt ToOtherObj<Ts, Tt>(this Ts obj) where Ts : class where Tt : class
        {
            return obj.ToJson().ToObj<Tt>();
        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            return obj.ToJson().ToObj<T>();
        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object ObjectDeepClone(this object obj)
        {
            return obj.ToJson().ToObj(obj.GetType());
        }

        /// <summary>
        /// 获取实体类的str形式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToCommonString(this object obj)
        {
            return obj.ToCommonString("\r\n", "=", true, true, true);
        }

        /// <summary>
        /// 使用特定方式输出字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="endstr">分隔符</param>
        /// <param name="midstr">键值对分隔符(仅当显示键和值时有效</param>
        /// <param name="HasNull">输出空值</param>
        /// <param name="hasname">输出键名</param>
        /// <param name="hasvalue">输出值</param>
        /// <returns></returns>
        public static string ToCommonString(this object obj, string endstr, string midstr, bool HasNull, bool hasname, bool hasvalue)
        {
            List<string> strlist = new List<string>();
            PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                string typename = p.Name;
                string value = p.GetValue(obj, null) == null ? "" : p.GetValue(obj, null).ToString();
                if (HasNull || !string.IsNullOrEmpty(value))
                {
                    if (hasname && hasvalue)
                    {
                        strlist.Add(typename + midstr + value);
                    }
                    else if (hasvalue)
                    {
                        strlist.Add(value);
                    }
                    else if (hasname)
                    {
                        strlist.Add(typename);
                    }
                }
            }
            return String.Join(endstr, strlist);
        }

        /// <summary>
        /// 序列化为字典集
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            return obj.ToDictionary<string>();
        }

        /// <summary>
        /// 序列化为字典集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, T> ToDictionary<T>(this object obj) where T : class
        {
            Dictionary<string, T> dic = new Dictionary<string, T>();
            List<string> strlist = new List<string>();
            PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                string typename = p.Name;
                object o = p.GetValue(obj, null);
                T value = o == null ? default(T) : o as T;
                dic.Add(typename, value);
            }
            return dic;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> infos) where T : class
        {
            DataTable dt = new DataTable();
            if (infos == null)
                return null;
            T t = default(T);
            PropertyInfo[] props = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                dt.Columns.Add(p.Name, p.PropertyType);
            }
            infos.ForEach(info =>
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo p in props)
                {
                    string typename = p.Name;
                    object o = p.GetValue(info, null);
                    T value = o == null ? default(T) : o as T;
                    dr[p.Name] = value;
                }
                dt.Rows.Add(dr);
            });
            return dt;
        }
        #endregion
        #region 继承
        /// <summary>
        /// 继承于此类
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="T2"></param>
        /// <returns></returns>
        public static bool BaseOn(this Type T1, Type T2)
        {
            if (T1 == T2)
            {
                return true;
            }
            else
            {
                if (T1.BaseType == null)
                {
                    return false;
                }
                else
                    return T1.BaseType.BaseOn(T2);
            }
        }

        /// <summary>
        /// 继承于此类
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="T1"></param>
        /// <returns></returns>
        public static bool BaseOn<T2>(this Type T1)
        {
            return T1.BaseOn(typeof(T2));
        }
        #endregion
        #region 反射
        /// <summary>  
        /// 根据属性名获取属性值  
        /// </summary>  
        /// <typeparam name="T">对象类型</typeparam>  
        /// <param name="t">对象</param>  
        /// <param name="name">属性名</param>  
        /// <returns>属性的值</returns>  
        public static object GetPropertyValue<T>(this T t, string name)
        {
            Type type = t.GetType();
            PropertyInfo p = type.GetProperty(name);
            if (p == null)
            {
                throw new Exception($"类型{t.GetType().Name}没有名为{name}的属性");
            }
            var param_obj = Expression.Parameter(typeof(T));
            var param_val = Expression.Parameter(typeof(object));

            //转成真实类型，防止Dynamic类型转换成object  
            var body_obj = Expression.Convert(param_obj, type);

            var body = Expression.Property(body_obj, p);
            var getValue = Expression.Lambda<Func<T, object>>(body, param_obj).Compile();
            return getValue(t);
        }

        /// <summary>  
        /// 根据属性名称设置属性的值  
        /// </summary>  
        /// <typeparam name="T">对象类型</typeparam>  
        /// <param name="t">对象</param>  
        /// <param name="name">属性名</param>  
        /// <param name="value">属性的值</param>  
        public static void SetPropertyValue<T>(this T t, string name, object value)
        {
            Type type = t.GetType();
            PropertyInfo p = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p == null)
            {
                throw new Exception($"类型{t.GetType().Name}没有名为{name}的属性");
            }
            if (p.PropertyType.IsEnum)
            {
                if (value == DBNull.Value)
                    p.SetValue(t, default(T), null);
                else
                    p.SetValue(t, value.ToString().ToInt(), null);
            }
            else
            {
                p.SetValue(t, value.ChangeType(p.PropertyType), null);
            }
            return;
            //下面的报错
            var param_obj = Expression.Parameter(type);
            var param_val = Expression.Parameter(typeof(object));
            var body_obj = Expression.Convert(param_obj, type);
            var body_val = Expression.Convert(param_val, p.PropertyType);

            //获取设置属性的值的方法  
            var setMethod = p.GetSetMethod(true);

            //如果只是只读,则setMethod==null  
            if (setMethod != null)
            {
                var body = Expression.Call(param_obj, p.GetSetMethod(), body_val);
                var setValue = Expression.Lambda<Action<T, object>>(body, param_obj, param_val).Compile();
                setValue(t, value);
            }
        }

        /// <summary>
        /// 给数组类型赋值
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public static void SetArrValue(this PropertyInfo prop, object info, IEnumerable<object> data)
        {
            if (prop.PropertyType.IsArray)
            {
                object arr = new object();
                arr = prop.PropertyType.InvokeMember("Set", BindingFlags.CreateInstance, null, arr, new object[] { data.Count() });
                data.ForEach2((obj, inx) =>
                {
                    prop.PropertyType.GetMethod("SetValue", new Type[2] { typeof(object), typeof(int) }).Invoke(arr, new object[] { obj, inx });
                });
                prop.SetValue(info, arr, null);
            }
            else if (prop.PropertyType.IsGenericType)
            {
                object arr = new object();
                arr = prop.PropertyType.InvokeMember("Set", BindingFlags.CreateInstance, null, arr, new object[] { data.Count() });
                data.ForEach2((obj, inx) =>
                {
                    prop.PropertyType.GetMethod("Add", new Type[] { prop.GetChildren() }).Invoke(arr, new object[] { obj });
                });
                prop.SetValue(info, arr, null);
            }
            else
            {
                throw new Exception("对象不是数组类型");
            }
        }

        /// <summary>
        /// 属性是一个数组类型
        /// </summary>
        /// <param name="pinfo"></param>
        /// <returns></returns>
        public static bool IsArray(this PropertyInfo pinfo)
        {
            return pinfo.PropertyType.IsGenericType || pinfo.PropertyType.IsArray;
        }

        /// <summary>
        /// 检测一个类是空的
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullValue(this object obj)
        {
            return obj == null || obj == DBNull.Value;
        }

        /// <summary>
        /// 获取一个类的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// 获取数组类型属性的子类型
        /// </summary>
        /// <param name="pinfo"></param>
        /// <returns></returns>
        public static Type GetChildren(this PropertyInfo pinfo)
        {
            if (pinfo.IsArray())
            {
                if (pinfo.PropertyType.IsArray)
                {
                    return pinfo.PropertyType.GetElementType();
                }
                if (pinfo.PropertyType.IsGenericType)
                {
                    return pinfo.PropertyType.GetGenericArguments().FirstOrDefault();
                }
            }
            return null;
        }

        /// <summary>
        /// 遍历类型为数组的一个属性
        /// </summary>
        /// <typeparam name="Tp">父类型</typeparam>
        /// <typeparam name="Tc">子类型</typeparam>
        /// <param name="pinfo">父属性</param>
        /// <param name="info">父对象</param>
        /// <param name="act">子对象</param>
        public static void ForEach<Tp, Tc>(this PropertyInfo pinfo, Tp info, Action<Tc> act)
        {
            if (pinfo.IsArray())
            {
                if (pinfo.PropertyType.IsArray)
                {
                    Tc[] items = pinfo.GetValue(info, null) as Tc[];
                    items.ForEach(act);
                }
                else if (pinfo.PropertyType.IsGenericType)
                {
                    IEnumerable<Tc> items = pinfo.GetValue(info, null) as IEnumerable<Tc>;
                    items.ForEach(act);
                }
            }
        }

        /// <summary>
        /// 获取去掉可空类型（Nullable）的类型.
        /// </summary>
        /// <param name="type">当前类型的实例对象.</param>
        /// <returns>去掉可空类型（Nullable）的类型.</returns>
        public static Type ToNotNullable(this Type type)
        {
            if (IsNullable(type))
                return Nullable.GetUnderlyingType(type);
            return type;
        }

        /// <summary>
        /// 获取可空类型（Nullable）的类型.
        /// </summary>
        /// <param name="type">当前类型的实例对象.</param>
        /// <returns>可空类型（Nullable）的类型.</returns>
        public static Type ToNullable(this Type type)
        {
            if (!IsNullable(type) && type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            return type;
        }

        /// <summary>
        /// 判断是可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 改变类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(this object obj, Type type)
        {
            if (obj.IsNullValue())
                return type.GetDefaultValue();
            return Convert.ChangeType(obj, type);
        }

        /// <summary>
        /// 改变类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ChangeType<T>(this object obj)
        {
            return (T)obj.ChangeType(typeof(T));
        }

        #endregion
        #region 程序集
        /// <summary>
        /// 获取程序集下所有的符合条件的类
        /// </summary>
        /// <param name="ass"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<Type> FindAllType(this Assembly ass, Func<Type, bool> func)
        {
            return ass.GetTypes().Where(func);
        }
        #endregion
    }
}
