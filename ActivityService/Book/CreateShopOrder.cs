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

namespace ActivityService.Book
{
    public class CreateShopOrderParameter
    {
    }

    public class CreateShopOrderResult
    {

    }
    public class CreateShopOrderCommand : Command<int>
    {
        protected override CommandResult<int> OnExecute(object commandParameter)
        {
            // var param = commandParameter as CreateShopOrderParameter;
            var result = new CommandResult<int>();
            using (CoreContext context = new CoreContext())
            {
                //if (DateTime.Now == DateTime.Parse("2018-10-31 23:30:00"))
                {

                    var bookList = context.BookInfo.FromSql(@"select b.* from book_info b left join pay_order p on p.order_no=b.order_no  where p.`STATUS`=1 and b.`STATUS` !=1 ").ToList();
                    foreach (var book in bookList)
                    {
                        var acc = context.MemberInfo.Where(m => m.AccountId == book.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                        var couponticket = book.CouponTicket;
                        if (string.IsNullOrEmpty(couponticket))
                        {
                            int? couponId = context.BookInfo.Where(b => b.OrderNo == book.OrderNo && b.MemberAccount == book.MemberAccount).Select(b => b.CouponId).FirstOrDefault();
                            var url = ConfigurationUtil.GetSection("CouponUrl").Value as string;
                            //领取抵扣优惠券
                            var res = ZlanAPICaller.Call<CommandResult<string>>(url, new { MemberAccount = acc, RuleId = couponId });
                            if (res.ErrorCode == 0)
                            {
                                context.Database.ExecuteSqlCommand($"update book_info set coupon_ticket={res.Data} where order_no={book.OrderNo}");
                                couponticket = res.Data;
                            }
                            else
                            {
                                result.ErrorCode = res.ErrorCode;
                                result.ErrorMessage = res.ErrorMessage;
                                LogUtil.Log("CreateShopOrder_TakeCoupon", book.OrderNo, res.ErrorMessage);
                            }
                        }

                        {
                            //移除购物车
                            var RemoveProductRes = ZlanAPICaller.ExecuteShop("Member.RemoveProduct", $"{{ 'MemberAccount': '{acc}','SkuCodeList':['{book.ProductSkuNo}'] }}");
                            if (RemoveProductRes["ErrorCode"].Value<string>() == "0000")
                            {
                                //加入购物车
                                var SelectProductRes = ZlanAPICaller.ExecuteShop("Member.SelectProduct", $"{{ 'ProductList':[{{ 'Increment': 0, 'Counter': 1, 'ProductSkuCode':'{book.ProductSkuNo}', 'Remark': ''}}], 'MemberAccount': '{acc}' }}");
                                if (SelectProductRes["ErrorCode"].Value<string>() == "0000")
                                {
                                    //确认结算产品
                                    var ConfirmSelectProductRes = ZlanAPICaller.ExecuteShop("Member.ConfirmSelectProduct", $"{{ 'MemberAccount': '{acc}','SkuCodeList':['{book.ProductSkuNo}'] }}");
                                    //生成订单
                                    if (ConfirmSelectProductRes["ErrorCode"].Value<string>() == "0000")
                                    {
                                        var CouponTicket = new Dictionary<string, IList<string>>();
                                        CouponTicket["Coupon"] = new List<string>() { couponticket };
                                        CouponTicket["RedPackage"] = new List<string>();
                                        var orderParam = new { MemberAccount = acc, book.AddressId, PaymentId = 3, Kind = "shop", CouponTicket, UseSpecialDiscount = 0, VCartDiscount = 0, Score = 0 };
                                        LogUtil.Log("CreateShopOrder_CreateOrderRes", book.OrderNo, JsonConvert.SerializeObject(orderParam));
                                        var CreateOrderRes = ZlanAPICaller.ExecuteShop("Member.CreateOrder", orderParam);
                                        if (CreateOrderRes["ErrorCode"].Value<string>() == "0000")
                                        {
                                            result.Data += context.Database.ExecuteSqlCommand($"update book_info set Status=1,Shop_Order_No={CreateOrderRes["Result"]["PaymentNo"].Value<string>()} where order_no={book.OrderNo}");
                                            //book.Status = 1;
                                            //book.ShopOrderNo = CreateOrderRes["Result"]["PaymentNo"].Value<string>();
                                            //book.CouponTicket = res.Data;
                                            //context.SaveChanges();

                                        }
                                        else
                                        {
                                            result.ErrorCode = -1;
                                            result.ErrorMessage = CreateOrderRes["ErrorMsg"].Value<string>();
                                            LogUtil.Log("CreateShopOrder_CreateOrderRes", book.OrderNo, result.ErrorMessage);
                                        }
                                    }
                                    else
                                    {
                                        result.ErrorCode = -1;
                                        result.ErrorMessage = ConfirmSelectProductRes["ErrorMsg"].Value<string>();
                                        LogUtil.Log("CreateShopOrder_ConfirmSelectProductRes", book.OrderNo, result.ErrorMessage);
                                    }
                                }
                                else
                                {
                                    result.ErrorCode = -1;
                                    result.ErrorMessage = SelectProductRes["ErrorMsg"].Value<string>();
                                    LogUtil.Log("CreateShopOrder_SelectProductRes", book.OrderNo, result.ErrorMessage);
                                }

                            }
                            else
                            {
                                result.ErrorCode = -1;
                                result.ErrorMessage = RemoveProductRes["ErrorMsg"].Value<string>();
                                LogUtil.Log("CreateShopOrder_RemoveProductRes", book.OrderNo, result.ErrorMessage);
                            }

                        }

                    }

                }

            }
            return result;
        }
    }
}
