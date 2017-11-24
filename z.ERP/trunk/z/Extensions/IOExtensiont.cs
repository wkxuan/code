using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace z.Extensions
{
    public static class IOExtension
    {
        #region 地址方法
        /// <summary>
        /// 获取当前机器名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            try
            {
                return Dns.GetHostName();
            }
            catch
            {
                return "未知";
            }
        }

        /// <summary>
        /// 取当前动态库所在目录
        /// </summary>
        /// <returns></returns>
        public static string GetBaesDir()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }


        /// <summary>
        /// 组成文件目录(不创建)
        /// </summary>
        /// <param name="str">目录拼接</param>
        /// <returns></returns>
        public static string MakeDir(params string[] str)
        {
            return MakeDir(false, str);
        }

        /// <summary>
        /// 组成文件目录
        /// </summary>
        /// <param name="MakeDir">创建该目录(仅本机有效)</param>
        /// <param name="dir">目录拼接</param>
        /// <returns></returns>
        public static string MakeDir(bool MakeDir, params string[] dir)
        {
            string retstr = "";
            foreach (string s in dir)
            {
                string str = fixdir(s);
                //首个节点
                if (string.IsNullOrEmpty(retstr))
                {
                    retstr += str;
                }
                else
                {
                    if (str.StartsWith(@"\"))
                        retstr += str;
                    else
                        retstr += @"\" + str;
                }
            }
            retstr += @"\";
            if (MakeDir)
            {
                if (!Directory.Exists(retstr))
                {
                    Directory.CreateDirectory(retstr);
                }
            }
            return retstr;
        }

        /// <summary>
        /// 规范格式,去掉尾部
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string fixdir(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            string s = str.Replace("/", @"\");
            s = s.Replace(@"\\", @"\");
            if (s.IndexOf(@"\\") >= 0)
            {
                s = fixdir(s);
            }
            if (s.EndsWith(@"\"))
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }

        /// <summary>
        /// uri拼接
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string MakeUri(params string[] uri)
        {
            string retstr = "";
            foreach (string s in uri)
            {
                string str = fixuri(s);
                if (str.IsEmpty())
                    continue;
                //首个节点
                if (string.IsNullOrEmpty(retstr))
                {
                    retstr += str;
                }
                else
                {
                    if (str.StartsWith("/"))
                        retstr += str;
                    else
                        retstr += "/" + str;
                }
            }
            return retstr;
        }

        /// <summary>
        /// 规范格式,去掉尾部
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string fixuri(string str)
        {
            string s = str.Replace(@"\", "/");
            if (!(s.StartsWith("http:/", true, null) || s.StartsWith("https:/", true, null)))
            {
                s = s.Replace("//", "/");
                if (s.IndexOf("//") >= 0)
                {
                    s = fixdir(s);
                }
            }
            if (s.EndsWith(@"/"))
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }
        #endregion
        #region 文件方法
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="text"></param>
        /// <param name="isAppend"></param>
        /// <param name="encoding"></param>
        public static void WriteFile(string filepath, string text, bool isAppend = false, Encoding encoding = null)
        {
            FileStream fs = new FileStream(filepath, isAppend ? FileMode.Append : FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, encoding == null ? Encoding.Default : encoding);
            sw.Write(text);
            sw.Close();
            fs.Close();
        }
        #endregion
    }
}
