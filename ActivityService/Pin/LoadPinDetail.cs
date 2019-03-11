using Command;
using Model.CommandData;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ActivityService.Pin
{
    public class LoadPinDetailParameter : MemberParameter
    {
        public string OrderNo { get; set; }
    }

    public class LoadPinDetailResult
    {
        public PinData PinData { get; set; }

        public Dictionary<string, object> PinMember { get; set; }
    }

    public class LoadPinDetailCommand : Command<LoadPinDetailResult>
    {
        protected override CommandResult<LoadPinDetailResult> OnExecute(object commandParameter)
        {
            var result = new CommandResult<LoadPinDetailResult>();
            var param = commandParameter as LoadPinDetailParameter;
            result.Data = new LoadPinDetailResult();
            result.Data.PinData = new PinData();
            result.Data.PinMember = new Dictionary<string, object>();
            using (CoreContext context = new CoreContext())
            {
                var acc = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => new { m.AccountId, m.PhotoUrl, m.Id }).FirstOrDefault();
                result.Data.PinData = context.PinOrder.Where(o => o.MemberAccount == acc.AccountId && o.OrderNo == param.OrderNo && o.Status == 1)
                     .Join(context.PinInfo, o => o.MainId, p => p.Recid, (o, p) => new PinData
                     {
                         MainId = o.MainId,
                         Status = p.Status,
                         CreateDate = p.CreateDate,
                         MaxDate = p.MaxDate,
                         MinCount = p.MinCount,
                         OrderNo = o.OrderNo,
                         ProductConfig = p.Config,
                         EndTime = p.EndDate

                     }).FirstOrDefault();
                if (result.Data.PinData != null)
                {
                    result.Data.PinMember["List"] = context.PinOrder.Where(p => p.MainId == result.Data.PinData.MainId && p.Status == 1)
                       .OrderBy(p => p.CreateDate)
                       .Join(context.MemberInfo, p => p.MemberAccount, m => m.AccountId, (p, m) => new
                       {
                           p.Recid,
                           m.PhotoUrl,
                           m.MemberName
                       })
                      .ToList();

                    result.Data.PinMember["Current"] = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => new
                    {
                        m.PhotoUrl,
                        m.MemberName
                    }).FirstOrDefault();
                }

            }
            return result;
        }
    }
}