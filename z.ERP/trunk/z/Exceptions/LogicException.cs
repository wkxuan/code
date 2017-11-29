using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.Exceptions
{
    /// <summary>
    /// 逻辑错误
    /// </summary>
    public class LogicException : zExceptionBase
    {
        public LogicException(string msg)
            : base(msg)
        {

        }
        public override int Flag
        {
            get
            {
                return 102;
            }
        }
    }
}
