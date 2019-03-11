using Command;
using Model.CommandData;
using Model.Data;
using Newtonsoft.Json.Linq;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubService.Command
{
    public class LoadAddressParameter
    {
        /// <summary>
        ///键值对,形如 Province:val1;City:val2;Area:val3;
        /// </summary>
        public string KeyValuePairString { get; set; }

        /// <summary>
        /// 分组/Province/City/Area
        /// </summary>
        public string Category { get; set; }
    }
    public class LoadAddressCommand : Command<IList<ParamKeyValuePair>>
    {
        protected override CommandResult<IList<ParamKeyValuePair>> OnExecute(object commandParameter)
        {
            var param = commandParameter as LoadAddressParameter;
            var result = new CommandResult<IList<ParamKeyValuePair>>();

            var res = ZlanAPICaller.ExecuteShop("Pub.LoadAddress", param);
            result.ErrorCode = res["ErrorCode"].Value<int>();
            result.ErrorMessage = res["ErrorMsg"].Value<string>();
            if (res["Result"]["List"] != null)
            {
                result.Data = res["Result"]["List"].ToObject<IList<ParamKeyValuePair>>();
            }
            return result;
        }
    }
}
