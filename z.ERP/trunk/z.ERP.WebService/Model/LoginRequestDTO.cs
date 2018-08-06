using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.WebService.Model
{
    public class LoginRequestDTO
    {
        public string UserName
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