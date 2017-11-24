using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using z.Exceptions;

namespace z.Extensions
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionExtension
    {
        /// <summary>
        /// 取最内层的message
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string InnerMessage(this Exception ex)
        {
            if (ex.InnerException != null)
                return ex.InnerException.InnerMessage();
            else
                return ex.Message;
        }

        /// <summary>
        /// 取最内层的Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetInnerException(this Exception ex)
        {
            if (ex.InnerException != null)
                return ex.InnerException.GetInnerException();
            else
                return ex;
        }

    }
}
