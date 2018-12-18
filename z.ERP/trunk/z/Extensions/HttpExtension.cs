using NewtonsoftCode.Json;
using System;
using System.Web;

namespace z.Extensions
{
    public class HttpExtension
    {
        public static string op = "op";

        public static T GetRequestParam<T>()
        {
            string str = GetRequestParam(typeof(T).Name);
            if (string.IsNullOrEmpty(str))
                return default(T);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static T GetRequestParam<T>(string key)
        {
            string str = GetRequestParam(key);
            if (string.IsNullOrEmpty(str))
                return default(T);
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 获取 request中参数 包含get 及 post 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRequestParam(string key)
        {
            return HttpContext.Current.Request.QueryString[key] == null
                ? HttpContext.Current.Request.Form[key] == null ? "" : HttpContext.Current.Request.Form[key].ToString()
                : HttpContext.Current.Request.QueryString[key].ToString();
        }

        /// <summary>
        /// 获取 request中参数 包含get 及 post 
        /// </summary>
        /// <param name="request">request 请求</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRequestParam(HttpRequest request, string key)
        {
            string backValue = "";
            if (request.RequestType.ToUpper() == "GET")
            {
                backValue = request.QueryString[key] == null ? "" : request.QueryString[key].ToString();
            }
            else
            {
                backValue = request.Form[key] == null ? "" : request.Form[key].ToString();
            }
            //backValue = HttpUtility.HtmlEncode(backValue);
            return backValue;
        }

        /// <summary>
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIp(HttpContext context)
        {
            string result = String.Empty;
            result = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理
                if (result.IndexOf(".") == -1)     //没有“.”肯定是非IPv4格式
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (temparyip[i].IsIPAddress()
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];     //找到不是内网的地址
                            }
                        }
                    }
                    else if (result.IsIPAddress()) //代理即是IP格式
                        return result;
                    else
                        result = null;     //代理中的内容 非IP，取IP
                }

            }
            if (null == result || result == String.Empty)
                result = context.Request.ServerVariables["REMOTE_ADDR"];

            if (result == null || result == String.Empty)
                result = context.Request.UserHostAddress;

            return result;
        }
        
        /// <summary>
        /// 获取当前网站的虚拟根目录
        /// </summary>
        /// <returns></returns>
        public static string GetWebBasePath()
        {
            return IOExtension.MakeUri("http://", HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath);
        }
    }
}
