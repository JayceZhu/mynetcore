using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PubService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrderService
{
    public class GetWxPaymentSignParameter : MemberParameter
    {
        public string PayNo { get; set; }

    }
    public class GetWxPaymentSignResult
    {
        public string AppId { get; set; }

        public string Timestamp { get; set; }

        public string NonceStr { get; set; }

        public string Package { get; set; }

        public string SignType { get; set; }

        public string Signature { get; set; }
    }
    public class GetWxPaymentSignCommand : Command<GetWxPaymentSignResult>
    {
        private HttpContext httpContext { get; set; }
        public GetWxPaymentSignCommand(HttpContext http) { httpContext = http; }
        protected override CommandResult<GetWxPaymentSignResult> OnExecute(object commandParameter)
        {
            var result = new CommandResult<GetWxPaymentSignResult>();
            var param = commandParameter as GetWxPaymentSignParameter;

            if (string.IsNullOrEmpty(param.PayNo))
            {
                return ErrorResult<GetWxPaymentSignResult>.ParameterError;
            }
            using (CoreContext _context = new CoreContext())
            {
                string payno = param.PayNo;
                PayOrder payOrder = _context.PayOrder.Where(o => o.OrderNo == payno).FirstOrDefault();
                if (payOrder == null)
                {
                    //Logger.Error("pay error,not found payconfig,payNo:" + payno);
                    LogUtil.Log("wxpay", payno, "pay error,not found payconfig,payNo:" + payno);
                    result.ErrorCode = -1;
                    result.ErrorMessage = "找不到订单";
                    return result;
                }
                if (payOrder.Status != 0)
                {
                    //Log.Logger.Error("pay error,order is pay,payNo:" + payno);
                    LogUtil.Log("wxpay", payno, "pay error,order is pay,payNo:" + payno);
                    result.ErrorCode = 1;
                    result.ErrorMessage = "订单已支付,请刷新页面";
                    return result;
                }
                PaymentConfig paymentConfig = _context.PaymentConfig.Where(c => c.Id == payOrder.PaymentId).FirstOrDefault();
                if (paymentConfig == null)
                {
                    // Log.Logger.Error("pay error,not found payconfig,payNo:" + payno);
                    LogUtil.Log("wxpay", payno, "pay error,not found payconfig,payNo:" + payno);
                    result.ErrorCode = -1;
                    result.ErrorMessage = "支付方式不正确";
                    return result;
                }
                MemberInfo member = _context.MemberInfo.Where(m => m.AccountId == payOrder.MemberAccount).FirstOrDefault();
                ////通过商城授权
                if (string.IsNullOrEmpty(member.ZlOpenId))
                {
                    // Log.Logger.Error("pay error,ouath faild");
                    LogUtil.Log("wxpay", payno, "pay error,ouath faild,openid is null");
                    return ErrorResult<GetWxPaymentSignResult>.NoAuthorization;
                }

                var connectionString = ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
                var openid = member.ZlOpenId;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        string opneidFiled = "wx_open_id";
                        switch (paymentConfig.Kind)
                        {
                            case "XCX":
                                opneidFiled = "xcx_open_id"; break;
                            case "WX": opneidFiled = "wx_open_id"; break;
                            default: break;
                        }
                        conn.Open();
                        string sql = $"select {opneidFiled} from member_info WHERE account_id=?acc LIMIT 1";
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.Add(new MySqlParameter()
                        {
                            ParameterName = "?acc",
                            Value = member.ZlOpenId
                        });

                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            openid = reader[opneidFiled] as string;
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        //Log.Logger.Error("get shop wx_openid fail#ex#" + ex.Message);
                        //LogUtil.Log("GetWxPaymentSignCommand", "GetWxPaymentSignCommand", "GetWxPaymentSignCommand", "get shop wx_openid fail#ex#" + ex.Message);
                        LogUtil.Log("wxpay", payno, ex.Message);
                        return ErrorResult<GetWxPaymentSignResult>.NoLogin;
                    }

                }
                string productName = "";
                string ProductDesc = payOrder.Memo;
                if (ProductDesc.Length > 20)
                {
                    ProductDesc = ProductDesc.Substring(0, 20) + "...";
                }
                string scheme = !StringValues.IsNullOrEmpty(httpContext.Request.Headers["X-Client-Scheme"]) ? httpContext.Request.Headers["X-Client-Scheme"].ToString() : httpContext.Request.Scheme;
                Hashtable paramHash = new Hashtable
                {
                    ["appid"] = paymentConfig.AppId,
                    ["mch_id"] = paymentConfig.UserId,       //商户号
                    ["trade_type"] = "JSAPI",
                    ["nonce_str"] = Guid.NewGuid().ToString("N"),
                    ["openid"] = openid,
                    ["out_trade_no"] = payno,
                    ["total_fee"] = ((int)(payOrder.PayFee * 100)).ToString(),
                    ["notify_url"] = $"{scheme}://{httpContext.Request.Host}" + "/PaymentCallBack/WeiXinPayCallBack",
                    ["body"] = ProductDesc,
                    ["spbill_create_ip"] = httpContext.Connection.LocalIpAddress.ToString(),
                    ["time_start"] = string.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(payOrder.CreateTime)),
                    ["time_expire"] = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now.AddMinutes(10))
                };

                WxPayUtil.SetMD5Sign(paramHash, paymentConfig.PrivateKey);

                Hashtable responseHash = WxPayUtil.GetResponseHash("wxorder_" + payno, "https://api.mch.weixin.qq.com/pay/unifiedorder", paramHash);


                var prepayId = "";
                if (responseHash["return_code"] as string == "SUCCESS" && responseHash["result_code"] as string == "SUCCESS")
                {
                    prepayId = responseHash["prepay_id"] as string;
                }
                //调用接口出错     
                else
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = responseHash["return_msg"] + "=>" + responseHash["err_code_des"];
                    return result;
                }
                //调用接口失败，未知错误
                if (string.IsNullOrEmpty(prepayId))
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "服务器忙";
                    return result;
                }

                result.Data = new GetWxPaymentSignResult()
                {
                    AppId = paymentConfig.AppId,
                    Timestamp = string.Format("{0}", WxPayUtil.GetTimestamp()),
                    NonceStr = WxPayUtil.GetNoncestr(),
                    Package = "prepay_id=" + prepayId,
                    SignType = "MD5"
                };

                Hashtable choosePayParamHash = new Hashtable
                {
                    ["appId"] = result.Data.AppId,
                    ["timeStamp"] = result.Data.Timestamp,
                    ["nonceStr"] = result.Data.NonceStr,
                    ["package"] = result.Data.Package,
                    ["signType"] = result.Data.SignType
                };

                //签名
                result.Data.Signature = WxPayUtil.SetMD5Sign(choosePayParamHash, paymentConfig.PrivateKey);
                LogUtil.Log("wxpay", payno, "ouput=>" + JsonConvert.SerializeObject(result.Data));
            }
            return result;
        }
    }
}
