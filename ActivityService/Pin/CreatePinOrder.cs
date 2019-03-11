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
using System.Web;

namespace ActivityService.Pin
{
    public class CreatePinParameter : MemberParameter
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public string PinId { get; set; }

        /// <summary>
        /// 参团Id
        /// </summary>
        public int MainId { get; set; }

        public string AddressId { get; set; }

        public string ProductSkuNo { get; set; }
    }
    public class CreatePinOrderCommad : Command<string>
    {
        protected override CommandResult<string> OnExecute(object commandParameter)
        {
            var result = new CommandResult<string>();
            var param = commandParameter as CreatePinParameter;
            using (CoreContext context = new CoreContext())
            {
                //使用zlopenid，因为此时没有登录
                var ZlOpenId = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                var pinCofig = context.PinConfig.Where(p => p.PingId == param.PinId && DateTime.Now >= p.StartDate && p.EndDate >= DateTime.Now).FirstOrDefault();
                if (pinCofig == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "找不到拼团活动";
                    return result;
                }

                var configArry = JsonConvert.DeserializeObject<JArray>(pinCofig.Config);
                string config = "";
                foreach (var item in configArry)
                {
                    if (item["ProductSkuNo"].Value<string>() == param.ProductSkuNo)
                    {
                        config = JsonConvert.SerializeObject(item);
                        break;
                    }
                }
                using (var tran = context.Database.BeginTransaction())
                {
                    PinInfo pinInfo = null;
                    int pinCount = 0;
                    if (param.MainId > 0)
                    {
                        pinInfo = context.PinInfo.Where(p => p.Recid == param.MainId && p.PingId == param.PinId && p.EndDate >= DateTime.Now).FirstOrDefault();
                        if (pinInfo != null && pinInfo.MemberAccount == param.MemberAccount)
                        {
                            result.ErrorCode = -1;
                            result.ErrorMessage = "不能参与自己发起的团";
                            return result;
                        }
                        pinCount = context.PinOrder.Where(p => p.MainId == param.MainId && p.Status == 1).Count();
                        if (pinCount == 0)
                        {
                            result.ErrorCode = -1;
                            result.ErrorMessage = "参数错误";
                            return result;
                        }
                    }
                    if (pinInfo == null)
                    {
                        var endDate = DateTime.Now.AddDays(Convert.ToInt32(pinCofig.MaxDate));
                        if (DateTime.Now.ToString("yyyy-MM-dd") == Convert.ToDateTime(pinCofig.EndDate).ToString("yyyy-MM-dd"))
                        {
                            endDate = Convert.ToDateTime(pinCofig.EndDate);
                        }
                        pinInfo = new PinInfo()
                        {
                            MaxDate = pinCofig.MaxDate,
                            PingId = pinCofig.PingId,
                            Status = 1,
                            Config = config,
                            CreateDate = DateTime.Now,
                            EndDate = endDate,
                            MemberAccount = param.MemberAccount,
                            MinCount = pinCofig.MinCount
                        };
                        context.PinInfo.Add(pinInfo);
                        context.SaveChanges();
                    }
                    if (pinInfo.Status == 9)
                    {
                        result.ErrorCode = -1;
                        result.ErrorMessage = "拼团失败，该团已满人";
                        return result;
                    }
                    if (pinInfo.Status == -1)
                    {
                        result.ErrorCode = -1;
                        result.ErrorMessage = "拼团失败，该团已失效";
                        return result;
                    }
                    if (pinInfo.EndDate < DateTime.Now)
                    {
                        result.ErrorCode = -1;
                        result.ErrorMessage = "拼团失败,该团已失效";
                        return result;
                    }


                    try
                    {
                        var pinOrder = new PinOrder()
                        {
                            Status = 0,
                            ProductConfig = pinInfo.Config,
                            CreateDate = DateTime.Now,
                            MainId = pinInfo.Recid,
                            MemberAccount = param.MemberAccount
                        };
                        context.PinOrder.Add(pinOrder);
                        //创建商城订单
                        JObject productconfig = JsonConvert.DeserializeObject<JObject>(pinInfo.Config);
                        var res = new CreateShopOrderCommand().Execute(new CreateShopOrderParameter()
                        {
                            ZlOpenId = ZlOpenId,
                            ProductSkuNo = productconfig["ProductSkuNo"].Value<string>(),
                            AddressId = param.AddressId,
                            Id = pinCofig.PingId
                        });
                        if (!string.IsNullOrEmpty(res.Data))
                        {
                            pinOrder.OrderNo = res.Data;
                        }
                        else
                        {
                            result.ErrorCode = -1;
                            result.ErrorMessage = res.ErrorMessage;
                            return result;
                        }

                        context.SaveChanges();
                        result.Data = pinOrder.OrderNo;

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {

                        tran.Rollback();
                        result = ErrorResult<string>.ParameterError;
                        result.ErrorMessage = ex.Message;
                        return result;
                    }

                    //未成团锁定商城订单状态
                    var confirmRes = ZlanAPICaller.ExecuteSys("Sys.ChangeOrderConfirm", new
                    {
                        OrderNo = new List<string> {
                            result.Data
                        },
                        Type = "AWAIT"
                    });
                    if (!confirmRes["ErrorCode"].Value<string>().Equals("0000"))
                    {
                        LogUtil.Log("CreatePinOrder", result.Data, confirmRes["ErrorMsg"].Value<string>());

                        result.ErrorCode = -1;
                        result.ErrorMessage = "锁定商城订单状态失败";
                        return result;
                    }

                }
            }

            return result;
        }
    }
}