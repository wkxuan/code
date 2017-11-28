using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace z.Extensiont
{
    public static class ArrayExtensiont
    {

        /// <summary>
        /// 针对更多类型的遍历方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="act"></param>
        public static void ForEach<T>(this IEnumerable<T> arr, Action<int, T> act)
        {
            if (!arr.IsEmpty())
                for (int i = 0; i < arr.Count(); i++)
                {
                    act(i, arr.ElementAt(i));
                }
        }

        /// <summary>
        /// 针对更多类型的遍历方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="act"></param>
        public static void ForEach<T>(this IEnumerable<T> arr, Action<T> act)
        {
            arr.ForEach((i, a) => act(a));
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
            return list == null || list.Count() == 0;
        }

    }
}
