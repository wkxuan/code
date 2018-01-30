using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.LogFactory
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 消息
        /// </summary>
        Info = 1,
        /// <summary>
        /// 警告
        /// </summary>
        Woring = 2,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal = 4,
        /// <summary>
        /// 不记录日志
        /// </summary>
        NoLog
    }
}
