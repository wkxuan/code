using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.MVC5.Results
{
    /// <summary>
    /// 返回消息的枚举
    /// </summary>
    public static class UIResultType
    {
        /// <summary>
        /// 报错
        /// </summary>
        public static readonly int Exception = -1;
        /// <summary>
        /// 成功
        /// </summary>
        public static readonly int Success = 0;
        /// <summary>
        /// 未登录
        /// </summary>
        public static readonly int NoLogin = 100;
        /// <summary>
        /// 没有权限
        /// </summary>
        public static readonly int Forbidden = 101;
        /// <summary>
        /// 无效的请求参数
        /// </summary>
        public static readonly int UndefinedOp = 102;
        /// <summary>
        /// 没有数据
        /// </summary>
        public static readonly int NoData = 103;
    }
}
