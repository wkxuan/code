using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using z.CacheBox;
using z.Encryption;

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
        /// 取配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetEncryptConfig(string key, string defaultValue = "")
        {
            var str = ConfigurationManager.AppSettings[key];
            if (str.IsEmpty())
                return defaultValue;
            if (str.Contains(EncryptSplit) && str.Length > 100)
            {
                return RSAEncryption.Decrypt(EncryptKey, str.Substring(str.IndexOf(EncryptSplit) + EncryptSplit.Length));
            }
            else
            {
                return str;
            }
        }
        static readonly string EncryptSplit = "@@";
        static readonly string EncryptKey = "<RSAKeyValue><Modulus>1tsHuG5yq8FHWBm8yFyGCDeqGHYwJxJ9Tj8eExxYSigVUH1XrDlscgertN3pkZEKc6aP7+cLmdDFAbFKwhVUqweZAS8YebWEfIjy+bUIJ7HLzhTyfPYmKjPTiUBjEqIYj+3xHTBiH8bXhUxDjOtJxCEiZbe5vaPAJ8kaJNR0yvE=</Modulus><Exponent>AQAB</Exponent><P>9lA9iRJw5uW4u2HR7cruwlSUJQfxm+BtL3szZJmXpjGl4mJS1M+BJVtiTovB2rpAf7nYE6Fw3+fcNfYdROROgw==</P><Q>304WhW7WmK8uzus+N2DTjjtMBmXhvUm47gRRWUFwypMvhY9ZKPeWlR0SLBhg7YhF+qKA5wwOxwicSniah6QGew==</Q><DP>HgDSLhM3+3g6E2EsACo7ASLqVMRt8s3YnvMD5Jos9cqQaU4Oxutr0NAb3nN5rpoHZ0eNAX8lz7Bfi5cqI40n3w==</DP><DQ>Q0OFWyoY8CMMyX1o30uGTjikXOUBi4ASeXfJfUZOEGcnkGaup710mXQJTkkFoWdEFQwwIeiq5t88HN6ZRbRt0w==</DQ><InverseQ>OO5n/Q/KPMBLV9FqPKW8MwlL5LOGERmGqWEZOwdinr/71QaActcoemgf4S9ofHBjV6dYRUaBuu2VYw5REqCeBw==</InverseQ><D>TvM5SQo81OQ25Sa/+hgVoFtkA40acKEYrnH/CSK3Rrin7GXCm6SWNuierd2FgFn9rzWbWppZ5vGSEuclA2B3NZ0J8igxpt+u498W9FZKtG4gaXIPr4viB1Z1FRsqBiTOEZI/pUblNPXm0PVWUNgCRyNLT08Aep61akGXzH7G4yk=</D></RSAKeyValue>";
        
        static string _EncryptConfig(string start, string key)
        {
            return start + EncryptSplit + RSAEncryption.Encrypt(EncryptKey, key);
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

        /// <summary>
        /// 获取一个配置节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetSection<T>() where T : ConfigurationSection
        {
            return GetSection<T>(typeof(T).Name);
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
                return GetConfig("TestModel").ToLower().StartsWith("true");
            }
        }
        /// <summary>
        /// 测试模式
        /// </summary>
        public static string TestModel_User
        {
            get
            {
                if (TestModel)
                {
                    string[] arr = GetConfig("TestModel").Split(':');
                    if (arr.Length == 2)
                        return arr[1];
                    else
                        return "-1";
                }
                else
                    return "-1";
            }
        }
    }
}
