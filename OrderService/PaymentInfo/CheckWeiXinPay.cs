using Command;
using Model.Data;
using PubService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace OrderService
{
    public class CheckWeiXinPayParameter
    {
        public string PayNo { get; set; }
    }

    public class CheckWeiXinPayCommand : Command<int>
    {
        protected override CommandResult<int> OnExecute(object commandParameter)
        {
            var param = commandParameter as CheckWeiXinPayParameter;
            var result = new CommandResult<int>();
            result.Data = 0;
            using (CoreContext context = new CoreContext())
            {
                var payno = param.PayNo;
                PayOrder order = context.PayOrder.Where(o => o.OrderNo == payno).FirstOrDefault();
                if (order != null)
                {
                    if (order.Status == 1)
                    {
                        result.Data = 1;
                        return result;
                    }
                    PaymentConfig payment = context.PaymentConfig.Where(p => p.Id == order.PaymentId).FirstOrDefault();
                    Hashtable webParam = new Hashtable
                    {
                        ["out_trade_no"] = payno,
                        ["nonce_str"] = Guid.NewGuid().ToString("N"),
                        ["appid"] = payment.AppId,
                        ["mch_id"] = payment.UserId
                    };

                    WxPayUtil.SetMD5Sign(webParam, payment.PrivateKey);

                    string body = WxPayUtil.GetXMLString(webParam);

                    string posturl = "https://api.mch.weixin.qq.com/pay/orderquery";
                    WebClient web = new WebClient();
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";

                    byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(body);
                    request.ContentLength = postdata.Length;

                    Stream newStream = request.GetRequestStream();

                    newStream.Write(postdata, 0, postdata.Length);
                    newStream.Close();

                    HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                    //StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                    Hashtable hash = WxPayUtil.ParseXML(myResponse.GetResponseStream());
                    if (hash["return_code"] as string == "SUCCESS" && hash["result_code"] as string == "SUCCESS" && hash["trade_state"] as string == "SUCCESS")
                    {
                        var payRes = new PayOrderCommand().Execute(new PayOrderParamter()
                        {
                            PaymentPrice = Convert.ToDecimal(hash["total_fee"]) / 100,
                            Order = order,
                            TradeNo = hash["transaction_id"] as string
                        });
                        if (payRes.Data.PayOrder.Status == 1)
                        {
                            result.Data = 1;
                        }
                    }
                    //记录同步操作
                    Serilog.Log.Logger.Information("wxpay_synchro", posturl,
                        "post====>\n" + Newtonsoft.Json.JsonConvert.SerializeObject(webParam) +
                        "res====>\n" + Newtonsoft.Json.JsonConvert.SerializeObject(hash));
                }
            }
            return result;
        }
    }
}
