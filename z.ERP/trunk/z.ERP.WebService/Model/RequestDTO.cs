using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.WebService
{
    public class RequestDTO
    {
        public string SecretKey
        {
            get;
            set;
        }

        public string ServiceName
        {
            get;
            set;
        }

        public string Context
        {
            get;
            set;
        }
    }
}