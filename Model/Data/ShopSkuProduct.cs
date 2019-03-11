using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class ShopSkuProduct
    {
        public int Recid { get; set; }
        public string ProductNo { get; set; }
        public int Storage { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string ProductSkuCode { get; set; }
        public string InteriorCode { get; set; }
        public string Status { get; set; }
        public string ProductSkuTitle { get; set; }
        public string ParamCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string IsShow { get; set; }
        public string OwnerAccount { get; set; }
        public string DeptCode { get; set; }
        public string ShortDesc { get; set; }
        public string ProductImg { get; set; }
        public string Description { get; set; }
        public string ParamContent { get; set; }
        public string ThumbnailImg { get; set; }
        public string ProductSkuName { get; set; }
    }
}
