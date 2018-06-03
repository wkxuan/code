using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using z.CacheBox;

namespace z.Extensions
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public static class ConfigExtension
    {
        /// <summary>
        /// 取配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetConfig(string key, string defaultValue = "")
        {
            var a = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(a))
            {
                return defaultValue;
            }
            else
                return a;
        }

        /// <summary>
        /// 获取一个配置节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetSection<T>(string name) where T : ConfigurationSection
        {
            return ConfigurationManager.GetSection(name) as T;
        }

        ///// <summary>
        ///// 读取自定义配置文件
        ///// </summary>
        ///// <returns></returns>
        //public static List<T> GetConfig<T>(string filename, string key)
        //{
        //    ICache c = new WebCache();
        //}

        //public static void SetConfig<T>(string filename, string key, T data)
        //{
        //    using (StreamReader sr = new StreamReader(IOExtension.MakeDir(IOExtension.GetBaesDir(), "Config", filename)))
        //    {
        //        XmlSerializer xmldes = new XmlSerializer(typeof(T));
        //        var re = xmldes.Deserialize(sr);
        //    }
        //}

        /// <summary>
        /// 测试模式
        /// </summary>
        public static bool TestModel
        {
            get
            {
                return GetConfig("TestModel").ToLower() == "true";
            }
        }
    }
}
