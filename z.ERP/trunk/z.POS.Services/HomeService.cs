using System.Data;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.SSO.Model;
using System.Linq;
using z.Encryption;

namespace z.POS.Services
{
    public class HomeService : ServiceBase
    {
        protected const string LoginSalt = "z.SSO.LoginSalt.1";
        internal HomeService()
        {
        }

        public User GetUserByCode(string code, string password)
        {
            return new User()
            {
                Code = "000012",
                Id = "13",
                Name = "杨梦云"
            };
        }
    }
}
