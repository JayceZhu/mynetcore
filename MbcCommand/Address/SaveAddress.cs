using Command;
using Model.Data;
using Newtonsoft.Json.Linq;
using PubService.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbcService
{
    public class SaveAddressParameter : MemberParameter
    {
        public int AddressId { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Status { get; set; }
    }
    public class SaveAddressCommand : Command<Hashtable>
    {
        protected override CommandResult<Hashtable> OnExecute(object commandParameter)
        {
            var param = commandParameter as SaveAddressParameter;
            var result = new CommandResult<Hashtable>();
            using (CoreContext context = new CoreContext())
            {
                var acc = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                param.MemberAccount = acc;
                var res = ZlanAPICaller.ExecuteShop("Member.SaveAddress", param);

                if (res["ErrorCode"].Value<string>() == "0000" && res["Result"]["Data"] != null)
                {
                    result.Data = res["Result"]["Data"].ToObject<Hashtable>();
                    result.ErrorCode = res["ErrorCode"].Value<int>();
                    result.ErrorMessage = res["ErrorMsg"].Value<string>();
                }
                else
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "创建地址失败,请刷新页面重试";
                }
            }
            return result;
        }
    }
}
