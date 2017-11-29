using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.CacheBox
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void Set<T>(string key, T data);

        /// <summary>
        /// 简单的缓存使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="IsNull"></param>
        /// <returns></returns>
        T Simple<T>(string key, Func<T> IsNull);
    }
}
