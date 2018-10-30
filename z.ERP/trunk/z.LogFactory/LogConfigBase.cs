using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using z.Extensions;

namespace z.LogFactory
{
    /// <summary>
    /// 日志配置类
    /// </summary>
    public class LogConfigBase
    {
        private LogUtils _utils;

        private string _FilePath;
        private string _pattern;
        private string _FileName;
        private LogLevel _loglevel;

        /// <summary>
        /// 最低日志级别
        /// 高于等于此级别的日志将被记录
        /// </summary>
        public LogLevel Loglevel
        {
            get { return _loglevel; }
            set { _loglevel = value; }
        }

        /// <summary>
        /// 日志文件名
        /// 默认yyyy年MM月dd日.'log'
        /// 限定Log4Net
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        /// <summary>
        /// 日志基础模板
        /// 默认%p %d %n %m %n
        /// 限定Log4Net
        /// </summary>
        public string Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }

        /// <summary>
        /// 文件路径
        /// 默认Logger
        /// 限定Log4Net
        /// </summary>
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }
        /// <summary>
        /// 日志记录工具
        /// </summary>
        public LogUtils Utils
        {
            get { return _utils; }
            set { _utils = value; }
        }

        /// <summary>
        /// 进行处理,合并默认值
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static LogConfigBase Fix(LogConfigBase conf)
        {
            if (conf.FileName.IsEmpty())
                conf.FileName = Default.FileName;
            if (conf.FilePath.IsEmpty())
                conf.FilePath = Default.FilePath;
            if (conf.Pattern.IsEmpty())
                conf.Pattern = Default.Pattern;
            return conf;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public static LogConfigBase Default
        {
            get
            {
                return new LogConfigBase()
                {
                    Utils = LogUtils.Log4Net,
                    FilePath = "Logger",
                    Pattern = "[%t] %p %d %m %n",
                    FileName = "yyyy年MM月dd日.'log'",
                    Loglevel = LogLevel.Debug
                };
            }
        }

    }
}
