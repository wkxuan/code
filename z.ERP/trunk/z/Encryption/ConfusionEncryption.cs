using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z.Extensions;

namespace z.Encryption
{
    /// <summary>
    /// 数字混淆加密算法
    /// </summary>
    public static class ConfusionEncryption
    {
        static string[] _key1 = new string[] {
            "8418727812",
            "5401722756",
            "1924493452",
            "7577976761",
            "7805364986",
            "6321133705",
            "1484100378",
            "4588890718",
            "1401482927",
            "5404015356"
        };
        static string[] _key2 = new string[] {
            "4010560068",
            "8007614826",
            "2315587534",
            "4011704812",
            "1566029945",
            "7907283280",
            "4099497040",
            "7937708988",
            "2501611830",
            "4935213027"
        };
        static string _key3 = "51";


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="num">待加密数字</param>
        /// <param name="length">结果位数</param>
        /// <returns></returns>
        public static string Encrypt(Int64 num, int length)
        {
            if (num.ToString().Length > length)
                throw new Exception($"待加密数字{num}大于位数{length}");
            if (length < 2)
                throw new Exception("最小支持2位数");
            string a = num.ToString().PadLeft(length, '0');
            string key1 = _key1[a[length - 1].ToString().ToInt()];  //个位
            string key2 = _key2[a[length - 2].ToString().ToInt()];  //十位
            string key = cel(key1, key2);  //最终的key
            string b = a.Substring(0, a.Length - 2);   //前几位
            string c = a.Substring(a.Length - 2);  //后两位
            string d = cel(b, key);   //前几位编码
            string e = cel(c.ToReverse(), _key3) + d;  //后两位倒序编码
            return e;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int64 Decrypt(string str)
        {
            if (str.Length < 2)
                throw new Exception("最小支持2位数");
            string a = str.Substring(0, 2);  //后两位
            string b = fcel(a, _key3).ToReverse(); //后两位明文
            string key1 = _key1[b[1].ToString().ToInt()];  //个位
            string key2 = _key2[b[0].ToString().ToInt()];  //十位
            string key = cel(key1, key2);  //最终的key
            string c = str.Substring(2, str.Length - 2);//前几位
            string d = fcel(c, key);//解密前几位
            string e = d + b;
            return Convert.ToInt64(e);
        }


        static string cel(string str, string key)
        {
            string res = "";
            str.ToCharArray().ForEach2((c, i) =>
            {
                int a = c.ToString().ToInt();
                int num = key[i % key.Length].ToString().ToInt();
                res += (a + num >= 10 ? a + num - 10 : a + num).ToString();
            });
            return res;
        }

        static string fcel(string str, string key)
        {
            string res = "";
            str.ToCharArray().ForEach2((c, i) =>
            {
                int a = c.ToString().ToInt();
                int num = key[i % key.Length].ToString().ToInt();
                res += (a - num < 0 ? a + 10 - num : a - num).ToString();
            });
            return res;
        }
    }
}
