using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Pin
{
    public class LoadPinGroupParameter : ListParameter
    {
        public string ProductNo { get; set; }
    }
    public class LoadPinGroupCommand : Command<List<Dictionary<string, object>>>
    {
        protected override CommandResult<List<Dictionary<string, object>>> OnExecute(object commandParameter)
        {
            var result = new CommandResult<List<Dictionary<string, object>>>();
            var param = commandParameter as LoadPinGroupParameter;
            result.Data = new List<Dictionary<string, object>>();
            using (CoreContext context = new CoreContext())
            {
                var pinconfig = context.PinConfig.Where(p => p.Config.Contains(param.ProductNo)).FirstOrDefault();

                var pininfo = context.PinInfo.FromSql($@"select p.* from pin_info p left join pin_order o on p.MEMBER_ACCOUNT=o.MEMBER_ACCOUNT and o.MAIN_ID=p.RECID
                            where o.`STATUS`=1 and p.PING_ID={pinconfig.PingId} and p.END_DATE > now() and p.status!=9
                            ORDER BY
                                p.RECID
                             limit {(param.PageIndex - 1) * param.PageSize},{param.PageSize}")
                                 .Select(p => new
                                 {
                                     p.EndDate,
                                     p.MinCount,
                                     p.Recid,
                                     p.PingId,
                                     Counter = 0,
                                     p.MemberAccount
                                 })
                             .ToList();


                foreach (var p in pininfo)
                {
                    //if (context.PinOrder.Where(o => o.MainId == p.Recid && o.Status == 1 && o.MemberAccount == p.MemberAccount).Count() > 0)
                    {
                        var dic = new Dictionary<string, object>();
                        var member = context.MemberInfo.Where(m => m.AccountId == p.MemberAccount).Select(m => new { m.PhotoUrl, m.NickName }).FirstOrDefault();

                        dic["MemberName"] = member.NickName;
                        dic["PhotoUrl"] = member.PhotoUrl;
                        dic["EndDate"] = p.EndDate;
                        dic["Recid"] = p.Recid;
                        dic["PingId"] = p.PingId;
                        dic["MinCount"] = p.MinCount;
                        dic["Counter"] = context.PinOrder.Where(o => o.MainId == p.Recid && o.Status == 1).Count();

                        result.Data.Add(dic);
                    }

                }
            }
            return result;
        }
    }
}
