using Command;
using Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Book
{
    public class CreateBookOrderParameter : MemberParameter
    {
        public string ProductNo { get; set; }

        public string ProductSkuNo { get; set; }

        public int PayId { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Araea { get; set; }

        public int AddressId { get; set; }

        public string Name
        {
            get; set;
        }
    }

    public class CreateBookOrderResult
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string PayNo { get; set; }
        /// <summary>
        /// 支付Id
        /// </summary>
        public int PayId { get; set; }
    }
    public class CreateBookOrderCommand : Command<CreateBookOrderResult>
    {
        protected override CommandResult<CreateBookOrderResult> OnExecute(object commandParameter)
        {
            var param = commandParameter as CreateBookOrderParameter;
            var result = new CommandResult<CreateBookOrderResult>();
            result.Data = new CreateBookOrderResult();
            using (CoreContext context = new CoreContext())
            {
                var book = new BookUtil();
                if (book._Config == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }
                if (DateTime.Now < book._Config.StartTime)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动未开始";
                    return result;
                }
                if (book._Config.EndTime < DateTime.Now)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }

                var pmodel = book._ProductConfig.Where(p => p.ProductNo == param.ProductNo).FirstOrDefault();
                if (pmodel == null)
                {
                    return ErrorResult<CreateBookOrderResult>.ParameterError;
                }
                var pSku = pmodel.SkuList.Where(s => s.ProductSkuNo == param.ProductSkuNo).FirstOrDefault();
                if (pSku == null)
                {
                    return ErrorResult<CreateBookOrderResult>.ParameterError;
                }

                PaymentConfig payment = (from p in context.PaymentConfig where p.Id == param.PayId select p).FirstOrDefault();

                if (payment == null)
                {
                    result.ErrorMessage = "请设置支付方式";
                    result.ErrorCode = -1;
                    return result;
                }

                var redisdb = RedisClient.GetDatabase();
                var orderIndex = redisdb.HashIncrementAsync("zlan.membercart.index", "OrderIndex").Result;

                string mainNo = string.Format("{0:yyMMddHHmmss}{1:D6}", DateTime.Now, orderIndex);
                //订单号
                string orderNo = string.Format("AT{0}00", mainNo);

                decimal totalFee = 0, discountFee = 0, productFee = 0;
                productFee = pSku.SalePrice;

                //开启事务
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        totalFee = productFee - discountFee;
                        //金额小于等于0创建失败
                        if (totalFee <= 0)
                        {
                            return ErrorResult<CreateBookOrderResult>.ParameterError;
                        }
                        var order = new PayOrder()
                        {
                            OrderNo = orderNo,
                            PaymentId = param.PayId,
                            PayFee = totalFee,
                            MemberAccount = param.MemberAccount,
                            TotalFee = totalFee,
                            CreateTime = DateTime.Now,
                            Kind = "gift",
                            Status = 0,
                            ProductFee = productFee,
                            DiscountFee = discountFee,
                            Memo = $"【{pmodel.ProductName}】预付订单"
                        };
                        context.Add(order);

                        var bookinfo = new BookInfo()
                        {
                            Address = param.Address,
                            City = param.City,
                            Province = param.Province,
                            Area = param.Araea,
                            Phone = param.Phone,
                            Name = param.Name,
                            Counter = 1,
                            ProductNo = pmodel.ProductNo,
                            ProductSkuNo = pSku.ProductSkuNo,
                            Status = 0,
                            OrderNo = orderNo,
                            ConfigId = 1,
                            ProductConfig = JsonConvert.SerializeObject(pmodel),
                            StartTime = book._ValidDate["Start"].Value<DateTime>(),
                            EndTime = book._ValidDate["End"].Value<DateTime>(),
                            MemberAccount = param.MemberAccount,
                            CouponId = pmodel.CouponId,
                            AddressId = param.AddressId == 0 ? param.AddressId = -99 : param.AddressId
                        };
                        context.Add(bookinfo);
                        context.SaveChanges();

                        result.Data.PayId = payment.Id;
                        result.Data.PayNo = orderNo;
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        result = ErrorResult<CreateBookOrderResult>.ParameterError;
                        result.ErrorMessage = ex.Message;
                        return result;
                    }
                 
                }
            }

            return result;
        }
    }
}
