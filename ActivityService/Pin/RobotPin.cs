using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Pin
{
    public class RobotPinCommand : Command<int>
    {
        protected override CommandResult<int> OnExecute(object commandParameter)
        {
            var result = new CommandResult<int>();
            using (CoreContext context = new CoreContext())
            {
                //还有半小时结束的拼团
                var pinInfo = context.PinInfo.Where(p => p.Status == 1 && p.EndDate <= DateTime.Now.AddMinutes(-30)).ToList();

                foreach (var item in pinInfo)
                {
                    var pinOrder = context.PinOrder.Where(p => p.MemberAccount == item.MemberAccount && p.Status == 1 && p.MainId == item.Recid).FirstOrDefault();
                    if (pinOrder != null)
                    {
                        var redisdb = RedisClient.GetDatabase();
                        var orderIndex = redisdb.HashIncrementAsync("zlan.membercart.index", "OrderIndex").Result;

                        string mainNo = string.Format("{0:yyMMddHHmmss}{1:D6}", DateTime.Now, orderIndex);
                        //订单号
                        string orderNo = string.Format("AT{0}00", mainNo);

                        var r = new Random();
                        string acc = $"test{r.Next(1, 30)}";
                        context.PinOrder.Add(new PinOrder()
                        {
                            Status = 1,
                            ProductConfig = item.Config,
                            CreateDate = DateTime.Now,
                            MainId = item.Recid,
                            MemberAccount = acc,
                            OrderNo = orderNo

                        });
                        //设置拼团成功
                        //context.Database.ExecuteSqlCommand("update pin_info set status=9 where recid=@p0 and status!=9 ", item.Recid);


                        context.SaveChanges();

                        new SetPinInfoSuccessCommand().Execute(new SetPinInfoSuccessParameter { OrderNo = new List<string>() { pinOrder.OrderNo }, MainId = item.Recid });
                    }

                }
            }

            return result;
        }
    }
}
