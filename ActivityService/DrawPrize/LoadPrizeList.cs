using Command;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.DrawPrize
{
    public class LoadPrizeListParameter : OAuthListParameter
    {
        public string Kind { get; set; }
    }
    public class LoadPrizeListCommand : Command<List<PrizeInfo>>
    {
        protected override CommandResult<List<PrizeInfo>> OnExecute(object commandParameter)
        {
            var param = commandParameter as LoadPrizeListParameter;
            var result = new CommandResult<List<PrizeInfo>>();
            result.Data = new List<PrizeInfo>();
            using (CoreContext context = new CoreContext())
            {
                var act = context.ActivityInfo.Where(a => a.StartTime <= DateTime.Now && a.EndTime >= DateTime.Now && a.Status == 1 && a.Kind == param.Kind).FirstOrDefault();
                if (act == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }
                var zlopenid = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                result.Data = context.PrizeInfo.Where(p => p.MemberAccount == zlopenid && p.ActId == act.Recid)
                    .OrderByDescending(p => p.Recid)
                    .Skip(param.PageSize * (param.PageIndex - 1))
                    .Take(param.PageSize)
                    .ToList();
            }

            return result;
        }
    }
}
