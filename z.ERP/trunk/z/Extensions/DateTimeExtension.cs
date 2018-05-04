using NewtonsoftCode.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace z.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 转化为短字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToShortString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 转化为长字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToLongString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
