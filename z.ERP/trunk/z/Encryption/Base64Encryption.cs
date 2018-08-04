using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace z.Encryption
{
    /// <summary>
    /// RSA
    /// </summary>
    public static class Base64Encryption
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="xmlPublicKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            if (value == null || value == "")
            {
                return "";
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt(string value)
        {
            if (value == null || value == "")
            {
                return "";
            }
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
