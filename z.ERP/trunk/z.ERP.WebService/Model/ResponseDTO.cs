using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.WebService
{
    public class ResponseDTO
    {
        public bool Success
        {
            get;
            set;
        }

        public string ErrorMsg
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