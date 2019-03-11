using System;
using System.Collections.Generic;
using System.Text;

namespace Model.CommandData
{
    public class ProductData
    {
        //商品图片路径
        public string ThumbnailImg { get; set; }
        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductNo { get; set; }
        //简介
        public string ShortDesc { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 产品库存，SKU库存
        /// </summary>
        public int Storage { get; set; }
        /// <summary>
        /// 已售数量
        /// </summary>
        public int SellCount { get; set; }

        /// <summary>
        /// 状态 1=》上架，0=》下架
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 产品多图
        /// </summary>
        public string ProductImage { get; set; }

        public string CustomFiled { get; set; }

        public IList<ProductSkuData> SkuList { get; set; }

        public string Description { get; set; }

        public int? Sort { get; set; }

        public int CouponId { get; set; }

    }

    public class ProductSkuData
    {
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }
        public int Storage { get; set; }

        public string ProductSkuNo { get; set; }

        public int Status { get; set; }

        public string ParamCode { get; set; }

        public decimal ProductFee { get; set; }

        public decimal DiscountFee { get; set; }

        public string SkuTitle { get; set; }
    }
}
