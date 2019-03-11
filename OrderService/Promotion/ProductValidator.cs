using Model.CommandData;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace OrderService
{
    public class ProductValidator : IValidator
    {
        public string FilterExpression { get; set; }

        public string Property { get; set; }

        public string Operation { get; set; }

        public object Value { get; set; }

        public void InitData(System.Collections.Hashtable hash)
        {
            FilterExpression = hash["Filter"] as string;
            Operation = hash["Operation"] as string;
            Property = hash["Property"] as string;
            Value = hash["Value"];
        }

        public bool Validate(DiscountParameter parameter, CoreContext context)
        {
            var plist = GetProducts(parameter);
           
            switch (Property)
            {
                case "TotalFee":
                    {
                        var expressionInput = (decimal)plist.Sum(p => p.Counter * p.SalePrice);
                        ParameterExpression paramExp = Expression.Parameter(typeof(decimal));
                        ConstantExpression constantExpression = Expression.Constant(Convert.ToDecimal(Value));
                        return OnValidate<decimal>(paramExp, constantExpression, expressionInput);
                    }
                case "TotalCount":
                    {
                        decimal expressionInput = plist.Sum(p => p.Counter);
                        ParameterExpression paramExp = Expression.Parameter(typeof(decimal));
                        ConstantExpression constantExpression = Expression.Constant(Convert.ToDecimal(Value));
                        return OnValidate<decimal>(paramExp, constantExpression, expressionInput);
                    }

            }
            return false;
        }

        private bool OnValidate<T>(ParameterExpression paramExp, ConstantExpression constantExpression, T expressionInput)
        {

            Expression validateExp = null;
            switch (Operation)
            {
                case "Equals":
                    validateExp = Expression.Equal(paramExp, constantExpression);
                    break;
                case "GreaterThan":
                    validateExp = Expression.GreaterThan(paramExp, constantExpression);
                    break;
                case "GreaterThanOrEqual":
                    validateExp = Expression.GreaterThanOrEqual(paramExp, constantExpression);
                    break;
                case "LessThan":
                    validateExp = Expression.LessThan(paramExp, constantExpression);
                    break;
                case "LessThanOrEqual":
                    validateExp = Expression.LessThanOrEqual(paramExp, constantExpression);
                    break;
            }
            if (validateExp != null)
            {
                return Expression.Lambda<Func<T, bool>>(validateExp, paramExp).Compile()(expressionInput);
            }
            return false;
        }

        public  IList<OrderProduct> GetProducts(DiscountParameter parameter)
        {
            if (string.IsNullOrEmpty(FilterExpression))
            {
                FilterExpression = "p => true";
            }
            var query = parameter.ProductList.AsQueryable();
            return query.Where(FilterExpression).ToList();
        }
    }
}
