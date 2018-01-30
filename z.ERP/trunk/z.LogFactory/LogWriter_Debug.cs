using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.LogFactory
{
    public partial class LogWriter
    {
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="title">标题</param>
        public void Debug(string title)
        {
            Log(LogLevel.Debug, title, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="obj">内容</param>
        public void Debug(params object[] obj)
        {
            Log(LogLevel.Debug, "调试信息", obj);
        }


        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="obj">内容</param>
        public void Debug(string title, params object[] obj)
        {
            Log(LogLevel.Debug, title, obj);
        }


    }
}
