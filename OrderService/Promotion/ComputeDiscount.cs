using Command;
using Model.CommandData;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace OrderService
{
    public class DiscountParameter : MemberParameter
    {
        public IList<OrderProduct> ProductList { get; set; }

        public int PageCounter { get; set; }

    }

    public class ComputeDiscountCommand : Command<IList<DisocuntResult>>
    {
        protected override CommandResult<IList<DisocuntResult>> OnExecute(object commandParameter)
        {
            var param = commandParameter as DiscountParameter;
            var result = new CommandResult<IList<DisocuntResult>>();
            result.Data = new List<DisocuntResult>();
            var rawProductList = param.ProductList.ToList();
            if (param.PageCounter == 0)
            {
                param.PageCounter = 1;
            }
            using (CoreContext context = new CoreContext())
            {
                List<PromotionConfig> ruleList = context.PromotionConfig.Where(r => r.StartTime <= DateTime.Now && r.EndTime >= DateTime.Now && r.Status == 1).OrderByDescending(r => r.Priority).ToList();
                var _discountRule = new List<DiscountRule>();
                IDictionary<DiscountRule, DisocuntResult> ruleDic = new Dictionary<DiscountRule, DisocuntResult>();

                foreach (var rule in ruleList)
                {
                    _discountRule.Add(new DiscountRule(rule));
                }

                foreach (var drule in _discountRule)
                {
                    var pList = drule.GetProducts(param);
                    if (pList.Count > 0)
                    {
                        var discountResult = JsonConvert.DeserializeObject<JObject>(drule.RuleModel.Result);
                        var dRes = new DisocuntResult()
                        {
                            Status = 0,
                            ProductList = pList,
                            Title = drule.RuleModel.Title,
                            Memo = discountResult["Memo"].Value<string>(),
                            DiscountFee = discountResult["DiscountFee"].Value<decimal>() * param.PageCounter,
                            Config = drule.RuleModel.Config,
                            Result = drule.RuleModel.Result,
                            RuleId = drule.RuleModel.Recid
                        };
                        foreach (OrderProduct p in pList)
                        {
                            //去除已划分优惠的产品
                            rawProductList.Remove(p);
                        }
                        result.Data.Add(dRes);
                        ruleDic[drule] = dRes;
                    }
                }

                //划分优惠状态
                foreach (var item in ruleDic.Keys.OrderByDescending(r => r.RuleModel.Priority))
                {
                    if (item.Validate(param, context))
                    {
                        ruleDic[item].Status = 1;
                        break;
                    }
                }

                if (rawProductList.Count > 0)
                {
                    var promotionResult = new DisocuntResult();

                    //优惠为空！
                    promotionResult.Status = -9;
                    promotionResult.ProductList = rawProductList;
                    result.Data.Add(promotionResult);
                }
            }
            return result;
        }
    }
}
