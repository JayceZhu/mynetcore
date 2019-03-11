using Command;
using Model.CommandData;
using Model.Data;
using Newtonsoft.Json.Linq;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbcService
{
    public class LoadAddressDetailParameter : MemberParameter
    {
        public int AddressId { get; set; }
    }
    public class LoadAddressDetailCommand : Command<AddressData>
    {
        protected override CommandResult<AddressData> OnExecute(object commandParameter)
        {
            var param = commandParameter as LoadAddressDetailParameter;
            var result = new CommandResult<AddressData>();
            using (CoreContext context = new CoreContext())
            {
                var acc = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                param.MemberAccount = acc;

                var res = ZlanAPICaller.ExecuteShop("Member.LoadAddressDetail", param);
                result.ErrorCode = res["ErrorCode"].Value<int>();
                result.ErrorMessage = res["ErrorMsg"].Value<string>();
                if (res["Result"]["Data"] != null)
                {
                    result.Data = res["Result"]["Data"].ToObject<AddressData>();
                }
            }
            return result;
        }
    }
}
