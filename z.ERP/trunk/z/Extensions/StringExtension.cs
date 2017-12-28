using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace z.Extensions
{
    public static class StringExtension
    {
        #region 操作方法
        /// <summary>
        /// 是空的,含null,empty,纯空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str?.Trim());
        }
        /// <summary>
        /// 不是空的,含null,empty,纯空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string str)
        {
            return !str.IsEmpty();
        }

        /// <summary>
        /// 是数字,含小数
        /// </summary>
        /// <returns></returns>
        public static bool IsNumber(this string str, out double d)
        {
            return double.TryParse(str, out d);
        }

        /// <summary>
        /// 是数字,含小数
        /// </summary>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            double d;
            return str.IsNumber(out d);
        }

        /// <summary>
        /// 是数字,不含小数,如果是小数,返回false
        /// </summary>
        /// <returns></returns>
        public static bool IsInt(this string str, out int d)
        {
            bool b = int.TryParse(str, out d);
            double db;
            str.IsNumber(out db);
            return b && db == d;
        }

        /// <summary>
        /// 是数字,不含小数,如果是小数,返回false
        /// </summary>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            int d;
            return str.IsInt(out d);
        }
        #endregion
        #region 转换方法

        /// <summary>
        /// 字符串转数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            int i = 0;
            int.TryParse(str, out i);
            return i;
        }

        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            decimal i = 0;
            decimal.TryParse(str, out i);
            return i;
        }
        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double i = 0;
            double.TryParse(str, out i);
            return i;
        }

        /// <summary>
        /// 根据字符型的枚举值取枚举
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Tenum ToEnum<Tenum>(this string str) where Tenum : struct
        {
            Tenum res;
            Enum.TryParse(str, out res);
            return res;
        }


        /// <summary>
        /// 转化为日期
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ThrowError">如果转换失败，则抛出错误</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, bool ThrowError = false)
        {
            str = str.Replace('/', '-');
            DateTime dt;
            Func<string, bool> GMT2Local = (gmt) =>
            {
                try
                {
                    string pattern = "";
                    if (gmt.IndexOf("+0") != -1)
                    {
                        gmt = gmt.Replace("GMT", "");
                        pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
                    }
                    if (gmt.ToUpper().IndexOf("GMT") != -1)
                    {
                        pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
                    }
                    if (pattern != "")
                    {
                        dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                        dt = dt.ToLocalTime();
                    }
                    else
                    {
                        dt = Convert.ToDateTime(gmt);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            };
            if (DateTime.TryParse(str, out dt))
            {
                return dt;
            }
            else if (str.TryToObj(out dt))
            {
                return dt;
            }
            else if (GMT2Local(str))
            {
                return dt;
            }
            else
            {
                if (ThrowError)
                {
                    throw new Exception("无法将字符串" + str + "转化为日期");
                }
                return System.DateTime.MinValue;
            }
        }
        #endregion
        #region 字符串处理
        /// <summary>
        /// 补齐字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="c">补足位</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string Fill(this string str, char c, int length)
        {
            if (str.Length >= length)
            {
                return str.Substring(0, length);
            }
            else
            {
                return Fill(str + c, c, length);
            }
        }

        /// <summary>
        /// 一个安全的截取字符串方法,不会报错,截不到返回empty
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubstringSafe(this string str, int startIndex, int length = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (str.Length < startIndex + 1)
                return "";
            if (str.Length < length + startIndex)
                return str.Substring(startIndex);
            if (length != 0)
                return str.Substring(startIndex, length);
            else
                return str.Substring(startIndex);
        }

        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(this string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15)
                return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length">总长度</param>
        /// <param name="useNum">有数字</param>
        /// <param name="useLow">有小写字母</param>
        /// <param name="useUpp">有大写字母</param>
        /// <param name="useSpe">有奇葩特殊符号</param>
        /// <param name="custom">总是有如下字符(不连续)</param>
        /// <param name="nature">有人性(会去掉一些比较难辨认的东西)</param>
        /// <returns></returns>
        public static string Random(int length, bool useNum = true, bool useLow = false, bool useUpp = false, bool useSpe = false, string custom = "", bool nature = false)
        {
            string NoNatureStr = "oOLl9gqVvUuI1\"',./:;<>\\^_`|~";//没人性的字符
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true)
            {
                str += "0123456789";
            }
            if (useLow == true)
            {
                str += "abcdefghijklmnopqrstuvwxyz";
            }
            if (useUpp == true)
            {
                str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            if (useSpe == true)
            {
                str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
            }
            if (nature)
            {
                foreach (char c in NoNatureStr)
                {
                    str = str.Replace(c.ToString(), "");
                }
            }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        #endregion
        #region 反射

        /// <summary>
        /// 反射到类型
        /// </summary>
        /// <param name="ClassName">类型全名</param>
        /// <param name="NameSpace">所在命名空间</param>
        /// <returns></returns>
        public static Type ToType(this string ClassName, string NameSpace)
        {
            Type type = Type.GetType(ClassName + "," + NameSpace);
            if (type != null)
            {
                return type;
            }
            return null;
        }

        #endregion
        #region 加密
        public static string ToMD5(this string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = Encoding.UTF8.GetBytes(str);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }
        #endregion
    }
}
