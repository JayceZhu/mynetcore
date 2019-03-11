using Command;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.ActiveMq;
using PubService.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.DrawPrize
{
    public class DrawPrizeParameter : MemberParameter
    {
        public string Kind { get; set; }
    }
    public class DrawPrizeCommand : Command<Hashtable>
    {
        protected override CommandResult<Hashtable> OnExecute(object commandParameter)
        {
            var result = new CommandResult<Hashtable>();
            var param = commandParameter as DrawPrizeParameter;
            result.Data = new Hashtable();
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
                                                        where member_account=@p0 and current_count-1>=0", zlopenid) > 0)
                {
                    var LogCount = context.ActivityLog.Where(l => l.MemberAccount == zlopenid && l.ActivityId == act.Recid).Count();
                    var res = new NewMemberSkillCommand().Execute(new NewMemberSkillParameter()
                    {
                        MemberAccount = zlopenid,
                        LogCount = LogCount,
                        Prize = pconfig
                    });
                    Hashtable prize = null;
                    if (res.ErrorCode == 0 && (Convert.ToBoolean(res.Data["isNew"]) || Convert.ToBoolean(res.Data["is30"])))
                    {
                        result.ErrorCode = res.ErrorCode;
                        result.ErrorMessage = res.ErrorMessage;
                        prize = res.Data["Prize"] as Hashtable;
                    }
                    else
                    {
                        var connect = ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
                        decimal payFee = 0;
                        using (MySqlConnection con = new MySqlConnection(connect))
                        {
                            con.Open();
                            MySqlCommand com1 = new MySqlCommand("select sum(payment_fee) as PayFee from shop_order_info " +
                                "where member_account=?acc and pay_status=1 and order_no like '%00' and date(payment_time)>='2018-11-01' and date(payment_time)<='2018-11-11' ", con);
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
                        //遍历奖品规则
                        var redisdb = RedisClient.GetDatabase(1);
                        foreach (var item in (pconfig["PayPrize"] as JArray).ToObject<List<Hashtable>>())
                        {
                            JObject condiction = item["Condition"] as JObject;

                            string key = "level:" + condiction["Fee"].Value<string>();
                            if (redisdb.HashExists(key, param.MemberAccount))
                            {
                                continue;
                            }
                            if (payFee >= condiction["Fee"].Value<decimal>())
                            {
                                prize = (item["Prize"] as JObject).ToObject<Hashtable>();
                                //处理实物奖品
                                if (prize["Kind"] as string == "RealPrize")
                                {
                                    //var hash = (prize["RealPrize"] as JObject).ToObject<Hashtable>();
                                    var _realprize = new List<PrizeModel>();
                                    foreach (var hash in (prize["RealPrize"] as JArray).ToObject<List<Hashtable>>())
                                    {
                                        if (DateTime.Parse(hash["start"] as string) <= DateTime.Now && DateTime.Parse(hash["end"] as string) >= DateTime.Now)
                                        {
                                            _realprize.Add(new PrizeModel()
                                            {
                                                PrizeCode = hash["PrizeCode"] as string,
                                                PrizeName = hash["PrizeName"] as string,
                                                Kind = prize["Kind"] as string
                                            });
                                        }
                                    }
                                    string RealPrize = "";

                                    if (_realprize.Count != 0)
                                    {
                                        var r = new Random(Guid.NewGuid().GetHashCode());
                                        var index = r.Next(0, _realprize.Count);
                                        RealPrize = redisdb.ListLeftPop("Prize:" + _realprize[index].PrizeCode);
                                    }
                                    if (!string.IsNullOrEmpty(RealPrize))
                                    {
                                        prize = JsonConvert.DeserializeObject<Hashtable>(RealPrize);
                                    }
                                    else
                                    {
                                        prize = null;
                                    }

                                }
                                if (prize != null)
                                {
                                    redisdb.HashIncrementAsync(key, param.MemberAccount);
                                }
                                break;
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
                            CouponId = Convert.ToInt32(prize["CouponId"]),
                            CreateDate = DateTime.Now,
                            MemberAccount = zlopenid,
                            PrizeCode = prize["PrizeCode"] as string,
                            PrizeName = prize["PrizeName"] as string,
                            Kind = prize["Kind"] as string,
                        };
                        context.PrizeInfo.Add(p);
                        if (prize["Kind"] as string == "Coupon")
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
                                                             RuleId = Convert.ToInt32(prize["CouponId"])
                                                         }
                                                     );
                            }
                            catch (Exception)
                            {
                            }
                            //var couponres = ZlanAPICaller.Call<CommandResult<string>>(url, new { MemberAccount = zlopenid, RuleId = Convert.ToInt32(prize["CouponId"]) });
                            //if (couponres.ErrorCode == 0)
                            //{
                            //    couponticket = couponres.Data;
                            //}
                            //else
                            //{
                            //    LogUtil.Log("DrawPrize_TakeCoupon", p.Recid.ToString(), $"input=>{JsonConvert.SerializeObject(new { MemberAccount = zlopenid, RuleId = Convert.ToInt32(prize["CouponId"]) })}#error=>{res.ErrorMessage}");
                            //}
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
                    result.ErrorMessage = "每下单68元，即获得一次抽奖机会，上不封顶";
                }
                context.ActivityLog.Add(new ActivityLog()
                {
                    MemberAccount = zlopenid,
                    ActivityId = act.Recid,
                    CreateTime = DateTime.Now,
                    Memo = result.Data.Keys.Count > 0 ? "抽中:" + result.Data["PrizeName"] as string : result.ErrorMessage
                });
                context.SaveChanges();
            }

            return result;
        }

        public override void AfterExecute(object commandParameter, CommandResult<Hashtable> commandResult)
        {

        }
    }
}
