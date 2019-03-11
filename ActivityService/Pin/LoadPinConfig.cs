using Command;
using Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Pin
{
    public class LoadPinConfigParameter : MemberParameter
    {
        /// <summary>
        /// 拼团活动配置id
        /// </summary>
        public string PinId { get; set; }
        /// <summary>
        /// 参团的id
        /// </summary>
        public int MainId { get; set; }

    }
    public class LoadPinConfigComand : Command<Dictionary<string, object>>
    {
        protected override CommandResult<Dictionary<string, object>> OnExecute(object commandParameter)
        {
            var param = commandParameter as LoadPinConfigParameter;
            var result = new CommandResult<Dictionary<string, object>>();
            result.Data = new Dictionary<string, object>();

            using (CoreContext context = new CoreContext())
            {


                if (param.MainId > 0)
                {
                    var pinlist = context.PinOrder.Where(p => p.MainId == param.MainId && p.Status == 1)
                        .OrderBy(p => p.CreateDate)
                        .Join(context.MemberInfo, p => p.MemberAccount, m => m.AccountId, (p, m) => new
                        {
                            p.Recid,
                            m.PhotoUrl,
                            m.MemberName
                        })
                       .ToList();
                    result.Data["PinInfo"] = pinlist;
                    var pinInfo = context.PinInfo.Where(p => p.Recid == param.MainId && p.Status != 0).FirstOrDefault();
                    if (pinInfo != null)
                    {
                        var pinCofig = context.PinConfig.Where(p => p.PingId == pinInfo.PingId && DateTime.Now >= p.StartDate && p.EndDate >= DateTime.Now).FirstOrDefault();
                        if (pinCofig == null)
                        {
                            result.ErrorCode = -1;
                            result.ErrorMessage = "活动已过期";
                            return result;
                        }
                        result.Data["Config"] = pinCofig;
                        result.Data["IsMain"] = 0;
                        if (pinInfo.MemberAccount == param.MemberAccount)
                        {
                            result.Data["IsMain"] = 1;
                        }
                        result.Data["PinDetail"] = pinInfo;
                        if (pinInfo.Status == 9)
                        {
                            result.Data["OrderNo"] = context.PinOrder.Where(o => o.MemberAccount == param.MemberAccount && o.Status == 1).Select(o => o.Status).FirstOrDefault();
                        }
                    }

                }
                else
                {
                    var pinCofig = context.PinConfig.Where(p => p.PingId == param.PinId && DateTime.Now >= p.StartDate && p.EndDate >= DateTime.Now).FirstOrDefault();
                    if (pinCofig == null)
                    {
                        result.ErrorCode = -1;
                        result.ErrorMessage = "找不到拼团活动";
                        return result;
                    }
                    result.Data["Config"] = pinCofig;
                }
                result.Data["CurrentMember"] = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => new
                {
                    m.MemberName,
                    m.PhotoUrl
                }).FirstOrDefault();
            }

            return result;
        }
    }
}
