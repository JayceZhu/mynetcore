using Model.CommandData;
using Model.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OrderService
{
    public class DiscountRule
    {
        public PromotionConfig RuleModel { get; set; }
        public DiscountRule()
        {

        }
        public DiscountRule(PromotionConfig rule)
        {
            RuleModel = rule;
            ConditionValidator = new ProductValidator();
            Hashtable hash = JsonConvert.DeserializeObject<Hashtable>(rule.Config);
            ConditionValidator.InitData(hash);
        }

        public IList<OrderProduct> GetProducts(DiscountParameter parameter)
        {
            ProductValidator productValidator = ConditionValidator as ProductValidator;
            return productValidator.GetProducts(parameter);
        }

        protected IValidator ConditionValidator { get; set; }

        public bool Validate(DiscountParameter parameter, CoreContext context)
        {
            return ConditionValidator.Validate(parameter, context);
        }
    }
}
