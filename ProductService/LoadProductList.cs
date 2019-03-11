using Command;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace ProductService
{
    public class LoadProductListParameter : ListParameter
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        public string TypeId { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderFiled { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string OrderMethod { get; set; }
    }
    //public class LoadProductListResult
    //{
    //    public IList<ProductInfo> List { get; set; }
    //}
    public class LoadProductListComomand : Command<IList<ProductInfo>>
    {
        protected override CommandResult<IList<ProductInfo>> OnExecute(object commandParameter)
        {
            var result = new CommandResult<IList<ProductInfo>>();
            var param = commandParameter as LoadProductListParameter;

            using (CoreContext context = new CoreContext())
            {
                string strWhere = " 1=1 ";
                if (!string.IsNullOrEmpty(param.Keywords))
                {
                    strWhere += $" &&(ProductName.Contains(\"{param.Keywords}\") || ProductDesc.Contains(\"{param.Keywords}\"))";
                }
                if (!string.IsNullOrEmpty(param.TypeId))
                {
                    strWhere += $" && TypePrefix.Contains(\"_{param.TypeId}_\")";
                }
                string strOrder = " SaleCount desc,Id desc";
                if (!string.IsNullOrEmpty(param.OrderFiled))
                {
                    strOrder = $"{param.OrderFiled} {param.OrderMethod}";
                }
                result.Data = context.ProductInfo.Where(strWhere).OrderBy(strOrder).Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList();
            }
            return result;
        }
    }
}
