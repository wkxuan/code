using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.LogFactory
{
    public partial class LogWriter
    {
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="title">标题</param>
        public void Woring(string title)
        {
            Log(LogLevel.Woring, title, null);
        }
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="obj">日志内容</param>
        public void Woring(string title, params object[] obj)
        {
            Log(LogLevel.Woring, title, obj);
        }
    }
}
