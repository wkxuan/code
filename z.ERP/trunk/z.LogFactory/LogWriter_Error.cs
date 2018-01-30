using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.LogFactory
{
    public partial class LogWriter
    {
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="title">标题</param>
        public void Error(string title)
        {
            Log(LogLevel.Error, title, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
            Log(LogLevel.Error, "无标题的错误", ex, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="ex"></param>
        public void Error(string title, Exception ex)
        {
            Log(LogLevel.Error, title, ex, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="title"></param>
        /// <param name="ex"></param>
        /// <param name="obj">错误对应的数据类</param>
        public void Error(string title, Exception ex, params object[] obj)
        {
            Log(LogLevel.Error, title, ex.Message, ex.StackTrace, obj);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="obj">日志内容</param>
        public void Error(string title, params object[] obj)
        {
            Log(LogLevel.Error, title, obj);
        }

    }
}
