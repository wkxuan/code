using NewtonsoftCode.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
        /// 是空的,含null,empty,纯空格
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string IsEmpty(this string str, string defaultValue)
        {
            return str.IsEmpty() ? defaultValue : str;
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
        /// 是数字,含小数
        /// </summary>
        /// <returns></returns>
        public static double IsNumber(this string str, double defaultValue)
        {
            double d;
            return str.IsNumber(out d) ? d : defaultValue;
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

        /// <summary>
        /// 是数字,含小数
        /// </summary>
        /// <returns></returns>
        public static int IsInt(this string str, int defaultValue)
        {
            int d;
            return str.IsInt(out d) ? d : defaultValue;
        }

        /// <summary>
        /// 字符串是否匹配当前表达式
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reg"></param>
        /// <returns></returns>
        public static bool IsRegexMatch(this string text, string reg)
        {
            if (text.IsEmpty())
                return false;
            return Regex.IsMatch(text, reg);
        }

        /// <summary>
        /// 匹配第一个符合的表达式
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reg"></param>
        /// <returns></returns>
        public static string IsRegexMatch(this IEnumerable<string> text, string reg)
        {
            if (text.IsEmpty())
                return null;
            foreach (string str in text)
            {
                if (str.IsRegexMatch(reg))
                    return str;
            }
            return null;
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
        /// 试图转换为数字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="act">转换成功执行</param>
        /// <returns></returns>
        public static int TryToInt(this string str, Action<int> act)
        {
            int i = 0;
            if (int.TryParse(str, out i))
            {
                act?.Invoke(i);
            }
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
        /// 字符串转Double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double i = 0;
            double.TryParse(str, out i);
            return i;
        }

        public static double ToDouble(this decimal str)
        {
            return (double)str;
        }

        public static decimal ToDecimal(this double str)
        {
            return (decimal)str;
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
            str = str.Trim().Replace('/', '-');
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
        #region 字符串验证

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
        /// 判断是邮件地址
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IgnoreEmpty">忽略空的,如果为true,那么当字符串为空时返回true</param>
        /// <returns></returns>
        public static bool IsEmail(this string str, bool IgnoreEmpty = false)
        {
            if (str.IsEmpty())
                return IgnoreEmpty;
            return Regex.IsMatch(str, "^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
        }

        /// <summary>
        /// 确认字符串在数组中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static bool In(this string str, params string[] strs)
        {
            if (strs == null || strs.Count() == 0)
                return false;
            return strs.Contains(str);
        }

        /// <summary>
        /// 确认字符串不在数组中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static bool NotIn(this string str, params string[] strs)
        {
            return !str.In(strs);
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
        public static string FillRight(this string str, char c, int length)
        {
            if (str.Length >= length)
            {
                return str.Substring(0, length);
            }
            else
            {
                return FillRight(str + c, c, length);
            }
        }

        /// <summary>
        /// 补齐字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="c">补足位</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string FillLeft(this string str, char c, int length)
        {
            if (str.Length >= length)
            {
                return str.Substring(str.Length - length);
            }
            else
            {
                return FillLeft(c + str, c, length);
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
        /// 获取一个随机种子
        /// </summary>
        /// <returns></returns>
        public static Random GetOneRandom()
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            return new Random(BitConverter.ToInt32(b, 0));
        }

        /// <summary>
        /// 在一堆字符串中随机挑一个
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string Random(params string[] strs)
        {
            if (strs.IsEmpty())
                return null;
            Random r = GetOneRandom();
            return strs[r.Next(strs.Length)];
        }

        /// <summary>
        /// 取一个随机数
        /// </summary>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <returns></returns>
        public static double Random(double min, double max)
        {
            Random r = GetOneRandom();
            return r.NextDouble() * (max - min) + min;
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
            Random r = GetOneRandom();
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

        /// <summary>
        /// 针对数字的遍历方法
        /// </summary>
        /// <param name="number"></param>
        /// <param name="act"></param>
        public static void ForEach(this int number, Action<int> act)
        {
            if (number >= 0)
            {
                for (int i = 0; i < number; i++)
                {
                    act(i);
                }
            }
        }

        /// <summary>
        /// 左相似
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strleft"></param>
        /// <returns></returns>
        public static bool LeftLike(this string str, string strleft)
        {
            return str.IndexOf(strleft) == 0;
        }

        /// <summary>
        /// 从右边切掉指定个字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutRight(this string str, int length)
        {
            if (str.IsEmpty() || str.Length < length)
                return str;
            else if (str.Length == length)
                return "";
            return str.SubstringSafe(0, str.Length - length);
        }

        /// <summary>
        /// 从左边切掉指定个字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutLeft(this string str, int length)
        {
            if (str.IsEmpty() || str.Length < length)
                return str;
            else if (str.Length == length)
                return "";
            return str.SubstringSafe(length);
        }

        /// <summary>
        /// 替换第一个匹配项
        /// </summary>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceOne(this string source, string match, string replacement)
        {
            Func<char[], char[], int> IndexOf = (s, m) =>
            {
                int inx = -1;
                for (int i = 0; i < s.Length - m.Length + 1; i++)
                {
                    if (s[i] == m[0])
                    {
                        bool isMatch = true;
                        for (int j = 0; j < m.Length; j++)
                        {
                            if (s[i + j] != m[j])
                            {
                                isMatch = false;
                                break;
                            }
                        }
                        if (isMatch)
                        {
                            inx = i;
                            break;
                        }
                    }
                }
                return inx;
            };
            char[] sArr = source.ToCharArray();
            char[] mArr = match.ToCharArray();
            char[] rArr = replacement.ToCharArray();
            int idx = IndexOf(sArr, mArr);
            if (idx == -1)
            {
                return source;
            }
            else
            {
                return new string(sArr.Take(idx).Concat(rArr).Concat(sArr.Skip(idx + mArr.Length)).ToArray());
            }
        }

        /// <summary>
        /// 倒序排列
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToReverse(this string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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
        #region 系统信息

        /// <summary>
        /// 获取CPU序列号代码 
        /// </summary>
        /// <returns></returns>
        public static string GetCpuID()
        {
            try
            {
                string cpuInfo = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return null;
            }
            finally
            {
            }

        }

        /// <summary>
        /// mac
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址 
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return null;
            }
            finally
            {
            }

        }

        /// <summary>
        /// 获取硬盘ID
        /// </summary>
        /// <returns></returns>
        public static string GetDiskID()
        {
            try
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return null;
            }
            finally
            {
            }

        }

        /// <summary> 
        /// 操作系统的登录用户名 
        /// </summary> 
        /// <returns></returns> 
        public static string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["UserName"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return null;
            }
            finally
            {
            }

        }

        /// <summary> 
        /// PC类型 
        /// </summary> 
        /// <returns></returns> 
        public static string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "";
            }
            finally
            {
            }

        }

        /// <summary> 
        /// 物理内存 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTotalPhysicalMemory()
        {
            try
            {

                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        /// <summary> 
        /// 计算机名
        /// </summary> 
        /// <returns></returns> 
        public static string GetComputerName()
        {
            try
            {
                return Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        #endregion
    }
}
