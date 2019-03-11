using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace PubService.Util
{
    public class ZlanAPICaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Call(string url, object param)
        {
            var responseStr = "";
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/json";

                string paramStr = "";
                if (param is string)
                {
                    paramStr = param as string;
                }
                else
                {
                    paramStr = JsonConvert.SerializeObject(param);
                }
                byte[] requestData = System.Text.Encoding.UTF8.GetBytes(paramStr);
                request.ContentLength = requestData.Length;

                Stream newStream = request.GetRequestStream();
                newStream.Write(requestData, 0, requestData.Length);
                newStream.Close();

                var response = request.GetResponse();
                Stream ReceiveStream = response.GetResponseStream();

                using (StreamReader stream = new StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    responseStr = stream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                responseStr = "{\"ErrorCode\":\"-4\",\"ErrorMessage\":\"" + ex.Message + "\"}";
            }
            return responseStr;
        }

        /// <summary>
        /// 调用服务
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T Call<T>(string url, object param)
        {
            return JsonConvert.DeserializeObject<T>(Call(url, param));
        }

        /// <summary>
        /// 调用商城前台
        /// </summary>
        /// <param name="key"></param>
        /// <param name="paramJSON"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JObject ExecuteShop(string key, object paramJSON = null)
        {
            return Execute(ConfigurationUtil.GetSection("ZlanApi")["APP_ID"], ConfigurationUtil.GetSection("ZlanApi")["APP_SECRECT"], ConfigurationUtil.GetSection("ZlanApi")["APP_URL"], key, paramJSON);
        }

        /// <summary>
        /// 调用商城后台
        /// </summary>
        /// <param name="key"></param>
        /// <param name="paramJSON"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JObject ExecuteSys(string key, object paramJSON = null)
        {
            return Execute(ConfigurationUtil.GetSection("ZlanApi")["APP_ID"], ConfigurationUtil.GetSection("ZlanApi")["APP_SECRECT"], ConfigurationUtil.GetSection("ZlanApi")["SYS_URL"], key, paramJSON);
        }

        public static Newtonsoft.Json.Linq.JObject Execute(string appid, string appsecect, string url, string key, object paramJSON = null)
        {
            string param = "";
            if (paramJSON == null)
            {
                paramJSON = "{}";
            }
            if (paramJSON is string)
            {
                param = paramJSON as string;
            }
            else
            {
                param = JsonConvert.SerializeObject(paramJSON);
            }
            Hashtable paramHash = new Hashtable();
            paramHash["appid"] = appid;
            paramHash["key"] = key;
            paramHash["param"] = param;
            paramHash["tmp"] = string.Format("{0}", DateTime.Now.Ticks);

            //获取签名
            string signStr = SingUtil.ZlanSign(paramHash, appsecect);

            paramHash["sign"] = signStr;

            if (url.IndexOf("?") < 0) url += "?";
            else url += "&";
            var num = 0;
            foreach (var item in paramHash.Keys)
            {
                if (num != 0) url += "&";
                url += item + "=" + HttpUtility.UrlEncode(paramHash[item].ToString());
                num++;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            //获得响应流
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            Stream responseStream = null;
            if (response.ContentEncoding == null)
                responseStream = response.GetResponseStream();//封装代码处理
            else if (response.ContentEncoding.ToLower() == "gzip")//RFC1952封装
                responseStream = new System.IO.Compression.GZipStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
            else if (response.ContentEncoding.ToLower() == "deflate")//RFC1951
                responseStream = new System.IO.Compression.DeflateStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
            else//RFC1950
                responseStream = response.GetResponseStream();//封装代码处理 
            StreamReader sr = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strResult = sr.ReadToEnd();//读取流文件
            sr.Close();
            response.Close();// 8/1加入，关闭请求流


            LogUtil.LogText(key, key, string.Format("{0}=>{1}", url, strResult));
            return JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(strResult);
        }
    }
}
