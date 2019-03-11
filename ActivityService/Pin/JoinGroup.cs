using Command;
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
    public class JoinGroupParameter : MemberParameter
    {
        public int MainId { get; set; }

    }

    public class JoinGroupResult
    {
        public string PinId { get; set; }

        public string ProductSkuNo { get; set; }
    }
    public class JoinGroupCommand : Command<JoinGroupResult>
    {
        protected override CommandResult<JoinGroupResult> OnExecute(object commandParameter)
        {
            var param = commandParameter as JoinGroupParameter;
            var result = new CommandResult<JoinGroupResult>();
            result.Data = new JoinGroupResult();

            using (CoreContext context = new CoreContext())
            {
                var pingInfo = context.PinInfo.Where(p => p.Recid == param.MainId).FirstOrDefault();
                if (pingInfo.Status == 9)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "该团已成功";
                    return result;
                }
                if (pingInfo.MemberAccount == param.MemberAccount)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "不能参与自己发起的团";
                    return result;
                }
                JObject _product = JsonConvert.DeserializeObject<JObject>(pingInfo.Config);

                result.Data.PinId = pingInfo.PingId;
                result.Data.ProductSkuNo = _product["ProductSkuNo"].Value<string>();
                //var acc = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();

            }

            return result;
        }
    }
}
