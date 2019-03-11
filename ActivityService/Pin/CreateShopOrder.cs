using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Pin
{
    public class CreateShopOrderParameter
    {
        public string ZlOpenId { get; set; }

        public string ProductSkuNo { get; set; }

        public string AddressId { get; set; }

        public string Id { get; set; }
    }

    public class CreateShopOrderResult
    {

    }
    public class CreateShopOrderCommand : Command<string>
    {
        protected override CommandResult<string> OnExecute(object commandParameter)
        {
            var param = commandParameter as CreateShopOrderParameter;
            var result = new CommandResult<string>();
            using (CoreContext context = new CoreContext())
            {
                //if (DateTime.Now == DateTime.Parse("2018-10-31 23:30:00"))
                {
                    var acc = param.ZlOpenId;

                    //移除购物车
                    var RemoveProductRes = ZlanAPICaller.ExecuteShop("Member.RemoveProduct", $"{{ 'MemberAccount': '{acc}','SkuCodeList':['{param.ProductSkuNo}'] }}");
                    if (RemoveProductRes["ErrorCode"].Value<string>() == "0000")
                    {
                        //加入购物车
                        var SelectProductRes = ZlanAPICaller.ExecuteShop("Member.SelectProduct", $"{{ 'ProductList':[{{ 'Increment': 0, 'Counter': 1, 'ProductSkuCode':'{param.ProductSkuNo}', 'Remark': ''}}], 'MemberAccount': '{acc}' }}");
                        if (SelectProductRes["ErrorCode"].Value<string>() == "0000")
                        {
                            //确认结算产品
                            var ConfirmSelectProductRes = ZlanAPICaller.ExecuteShop("Member.ConfirmSelectProduct", $"{{ 'MemberAccount': '{acc}','SkuCodeList':['{param.ProductSkuNo}'] }}");
                            //生成订单
                            if (ConfirmSelectProductRes["ErrorCode"].Value<string>() == "0000")
                            {
                                var CouponTicket = new Dictionary<string, IList<string>>();
                                CouponTicket["Coupon"] = new List<string>();
                                CouponTicket["RedPackage"] = new List<string>();
                                var orderParam = new { MemberAccount = acc, param.AddressId, PaymentId = 3, Kind = "shop", CouponTicket, UseSpecialDiscount = 0, VCartDiscount = 0, Score = 0,ActId=param.Id };
                                LogUtil.Log("Pin.CreateShopOrder_CreateOrderRes", param.ProductSkuNo, JsonConvert.SerializeObject(orderParam));
                                var CreateOrderRes = ZlanAPICaller.ExecuteShop("Member.CreateOrder", orderParam);
                                if (CreateOrderRes["ErrorCode"].Value<string>() == "0000")
                                {
                                    result.Data = CreateOrderRes["Result"]["PaymentNo"].Value<string>();// context.Database.ExecuteSqlCommand($"update  pin_order set order_no={CreateOrderRes["Result"]["PaymentNo"].Value<string>()},status=1 where recid={param.Id}");
                                    //book.Status = 1;
                                    //book.ShopOrderNo = CreateOrderRes["Result"]["PaymentNo"].Value<string>();
                                    //book.CouponTicket = res.Data;
                                    //context.SaveChanges();

                                }
                                else
                                {
                                    result.ErrorCode = -1;
                                    result.ErrorMessage = CreateOrderRes["ErrorMsg"].Value<string>();
                                    LogUtil.Log("Pin.CreateShopOrder_CreateOrderRes", param.ProductSkuNo, result.ErrorMessage);
                                }
                            }
                            else
                            {
                                result.ErrorCode = -1;
                                result.ErrorMessage = ConfirmSelectProductRes["ErrorMsg"].Value<string>();
                                LogUtil.Log("Pin.CreateShopOrder_ConfirmSelectProductRes", param.ProductSkuNo, result.ErrorMessage);
                            }
                        }
                        else
                        {
                            result.ErrorCode = -1;
                            result.ErrorMessage = SelectProductRes["ErrorMsg"].Value<string>();
                            LogUtil.Log("Pin.CreateShopOrder_SelectProductRes", param.ProductSkuNo, result.ErrorMessage);
                        }

                    }
                    else
                    {
                        result.ErrorCode = -1;
                        result.ErrorMessage = RemoveProductRes["ErrorMsg"].Value<string>();
                        LogUtil.Log("CreateShopOrder_RemoveProductRes", param.ProductSkuNo, result.ErrorMessage);
                    }


                }

            }
            return result;
        }
    }
}
