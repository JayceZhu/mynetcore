
using Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace OrderService
{
    public class PayOrderParamter
    {
        public decimal PaymentPrice { get; set; }

        public string TradeNo { get; set; }

        public PayOrder Order { get; set; }
    }
    public class PayOrderResult
    {
        public PayOrder PayOrder { get; set; }
    }
    public class PayOrderCommand : Command<PayOrderResult>
    {
        protected override CommandResult<PayOrderResult> OnExecute(object commandParameter)
        {
            var param = commandParameter as PayOrderParamter;
            var result = new CommandResult<PayOrderResult>();
            PaymentConfig payConfig = null;
            using (CoreContext db = new CoreContext())
            {

                PayOrder payOrder = param.Order;

                if (payOrder == null)
                {
                    Serilog.Log.Logger.Error($"#{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}#order payment not fount {payOrder.OrderNo}");
                    result.ErrorCode = -1;
                    result.ErrorMessage = "order payment not fount";
                    return result;
                }
                if (payOrder.PayFee != param.PaymentPrice)
                {
                    Serilog.Log.Logger.Error($"#{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}#Weixinpaynotify#支付失败,paymentNo ={payOrder.OrderNo}," +
                        $" tradeNo ={param.TradeNo},totalprice={param.PaymentPrice}");
                    result.ErrorCode = -1;
                    result.ErrorMessage = $"requestfee-{param.PaymentPrice} and checkfee-{payOrder.PayFee} not match ";
                    return result;
                }
                payConfig = db.PaymentConfig.Where(p => p.Id == payOrder.PaymentId && p.Status == "1").FirstOrDefault();

                int updateRes = db.Database.ExecuteSqlCommand($@"update pay_order set status=1 where order_no={payOrder.OrderNo} and status !=1 ");
                if (updateRes == 1 && payConfig != null)
                {
                    // payOrder.Status = 1;

                    db.PaymentLog.Add(new PaymentLog()
                    {
                        PayStatus = "1",
                        CreateDate = DateTime.Now,
                        PaymentAccount = payOrder.MemberAccount,
                        PaymentFee = payOrder.PayFee,
                        PaymentId = payConfig.Id,
                        PaymentNo = payOrder.OrderNo,
                        PayTime = DateTime.Now,
                        TradeNo = param.TradeNo
                    });
                    db.SaveChanges();
                    Serilog.Log.Logger.Error($"#{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}#Weixinpaynotify#支付成功,paymentNo ={payOrder.OrderNo}," +
                        $" tradeNo ={param.TradeNo},totalprice={param.PaymentPrice}");

                    result.Data = new PayOrderResult
                    {
                        PayOrder = payOrder
                    };
                    result.Data.PayOrder.Status = 1;
                }


            }
            return result;
        }

        public override void AfterExecute(object commandParameter, CommandResult<PayOrderResult> result)
        {
            var param = commandParameter as PayOrderParamter;
            //判断已支付
            if (result.ErrorCode == 0 && result.Data.PayOrder.Status == 1)
            {
                //var url = "";
                //string acc = "";
                //using (CoreContext context = new CoreContext())
                //{
                //    acc = context.MemberInfo.Where(m => m.AccountId == result.Data.PayOrder.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                //    int? couponId = context.BookInfo.Where(b => b.OrderNo == result.Data.PayOrder.OrderNo && b.MemberAccount == result.Data.PayOrder.MemberAccount).Select(b => b.CouponId).FirstOrDefault();
                //    var res = ZlanAPICaller.Call<CommandResult<string>>(url, new { MemberAccount = acc, CouponId = couponId });
                //    if (res.ErrorCode == 0)
                //    {
                //        context.Database.ExecuteSqlCommand($"update book_info set coupon_ticket={res.Data} where order_no={result.Data.PayOrder.OrderNo}");
                //    }

                //}
            }

        }
    }
}