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
    public class LoadAddressListParameter : MemberParameter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class LoadAddressListCommand : Command<List<AddressData>>
    {
        protected override CommandResult<List<AddressData>> OnExecute(object commandParameter)
        {
            var param = commandParameter as LoadAddressListParameter;
            var result = new CommandResult<List<AddressData>>();
            using (CoreContext context = new CoreContext())
            {
                var acc = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                param.MemberAccount = acc;

                var res = ZlanAPICaller.ExecuteShop("Member.LoadAddressList", param);
                result.ErrorCode = res["ErrorCode"].Value<int>();
                result.ErrorMessage = res["ErrorMsg"].Value<string>();
                if (res["Result"]["List"] != null)
                {
                    result.Data = res["Result"]["List"].ToObject<List<AddressData>>();
                }
            }
            return result;
        }
    }
}
