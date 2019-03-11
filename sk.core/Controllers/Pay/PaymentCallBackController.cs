using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model.Data;
using Newtonsoft.Json;
using OrderService;
using PubService;
using Serilog;

namespace sk.core.Controllers.Pay
{
    public class PaymentCallBackController : Controller
    {
        private CoreContext _context;
        private ILogger logger;
        public PaymentCallBackController(CoreContext memberCartContext)
        {
            _context = memberCartContext;
            logger = Log.Logger;
        }
        [HttpPost, HttpGet]
        public IActionResult WeiXinPayCallBack()
        {
            Hashtable requestHash = WxPayUtil.ParseXML(Request.Body);
            string payno = requestHash["out_trade_no"] as string;

            LogUtil.Log("wxpay", payno, JsonConvert.SerializeObject(requestHash));

            PaymentConfig payConfig = null;
            PayOrder payOrder = (from o in _context.PayOrder where o.OrderNo == payno select o).FirstOrDefault();
            payConfig = (from p in _context.PaymentConfig where p.Status == "1" && p.Id == payOrder.PaymentId select p).FirstOrDefault();
            if (payOrder == null || payConfig == null)
            {
                return Content("fail");
            }

            if (!WxPayUtil.CheckSign(requestHash, payConfig.PrivateKey))
            {
                logger.Error("Weixinpay:验证签名失败" + payno);
                return Content("fail");
            }

            //获取交易状态
            string returnCode = requestHash["return_code"] as string;
            string resultCode = requestHash["result_code"] as string;
            //状态正常的时候才能调用PayOrder接口
            if (returnCode == "SUCCESS" && resultCode == "SUCCESS")
            {
                string tradeNo = requestHash["transaction_id"] as string;
                decimal paymentFee = Convert.ToDecimal(requestHash["total_fee"]) / 100;

                var payRes = new PayOrderCommand().Execute(new PayOrderParamter()
                {
                    PaymentPrice = paymentFee,
                    Order = payOrder,
                    TradeNo = tradeNo
                });

                if (payRes.ErrorCode != 0)
                {
                    logger.Error(payRes.ErrorMessage);
                    return Content("fail");
                }
                else
                {
                    return Content("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");

                }
            }
            else
            {
                return Content("fail");
            }
        }

        public IActionResult TestPayCallBack(string payno, decimal fee)
        {
            //if (string.IsNullOrEmpty(payno))
            //{
            //    return Content("pay error payno is null");
            //}
            //PayOrder order = _context.PayOrder.Where(p => p.OrderNo == payno).FirstOrDefault();
            //if (order.Status != 0)
            //{
            //    logger.Information($"#Weixinpay#支付失败,paymentNo{payno },该订单已支付");
            //    return Content($"{payno}该订单已支付");
            //}
            //var payConfig = (from p in _context.PaymentConfig where p.Status == "1" && p.Id == order.PaymentId select p).FirstOrDefault();
            //if (payConfig == null)
            //{
            //    logger.Information($"#Weixinpay#支付失败,paymentNo{payno },获取支付方式失败");
            //    return Content("获取支付方式失败");
            //}
            //var payRes = new PayOrderCommand().Execute(new PayOrderParamter()
            //{
            //    PaymentPrice = fee,
            //    Order = order,
            //    TradeNo = string.Format("tp_{0:yyyyMMddHHmmss}", DateTime.Now)
            //});

            //if (payRes.ErrorCode != 0)
            //{
            //    logger.Error(payRes.ErrorMessage);
            //    return Content("fail");
            //}
            return Content(JsonConvert.SerializeObject(new Command.CommandResult<int>() { Data = 1 }));
        }
    }
}