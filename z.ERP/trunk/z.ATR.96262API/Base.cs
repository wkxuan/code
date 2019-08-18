using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using z.Encryption;
using z.Extensions;

namespace z.ATR._96262API
{
    public class Base
    {
        string BaseUrl
        {
            get
            {
                return ConfigExtension.GetConfig("96262API_URL");
            }
        }

        public TRequest Post<TResponse, TRequest>(string Url, TResponse Response)
        {
            string json = Response.ToJson();
            string md5 = MD5Encryption.Encrypt(json);
            string str = CreatePostHttpResponse(BaseUrl + Url, json + md5);
            return str.ToObj<TRequest>();
        }

        string CreatePostHttpResponse(string url, string json)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            var byteData = Encoding.UTF8.GetBytes(json);
            var length = byteData.Length;
            request.ContentLength = length;
            var writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                return sr.ReadToEnd();
        }
    }
}
