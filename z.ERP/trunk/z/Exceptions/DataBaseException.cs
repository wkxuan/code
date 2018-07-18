using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z.Extensions;

namespace z.Exceptions
{
    public class DataBaseException : zExceptionBase
    {
        public DataBaseException(string msg)
            : base(msg)
        {

        }

        public DataBaseException(string msg, string sql) 
            : base(msg + (ConfigExtension.TestModel ? ("\r\n相关语句为：\r\n" + sql) : ""))
        {

        }

        public DataBaseException(string msg, string sql, object prams) 
            : base(msg + (ConfigExtension.TestModel ? ("\r\n相关语句为：\r\n" + sql + "\r\n相关参数为：\r\n" + prams.ToJson()) : ""))
        {

        }

        public override int Flag
        {
            get
            {
                return 101;
            }
        }
    }
}
