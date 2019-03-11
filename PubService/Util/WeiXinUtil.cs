using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PubService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubService
{
    public class WeiXinUtil
    {
        private static string ConnectionString { get; set; }
        private static IHttpRequestServer _httpRequest;
        public WeiXinUtil(IConfiguration configuration, IHttpRequestServer httpRequestServer)
        {
            ConnectionString = configuration.GetConnectionString("ShopConnectString");
            _httpRequest = httpRequestServer;
        }
        public static string GetShopJSTicket()
        {

            string result = "";
            var connectionString = ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "select JS_TICKET_VALUE from wx_config WHERE owner_account=?owner LIMIT 1";
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.Add(new MySqlParameter()
                    {
                        ParameterName = "?owner",
                        Value = "admin"
                    });

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["JS_TICKET_VALUE"] as string;
                    }
                    reader.Close();
                }
                catch
                {

                    return "";
                }
            }
            return result;
        }

        public static string GetShopAppid()
        {
            string result = "";
            var connectionString = ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "select APP_ID from wx_config WHERE owner_account=?owner LIMIT 1";
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.Add(new MySqlParameter()
                    {
                        ParameterName = "?owner",
                        Value = "admin"
                    });

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["APP_ID"] as string;
                    }
                    reader.Close();
                }
                catch
                {

                    return "";
                }
            }
            return result;
        }

        //public static string GetJSTicket()
        //{
        //    return WxConfig.JsTicketValue;
        //}

        //public static string GetAppid()
        //{

        //}
        public static WxConfig WxConfig
        {
            get
            {
                using (CoreContext context = new CoreContext())
                {
                    return context.WxConfig.Where(w => w.Status == "1").FirstOrDefault();
                }
            }
        }
        public static string GetConfigScript(bool debugMode = false, string fromurl = null)
        {
            string nonceStr = CreateNonceStr();
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return GetConfigScript(nonceStr, timestamp, debugMode, fromurl);

        }

        public static string GetConfigScript(string nonceStr, long timestamp, bool debugMode = false, string fromurl = null)
        {
            string result = "";
            try
            {
                var debug = debugMode ? "true" : "false";

                var ticket = WxConfig.JsTicketValue;
                if (string.IsNullOrEmpty(ticket))
                {
                    throw new Exception("js_ticket获取失败");
                }
                ConfigData data = new ConfigData
                {
                    nonceStr = nonceStr,
                    timestamp = timestamp,
                    appId = WxConfig.AppId
                };

                string url = fromurl;

                if (string.IsNullOrEmpty(url))
                {
                    url = _httpRequest.AbsoluteUri;
                }

                data.url = url;
                // string str1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, data.nonceStr, data.timestamp, data.url);

                data.signature = CreateSign(data);
                var json = JsonConvert.SerializeObject(data);
                result = @"<script src='//res.wx.qq.com/open/js/jweixin-1.0.0.js' type='text/javascript'></script>
                        <script>
                        if(/\/$/.test(location.href))
                            location.href = location.href + 'default.aspx';
                        var r = " + json + @";
                        wx.config({
                            debug: " + debug + @",
                            appId: r.appId,
                            timestamp: r.timestamp,
                            nonceStr: r.nonceStr,
                            signature: r.signature,
                            jsApiList: [
                                'checkJsApi',
                                'onMenuShareTimeline',
                                'onMenuShareAppMessage',
                                'onMenuShareQQ',
                                'chooseImage',
                                'previewImage',
                                'uploadImage',
                                'downloadImage',
                                'getNetworkType',
                                'openLocation',
                                'getLocation',
                                'closeWindow',
                                'chooseWXPay',
                                'getLocalImgData'
                              ]
                        });
                        </script>";
            }
            catch
            {
                result = "";
            }
            return result;
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

        public static string CreateSign(ConfigData data)
        {
            string str1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", GetShopJSTicket(), data.nonceStr, data.timestamp, data.url);
            return EncryptUtil.Sha1Encrypt(str1);
        }

        public class ConfigData
        {
            public string appId { get; set; }
            public string nonceStr { get; set; }
            public long timestamp { get; set; }
            public string signature { get; set; }
            public string url { get; set; }
        }
    }
}
