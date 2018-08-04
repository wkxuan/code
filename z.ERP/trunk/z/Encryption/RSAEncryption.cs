using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace z.Encryption
{
    /// <summary>
    /// RSA
    /// </summary>
    public static class RSAEncryption
    {
        /// <summary>  
        /// 加密
        /// </summary>  
        /// <param name="xmlPublicKey">公钥</param>  
        /// <param name="str">待加密的字符串</param>  
        /// <returns></returns>  
        public static string Encrypt(string xmlPublicKey, string str)
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Encoding.UTF8.GetBytes(str);
                rsaProvider.FromXmlString(xmlPublicKey);
                int bufferSize = (rsaProvider.KeySize / 8) - 11;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var encryptedBytes = rsaProvider.Encrypt(temp, false);
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return Convert.ToBase64String(outputStream.ToArray());
                }
            }
        }

        /// <summary>  
        /// 加密
        /// </summary>  
        /// <param name="xmlPublicKey">公钥</param>  
        /// <param name="str">待加密的字符串</param>  
        /// <param name="Resstr">加密结果</param>
        /// <returns></returns>
        public static bool TryEncrypt(string xmlPublicKey, string str, out string Resstr)
        {
            try
            {
                Resstr = Encrypt(xmlPublicKey, str);
                return true;
            }
            catch
            {
                Resstr = null;
                return false;
            }
        }

        /// <summary>  
        /// 解密
        /// </summary>  
        /// <param name="xmlPrivateKey">私钥</param>  
        /// <param name="str">待解密的字符串</param>  
        /// <returns></returns>  
        public static string Decrypt(string xmlPrivateKey, string str)
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Convert.FromBase64String(str);
                rsaProvider.FromXmlString(xmlPrivateKey);
                int bufferSize = rsaProvider.KeySize / 8;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var rawBytes = rsaProvider.Decrypt(temp, false);
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }

        /// <summary>  
        /// 解密
        /// </summary>  
        /// <param name="xmlPrivateKey">私钥</param>  
        /// <param name="str">待解密的字符串</param>  
        /// <param name="Resstr">解密结果</param>
        /// <returns></returns>
        public static bool TryDecrypt(string xmlPrivateKey, string str, out string Resstr)
        {
            try
            {
                Resstr = Decrypt(xmlPrivateKey, str);
                return true;
            }
            catch
            {
                Resstr = null;
                return false;
            }
        }


        /// <summary>  
        /// 产生密钥  
        /// </summary>  
        /// <param name="xmlKeys">私钥</param>  
        /// <param name="xmlPublicKey">公钥</param>  
        public static void GetKey(out string xmlKeys, out string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }
    }
}
