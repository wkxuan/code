﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using z.Extensions;

namespace z.LogFactory.utils
{
    /// <summary>
    /// 日志记录工具基类
    /// </summary>
    public abstract class LogUtilsBase
    {
        /// <summary>
        /// 处理日志格式
        /// </summary>
        /// <param name="config"></param>
        /// <param name="title"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string FixLog(LogConfigBase config, string title, object[] obj)
        {
            return title + ":\r\n" + obj.ToJson();
        }
    }
}