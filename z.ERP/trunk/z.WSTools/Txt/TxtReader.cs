using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.WSTools.Txt
{
    /// <summary>
    /// 文本转换
    /// </summary>
    public static class TxtReader
    {
        /// <summary>
        /// 转化为table
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static DataTable ReadToDatatable(TxtReaderSettings settings)
        {
            if (settings == null)
                throw new Exception($"没有配置项");
            return settings.ReadTable();
        }

        /// <summary>
        /// 转化为model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static List<T> ReadToModel<T>(TxtReaderSettings settings) where T : class
        {
            return ReadToDatatable(settings).ToList<T>();
        }
    }
}
