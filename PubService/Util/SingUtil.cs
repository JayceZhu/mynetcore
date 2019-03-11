using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PubService
{
    public class SingUtil
    {

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="appid"></param>
        ///  <param name="nonceStr"></param>
        /// <param name="appsecret"></param>
        /// <param name="timestamp"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Sign(string appid,string appsecret, string nonceStr, string timestamp, string data, string token = null)
        {
            //string nonceStr = CreateNonceStr();
           // long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;

            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名参数
            var signStr = timestamp + nonceStr + appid + appsecret + data;
            //升序
            var sortStr = string.Concat(signStr.OrderBy(c => c));
            var bytes = Encoding.UTF8.GetBytes(sortStr);

            var md5Val = hash.ComputeHash(bytes);
            return BitConverter.ToString(md5Val).Replace("-", "").ToUpper();
        }

        public static string CreateNonceStr(int length = 16)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var str = "";
            var rand = new Random();
            for (var i = 0; i < length; i++)
            {
                str += chars.Substring(rand.Next(chars.Length), 1);
            }
            return str;
        }

        public static double ToTimeStamp(DateTime time)
        {
            TimeSpan timeSpan = time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return timeSpan.TotalSeconds;
        }

        public static string ZlanSign(System.Collections.IDictionary data, string privateKey)
        {
            StringBuilder sb = new StringBuilder();

            System.Collections.ArrayList akeys = new System.Collections.ArrayList(data.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)data[k];
                if (null != v && "".CompareTo(v) != 0 && "sign".CompareTo(k) != 0 && "appsecrect".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("appsecrect=" + privateKey);

            string sign = EncryptUtil.MD5Encrypt(sb.ToString()).ToUpper();


            return sign;
        }
    }
}