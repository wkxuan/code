using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.Exceptions
{
    public abstract class zExceptionBase : Exception
    {
        public zExceptionBase(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// 异常标记
        /// </summary>
        public abstract int Flag
        {
            get;
        }
    }
}
