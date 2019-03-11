using Command;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.ActiveMq;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ActivityService.DrawPrize
{

    public class NewYearDrawPrizeParameter : MemberParameter
    {
        public string Kind { get; set; }
    }
    public class NewYearDrawPrizeCommand : Command<PrizeModel>
    {
        protected override CommandResult<PrizeModel> OnExecute(object commandParameter)
        {
            var result = new CommandResult<PrizeModel>();
            var param = commandParameter as NewYearDrawPrizeParameter;
            result.Data = new PrizeModel();
            using (CoreContext context = new CoreContext())
            {
                var act = context.ActivityInfo.Where(a => a.StartTime <= DateTime.Now && a.EndTime >= DateTime.Now && a.Status == 1 && a.Kind == param.Kind).FirstOrDefault();
                if (act == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }

                Hashtable pconfig = JsonConvert.DeserializeObject<Hashtable>(act.ProductConfig);

                var zlopenid = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();

                //扣减次数，使用数据库锁
                if (context.Database.ExecuteSqlCommand(@"update member_draw_count set current_count=(current_count-1) 
                                                        where member_account=@p0 and act_id=@p1 and current_count-1>=0", zlopenid, act.Recid) > 0)
                {

                    PrizeModel prize = null;

                    var LogCount = context.ActivityLog.Where(l => l.MemberAccount == zlopenid && l.ActivityId == act.Recid).Count();

                    {
                        var connect = ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
                        decimal payFee = 0;
                        using (MySqlConnection con = new MySqlConnection(connect))
                        {
                            con.Open();
                            MySqlCommand com1 = new MySqlCommand("select sum(payment_fee) as PayFee from shop_order_info " +
                                "where member_account=?acc and pay_status=1 and order_no like '%00' and date(payment_time)>='2019-01-16' and date(payment_time)<='2019-01-25' ", con);
                            com1.Parameters.Add(new MySqlParameter("?acc", zlopenid));
                            var Reader1 = com1.ExecuteReader();
                            while (Reader1.Read())
                            {
                                if (Reader1["PayFee"] != DBNull.Value)
                                {
                                    payFee = Convert.ToDecimal(Reader1["PayFee"]);
                                }
                            }
                            Reader1.Close();
                            con.Close();

                        }
                        var plist = new List<PrizeModel>();
                        //遍历奖品规则
                        var redisdb = RedisClient.GetDatabase(2);
                        foreach (var item in (pconfig["PayPrize"] as JArray).ToObject<List<Hashtable>>())
                        {
                            JObject condiction = item["Condition"] as JObject;
                            if (payFee >= condiction["Fee"].Value<decimal>())
                            {
                                var _plist = new List<PrizeModel>();
                                foreach (var hash in (item["Prize"] as JArray).ToObject<List<Hashtable>>())
                                {
                                    if (!hash.ContainsKey("end") || DateTime.Parse(hash["start"] as string) <= DateTime.Now && DateTime.Parse(hash["end"] as string) >= DateTime.Now)
                                    {
                                        _plist.Add(new PrizeModel()
                                        {
                                            PrizeCode = hash["PrizeCode"] as string,
                                            PrizeName = hash["PrizeName"] as string,
                                            Kind = hash["Kind"] as string,
                                            Ratio = Convert.ToInt32(hash["Ratio"]),
                                            CouponId = hash.ContainsKey("CouponId")? Convert.ToInt32(hash["CouponId"]):0
                                        });
                                    }
                                }
                                if (_plist.Count > 0)
                                {
                                    var p = GetPrize(_plist);
                                    if (p != null)
                                    {
                                        string key = p.PrizeCode.IndexOf("prize") > -1 ? "RealPrize" : p.PrizeCode;
                                        //判断中奖次数
                                        if (condiction["Limit"] != null && ValidateCount(redisdb, key, param.MemberAccount, Convert.ToInt32(condiction["Limit"])))
                                        {
                                            continue;
                                        }
                                        //处理实物奖品
                                        if (p.Kind == "RealPrize")
                                        {
                                            string RealPrize = "";
                                            RealPrize = redisdb.ListLeftPop("Prize:" + p.PrizeCode);
                                            if (!string.IsNullOrEmpty(RealPrize))
                                            {
                                                prize = JsonConvert.DeserializeObject<PrizeModel>(RealPrize);
                                            }
                                            else
                                            {
                                                continue;//实物不中继续往下抽奖
                                            }
                                        }
                                        else
                                        {
                                            prize = p;
                                        }

                                        break;
                                    }

                                }
                            }
                        }
                    }
                    if (prize != null)
                    {
                        result.Data = prize;
                        var p = new PrizeInfo()
                        {
                            Status = 1,
                            ActId = act.Recid,
                            CouponId = prize.CouponId,
                            CreateDate = DateTime.Now,
                            MemberAccount = zlopenid,
                            PrizeCode = prize.PrizeCode,
                            PrizeName = prize.PrizeName,
                            Kind = prize.Kind,
                        };
                        context.PrizeInfo.Add(p);
                        if (prize.Kind == "Coupon")
                        {
                            //调用获取优惠券
                            var url = ConfigurationUtil.GetSection("CouponUrl").Value as string;
                            //领取抵扣优惠券
                            try
                            {
                                ActiveMQMessagePusher.Push("Command",
                                                     new Dictionary<string, string>
                                                         {
                                        { "caller","Restful"},
                                        { "url",url }
                                                         }, new
                                                         {
                                                             MemberAccount = zlopenid,
                                                             RuleId = Convert.ToInt32(prize.CouponId)
                                                         }
                                                     );
                            }
                            catch (Exception)
                            {
                            }

                        }
                    }
                    else
                    {
                        result.ErrorMessage = "未中奖,换个姿势再试试吧";
                        result.ErrorCode = -22;
                    }


                }
                else
                {
                    result.ErrorCode = -21;
                    result.ErrorMessage = "";
                }
                context.ActivityLog.Add(new ActivityLog()
                {
                    MemberAccount = zlopenid,
                    ActivityId = act.Recid,
                    CreateTime = DateTime.Now,
                    Memo = result.Data != null ? "抽中:" + result.Data.PrizeName : result.ErrorMessage
                });
                context.SaveChanges();
            }

            return result;
        }

        public int GetRandomNumber(int min, int max)
        {
            int rtn = 0;
            Random r = new Random();
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            r = new Random(iSeed);
            rtn = r.Next(min, max + 1);
            return rtn;
        }

        public PrizeModel GetPrize(List<PrizeModel> pList)
        {
            PrizeModel prize = null;
            var Roll = GetRandomNumber(0, 100000);
            int total = 1;
            foreach (var p in pList)
            {
                var ratio = p.Ratio;
                //中奖
                if (Roll >= total && Roll < total + ratio)
                {
                    prize = p;
                    break;
                }
                total += ratio;
            }
            //pList.ForEach(p =>
            //{
            //    Roll -= p.Ratio;
            //    if (string.IsNullOrEmpty(prize.PrizeCode) && Roll <= 0)
            //    {
            //        prize = p;
            //    }
            //});
            return prize;
        }

        public bool ValidateCount(IDatabase redisdb, string key, string hashfield, int value)
        {
            return Convert.ToInt32(redisdb.HashIncrementAsync(key, hashfield).Result) > value;
        }
    }
}
