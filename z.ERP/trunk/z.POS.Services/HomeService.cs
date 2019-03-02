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

namespace z.POS.Services
{
    public class HomeService : ServiceBase
    {
        protected const string LoginSalt = "z.SSO.LoginSalt.1";
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        internal HomeService()
        {
        }

        public User GetUserByCode(string code, string password)
        {
            string sql = "select a.person_id,a.person_name,a.rydm,b.login_password from BFPUB8.RYXX a join XTCZY b  on a.person_id=b.person_id where a.person_name={{code}}";
            DataTable dt = DbHelper.ExecuteTable(sql, new zParameter("code", code));
            if (dt.Rows.Count == 1)
            {
                string psw = dt.Rows[0]["login_password"].ToString();
                if (Decrypt(psw) == password)
                {
                    return new User()
                    {
                        Code = dt.Rows[0]["rydm"].ToString(),
                        Id = dt.Rows[0]["person_id"].ToString(),
                        Name = dt.Rows[0]["person_name"].ToString()
                    };
                }
                else
                {
                    throw new Exception("密码错误");
                }
            }
            else if (dt.Rows.Count > 1)
            {
                throw new Exception("用户重复");
            }
            else
            {
                throw new Exception("用户不存在");
            }
        }

        /// <summary>
        /// DELPHI解密字符串和V8.00相同
        /// </summary>
        /// <param name="InputBt">原解密byte[]</param>
        /// <returns>解密串</returns>
        public string Decrypt(byte[] InputBt)
        {
            UInt32 Key = 26493;
            UInt32 C1 = 21469;
            UInt32 C2 = 12347;
            //byte[] InputBt = Convert.FromBase64String(DecryptBt);
            string OutRst = "";
            byte[] bt = new byte[InputBt.Length];
            try
            {
                for (int i = 0; i < InputBt.Length; i++)
                {
                    bt[i] = (byte)(InputBt[i] ^ (Key >> 8));
                    Key = (UInt32)((InputBt[i] + Key) * C1 + C2);
                }
                OutRst = "";
                OutRst = Encoding.Default.GetString(bt);
                //for (int i = 0; i < bt.Length; i++)
                //{
                //    OutRst = OutRst + Convert.ToChar(bt[i]);
                //}
                OutRst = OutRst.TrimEnd();
                return OutRst;
            }
            catch
            {
                return OutRst;
            }
        }

        /// <summary>
        /// DELPHI解密字符串和V8.00相同
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public string Decrypt(string src)
        {
            List<byte> l = new List<byte>();
            for (int i = 0; i < src.Length; i += 2)
            {
                string sss = src[i].ToString() + src[i + 1].ToString();
                l.Add(Convert.ToByte(sss, 16));
            }
            return Decrypt(l.ToArray());
        }
        
    }
}
