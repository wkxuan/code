using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.LogFactory
{
    public partial class LogWriter
    {
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="title">标题</param>
        public void Info(string title)
        {
            Log(LogLevel.Info, title, null);
        }
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="obj">日志内容</param>
        public void Info(string title, params object[] obj)
        {
            Log(LogLevel.Info, title, obj);
        }


    }
}
