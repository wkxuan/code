using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.LogFactory
{
    public partial class LogWriter
    {
        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="title">标题</param>
        public void Fatal(string title)
        {
            Log(LogLevel.Fatal, title, null);
        }
        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="obj">日志内容</param>
        public void Fatal(string title, params object[] obj)
        {
            Log(LogLevel.Fatal, title, obj);
        }

    }
}
