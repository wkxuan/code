using System;
using System.Collections.Generic;
using System.Linq;

namespace z.Extensions
{
    public static class ArrayExtension
    {

        /// <summary>
        /// 针对更多类型的遍历方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="act"></param>
        public static void ForEach<T>(this IEnumerable<T> arr, Action<T> act)
        {
            arr.ForEach2((a, i) => act(a));
        }

        /// <summary>
        /// 针对字典集的遍历方法
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="Tvalue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="act"></param>
        public static void ForEach<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dic, Action<Tkey, Tvalue> act)
        {
            foreach (KeyValuePair<Tkey, Tvalue> k in dic)
            {
                act?.Invoke(k.Key, k.Value);
            }
        }

        /// <summary>
        /// 针对更多类型的遍历方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="act"></param>
        public static void ForEach2<T>(this IEnumerable<T> arr, Action<T, int> act)
        {
            if (!arr.IsEmpty())
            {
                IEnumerator<T> enumerator = arr.GetEnumerator();
                int i = 0;
                while (enumerator.MoveNext())
                {
                    act?.Invoke(enumerator.Current, i);
                    i++;
                }
            }
        }

        /// <summary>
        /// 针对更多类型的遍历方法,返回false跳出循环
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="act"></param>
        /// <returns>有任何一个返回false,则返回false</returns>
        public static bool ForEachWithBreak<T>(this IEnumerable<T> arr, Func<T, bool> act)
        {
            if (!arr.IsEmpty() && act != null)
            {
                IEnumerator<T> enumerator = arr.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (!act.Invoke(enumerator.Current))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 针对更多类型的遍历方法,返回false跳出循环
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="act"></param>
        public static void ForEachWithBreak<T>(this IEnumerable<T> arr, Func<T, int, bool> act)
        {
            if (!arr.IsEmpty() && act != null)
            {
                int i = 0;
                IEnumerator<T> enumerator = arr.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (!act.Invoke(enumerator.Current, i))
                    {
                        return;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// 修饰数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="Distinct">去重</param>
        /// <param name="ClearEmpty">去掉空值</param>
        /// <returns></returns>
        public static IEnumerable<T> Fixed<T>(this IEnumerable<T> list, bool Distinct = true, bool ClearEmpty = true)
        {
            List<T> res = new List<T>();
            list.ForEach(a =>
            {
                if (ClearEmpty && (a == null || string.IsNullOrWhiteSpace(a.ToString())))
                {
                    return;
                }
                res.Add(a);
            });
            if (Distinct)
            {
                res = res?.Distinct().ToList();
            }
            return res;
        }

        /// <summary>
        /// 数组为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        /// <summary>
        /// 数组中存在某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has<T>(this IEnumerable<T> list, T value)
        {
            if (list == null || value == null)
                return false;
            return list.Contains(value);
        }

        /// <summary>
        /// 超级合并字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static string SuperJoin<T>(this IEnumerable<T> list, string separator, Func<T, string> fun = null)
        {
            return string.Join(separator, list.Select(a => fun == null ? a.ToString() : fun(a)));
        }

        /// <summary>
        /// 获取集合中最小的值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static T Min2<T>(this IEnumerable<T> list, Func<T, double> selector)
        {
            if (list.IsEmpty())
                return default(T);
            double key = double.MaxValue;
            T outT = default(T);
            list.ForEach(l =>
            {
                double k = selector(l);
                if (k < key)
                {
                    key = k;
                    outT = l;
                }
            });
            return outT;
        }

        /// <summary>
        /// 获取集合中最大的值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static T Max2<T>(this IEnumerable<T> list, Func<T, double> selector)
        {
            if (list.IsEmpty())
                return default(T);
            double key = double.MaxValue;
            T outT = default(T);
            list.ForEach(l =>
            {
                double k = selector(l);
                if (k > key)
                {
                    key = k;
                    outT = l;
                }
            });
            return outT;
        }

        /// <summary>
        /// 确认元素在数组中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> list, Func<T, bool> selector)
        {
            if (list.IsEmpty())
                return false;
            return list.Any(selector);
        }

        /// <summary>
        /// 安全的链接两个序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static IEnumerable<T> ConcatSafe<T>(this IEnumerable<T> list, IEnumerable<T> add)
        {
            if (add.IsEmpty())
                return list;
            if (list == null)
                return null;
            return list.Concat(add);
        }
    }
}
