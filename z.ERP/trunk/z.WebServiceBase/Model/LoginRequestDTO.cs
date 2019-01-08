using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.WebServiceBase.Model
{
    public class LoginRequestDTO
    {
        public string UserCode
        {
            get;
            set;
        }

        public string UserPassword
        {
            get;
            set;
        }

        public string PlatformId
        {
            get;
            set;
        }
    }
}