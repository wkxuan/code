using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using z.LogFactory.utils;
using NewtonsoftCode.Json;

namespace z.LogFactory
{
    /// <summary>
    /// 日志记录器入口
    /// </summary>
    public partial class LogWriter
    {

        private string _logName;

        #region 初始化
        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="LogName">标题</param>
        public LogWriter(string LogName)
        {
            if (string.IsNullOrEmpty(LogName))
            {
                throw new Exception("日志名称不能初始化为空");
            }
            _logName = LogName;
        }


        #endregion

        /// <summary>
        /// 日志入口
        /// </summary>
        /// <param name="loglevel"></param>
        /// <param name="title"></param>
        /// <param name="obj"></param>
        void Log(LogLevel loglevel, string title, params object[] obj)
        {
            if (string.IsNullOrEmpty(title.Trim()))
            {
                throw new Exception("非调试信息的日志,标题不允许为空");
            }
            try
            {
                LogConfigBase config = GetConfig(_logName);
                if (config.Loglevel > loglevel)
                {
                    return;   //判断日志级别,不正确的级别就不记日志了
                }
                switch (config.Utils)
                {
                    case LogUtils.Log4Net:
                        {
                            utils.Log4Net.Log(config, _logName, loglevel, title, obj);
                            break;
                        }
                }
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("未找到组件" + ex.Source);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception("未找到组件" + ex.FileName);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("配置文件:" + ex.Source + "错误:" + ex.Message + ".请删除或修复配置文件");
            }
            catch (IOException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                //其他错误就不抛了
                throw ex;
            }
        }


        #region 配置文件
        /// <summary>
        /// 取配置文件,取不到就是ALL
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        internal static LogConfigBase GetConfig(string Title)
        {
            //静态里没有,就开始读文件初始化
            if (ConfigDic == null || ConfigDic.Count == 0)
            {
                ConfigDic = new Dictionary<string, LogConfigBase>();
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Log.config";
                if (File.Exists(path))
                {

                    StreamReader sr = File.OpenText(path);
                    string jsonArrayText = sr.ReadToEnd();
                    sr.Close();
                    ConfigDic = JsonConvert.DeserializeObject<Dictionary<string, LogConfigBase>>(jsonArrayText);
                    //没有ALL节点,加上默认
                    if (!ConfigDic.Keys.Contains("ALL"))
                    {
                        ConfigDic.Add("ALL", Default);
                    }
                }
                //不存在配置文件,加上默认
                else
                {
                    ConfigDic.Add("ALL", Default);
                }
            }

            //从静态里取配置
            if (ConfigDic.Keys.Contains(Title))
            {
                return ConfigDic[Title];
            }
            else
            {
                return ConfigDic["ALL"];
            }

        }


        private static Dictionary<string, LogConfigBase> _ConfigDic = new Dictionary<string, LogConfigBase>();
        /// <summary>
        /// 静态配置文件
        /// </summary>
        public static Dictionary<string, LogConfigBase> ConfigDic
        {
            get
            {
                return _ConfigDic;
            }
            set { _ConfigDic = value; }
        }


        /// <summary>
        /// 默认配置
        /// </summary>
        static LogConfigBase Default
        {
            get
            {
                return new LogConfigBase()
                {
                    Utils = LogUtils.Log4Net,
                    FilePath = "Logger",
                    Pattern = "[%t] %p %d %n %m %n",
                    FileName = "yyyy年MM月dd日.'log'",
                    Loglevel = LogLevel.Debug
                };
            }
        }
        #endregion
    }
}
