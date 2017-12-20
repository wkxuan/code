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
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                FileAttributes attr = File.GetAttributes(path);
                if (attr == FileAttributes.Directory)
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    File.Delete(path);
                }
            }
        }

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
        }

        /// <summary>
        /// 获取物理目录下的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns>文件</returns>
        public static List<FileInfo> GetAllFiles(string path)
        {
            _files = new List<FileInfo>();
            return _GetAllFile(path);
        }
        static List<FileInfo> _files = new List<FileInfo>();
        static List<FileInfo> _GetAllFile(string path)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            if (TheFolder.Exists)
            {
                //遍历文件夹
                foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                    _GetAllFile(NextFolder.FullName);
                //遍历文件
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                    _files.Add(NextFile);
            }
            return _files;
        }

        /// <summary>
        /// 获取物理目录下的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns>文件</returns>
        public static List<DirectoryInfo> GetAllDirectorys(string path)
        {
            _dics = new List<DirectoryInfo>();
            return _GetAllDirectory(path);
        }
        static List<DirectoryInfo> _dics = new List<DirectoryInfo>();
        static List<DirectoryInfo> _GetAllDirectory(string path)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            if (TheFolder.Exists)
            {
                //遍历文件夹
                foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                    _GetAllDirectory(NextFolder.FullName);
                _dics.Add(TheFolder);
            }
            return _dics;
        }

        /// <summary>  
        /// 计算文件大小函数(保留两位小数),Size为字节大小  
        /// </summary>  
        /// <param name="Size">初始文件大小</param>  
        /// <returns></returns>  
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        /// <summary>
        /// 获取文件相对路径
        /// </summary>
        /// <param name="strPath1"></param>
        /// <param name="strPath2"></param>
        /// <returns></returns>
        public static string GetRelativePath(string strPath1, string strPath2)
        {
            if (!strPath1.EndsWith("//"))
                strPath1 += "//";    //如果不是以"/"结尾的加上"/"
            int intIndex = -1, intPos = strPath1.IndexOf("//");
            //以"/"为分界比较从开始处到第一个"/"处对两个地址进行比较,如果相同则扩展到
            //下一个"/"处;直到比较出不同或第一个地址的结尾.
            while (intPos >= 0)
            {
                intPos++;
                if (string.Compare(strPath1, 0, strPath2, 0, intPos, true) != 0)
                    break;
                intIndex = intPos;
                intPos = strPath1.IndexOf("//", intPos);
            }

            //如果从不是第一个"/"处开始有不同,则从最后一个发现有不同的"/"处开始将strPath2
            //的后面部分付值给自己,在strPath1的同一个位置开始望后计算每有一个"/"则在strPath2
            //的前面加上一个"../"(经过转义后就是"..//").
            if (intIndex >= 0)
            {
                strPath2 = strPath2.Substring(intIndex);
                intPos = strPath1.IndexOf("//", intIndex);
                while (intPos >= 0)
                {
                    strPath2 = "..//" + strPath2;
                    intPos = strPath1.IndexOf("//", intPos + 1);
                }
            }
            //否则直接返回strPath2
            return strPath2;
        }
        #endregion
    }
}
