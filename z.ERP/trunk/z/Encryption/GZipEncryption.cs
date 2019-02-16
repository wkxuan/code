using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace z.Encryption
{
    public static class GZipEncryption
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(value.ToString());
                byte[] zippedData = Compress(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }
        }

        static byte[] Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] zippedData = Convert.FromBase64String(value.ToString());
                return (string)(System.Text.Encoding.UTF8.GetString(Decompress(zippedData)));
            }
        }
    }

}
