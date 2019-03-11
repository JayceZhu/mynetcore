using Command;
using Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ActivityService.Pin
{
    public class ComputePromotionParameter : MemberParameter
    {
        public string ActId { get; set; }

        public string ProductSkuNo { get; set; }

    }
    public class ComputePromotionCommand : Command<List<Hashtable>>
    {
        protected override CommandResult<List<Hashtable>> OnExecute(object commandParameter)
        {
            var result = new CommandResult<List<Hashtable>>();
            var param = commandParameter as ComputePromotionParameter;
            var res = new LoadPinConfigComand().Execute(new LoadPinConfigParameter() { PinId = param.ActId });
            if (res.ErrorCode == 0)
            {
                var configArry = JsonConvert.DeserializeObject<JArray>((res.Data["Config"] as PinConfig).Config);
                Hashtable hash = new Hashtable
                {
                    //["ProductNo"] = config["ProductNo"].Value<string>(),
                    //["SalePrice"] = config["SalePrice"].Value<decimal>(),
                    //["ProductSkuNo"] = config["ProductSkuNo"].Value<decimal>(),
                    ["Counter"] = 1,
                    ["DeptCode"] = "0001",
                    ["TypePrefix"] = "-1",
                    ["TypeId"] = "-1"
                };
                foreach (var item in configArry)
                {
                    if (item["ProductSkuNo"].Value<string>() == param.ProductSkuNo)
                    {
                        hash["ProductNo"] = item["ProductNo"].Value<string>();
                        hash["SalePrice"] = item["SalePrice"].Value<decimal>();
                        hash["ProductSkuNo"] = item["ProductSkuNo"].Value<string>();
                        break;
                    }
                }
                var url = ConfigurationUtil.GetSection("PromotionUrl").Value as string; ;

                var _data = new { ActId = param.ActId, ProductList = new List<Hashtable>() { hash } };
                var respromotion = ZlanAPICaller.Call<CommandResult<List<Hashtable>>>(url, _data);
                if (respromotion.ErrorCode == 0)
                {
                    result.Data = respromotion.Data;
                }

            }
            return result;
        }
    }
}
