using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PubService.WeiXinUtil;

namespace PubService
{
    public class GetScriptSignatureParameter
    {
        /// <summary>
        /// 当前URL
        /// </summary>
        public string Url { get; set; }
    }

    public class GetScriptSignatureCommand : Command<ConfigData>
    {
        protected override CommandResult<ConfigData> OnExecute(object commandParameter)
        {
            var param = commandParameter as GetScriptSignatureParameter;
            var result = new CommandResult<ConfigData>();

            result.Data = new ConfigData
            {
                appId = GetShopAppid(),
                nonceStr = CreateNonceStr(),
                timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                url = param.Url
            };
            result.Data.signature = CreateSign(result.Data);

            return result;
        }
    }
}
