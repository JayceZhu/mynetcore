using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace PubService
{
    public class WxPayUtil
    {
        //获取32随机串
        public static string GetNoncestr()
        {
            return Guid.NewGuid().ToString("N");
        }
        //获取时间戳
        public static long GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        //设置参数签名
        public static string SetMD5Sign(System.Collections.Hashtable data, string privateKey)
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(data.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)data[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + privateKey);

            string sign = EncryptUtil.MD5Encrypt(sb.ToString()).ToUpper();
            data["sign"] = sign;

            return sign;
        }
        public static string GetXMLString(System.Collections.Hashtable data)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            //加入一个根元素
            XmlElement xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);

            XmlNode root = xmldoc.SelectSingleNode("root");
            foreach (string k in data.Keys)
            {
                XmlElement xe1 = xmldoc.CreateElement(k);
                xe1.InnerText = (string)data[k];
                root.AppendChild(xe1);
            }
            return xmldoc.InnerXml;
        }


        public static Hashtable ParseXML(Stream stream)
        {
            Hashtable hash = new Hashtable();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.XmlResolver = null;
            xmlDoc.Load(stream);
            XmlNode root = xmlDoc.SelectSingleNode("xml");
            XmlNodeList xnl = root.ChildNodes;

            foreach (XmlNode xnf in xnl)
            {
                hash[xnf.Name] = xnf.InnerText;
            }
            return hash;
        }
        public static Hashtable GetResponseHash(string requestName, string requestUrl, Hashtable paramHash)
        {
            WebClient web = new WebClient();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string xmlStr = WxPayUtil.GetXMLString(paramHash);
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(xmlStr);
            request.ContentLength = postdata.Length;

            Stream newStream = request.GetRequestStream();

            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Hashtable responseHash = WxPayUtil.ParseXML(response.GetResponseStream());

            //记录微信接口调用日志
            LogUtil.Log("wxpay",requestName,"url====>\n" + requestUrl + "post====>\n" + JsonConvert.SerializeObject(paramHash) +"res====>\n" + JsonConvert.SerializeObject(responseHash));
            return responseHash;
        }

        public static bool CheckSign(Hashtable hash, string privateKey)
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(hash.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)hash[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + privateKey);

            string sign = EncryptUtil.MD5Encrypt(sb.ToString()).ToUpper();
            return sign.Equals(hash["sign"]);
        }
    }
}