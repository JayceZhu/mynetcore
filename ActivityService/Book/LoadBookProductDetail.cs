using Command;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Book
{
    /// <summary>
    /// 输入参数
    /// </summary>
    public class LoadBookProductDetailParameter
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public int ActId { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductNo { get; set; }
    }

    public class LoadBookProductDetailResult
    {

    }

    public class LoadBookProductDetailCommand : Command<ProductData>
    {
        protected override CommandResult<ProductData> OnExecute(object commandParameter)
        {
            var result = new CommandResult<ProductData>();
            var param = commandParameter as LoadBookProductDetailParameter;

            using (CoreContext context = new CoreContext())
            {

                var bookConfig = new BookUtil();
                if (bookConfig._Config == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }
                if (DateTime.Now < bookConfig._Config.StartTime)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动未开始";
                    return result;
                }
                if (bookConfig._Config.EndTime < DateTime.Now)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }

                var pmodel = bookConfig._ProductConfig.Where(p => p.ProductNo == param.ProductNo).FirstOrDefault();
                if (pmodel == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "产品对象为空";
                    return result;
                }
                var productInfo = context.ProductInfo.Where(p => p.ProductNo == pmodel.ProductNo).FirstOrDefault();
                var pinfo = context.Set<ShopProductInfo>().FromSql($@"select * from shop_product_info where product_no ={pmodel.ProductNo}").FirstOrDefault();
                if (pinfo != null)
                {
                    pmodel.ProductImage = pinfo.ProductImg;
                    pmodel.Description = pinfo.Description;
                    pmodel.ThumbnailImg = pinfo.ThumbnailImg;
                }
                if (productInfo != null)
                {
                    if (!string.IsNullOrEmpty(productInfo.ProductName))
                    {
                        pmodel.ProductName = productInfo.ProductName;
                    }
                    if (!string.IsNullOrEmpty(productInfo.ProductDesc))
                    {
                        pmodel.ShortDesc = productInfo.ProductDesc;
                    }
                    if (!string.IsNullOrEmpty(productInfo.ImageUrl))
                    {
                        pmodel.ThumbnailImg = productInfo.ImageUrl;
                    }

                }

                result.Data = pmodel;
            }

            return result;
        }
    }
}
