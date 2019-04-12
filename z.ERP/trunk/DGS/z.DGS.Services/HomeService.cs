using System.Data;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.SSO.Model;
using System.Linq;
using z.Encryption;
using z.DBHelper.DBDomain;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace z.DGS.Services
{
    public class HomeService : ServiceBase
    {
        internal HomeService()
        {
        }

        public User GetUserByCode(string code, string password)
        {
            bool b = DbHelper.ExecuteTable("select count(1) from STATION where TYPE=3  and STATIONBH= {{code}}", new zParameter("code", code)).Rows.Count > 0;
            if (!b)
                throw new Exception($"款台{code}不存在");
            if (password != MD5Encryption.Encrypt($"z.DGS.LoginSalt{code}"))
                throw new Exception($"款台{code}给定的密钥不正确");
            return new User()
            {
                Code = code,
                Id = code,
                Name = code
            };
        }
        

    }
}
