using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace z.Exceptions
{
    public class FailException : zExceptionBase
    {
        public FailException(string msg) : base(msg)
        {

        }

        public override int Flag
        {
            get
            {
                return 999;
            }
        }
    }
}
