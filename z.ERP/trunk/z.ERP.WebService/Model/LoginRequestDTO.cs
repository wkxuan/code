using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.WebService.Model
{
    public class LoginRequestDTO
    {
        public string userCode
        {
            get;
            set;
        }

        public string userPassword
        {
            get;
            set;
        }

        public string platformId
        {
            get;
            set;
        }
    }
}