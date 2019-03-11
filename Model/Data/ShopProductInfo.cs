using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class ShopProductInfo
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductNameTag { get; set; }
        public string ProductNo { get; set; }
        public string InteriorCode { get; set; }
        public int TypeId { get; set; }
        public string Brand { get; set; }
        public string ShortDesc { get; set; }
        public string ShortDescTag { get; set; }
        public string Unit { get; set; }
        public decimal Weight { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public string ThumbnailImg { get; set; }
        public string ProductImg { get; set; }
        public string Description { get; set; }
        public int Storage { get; set; }
        public string CustomField { get; set; }
        public string Kind { get; set; }
        public int MiniNum { get; set; }
        public string IsForSale { get; set; }
        public string IsCommend { get; set; }
        public string IsHot { get; set; }
        public string IsSpecialOffer { get; set; }
        public string IsVirtual { get; set; }
        public DateTime AddTime { get; set; }
        public string IsDel { get; set; }
        public string Status { get; set; }
        public int Sort { get; set; }
        public int Hits { get; set; }
        public string GiftContent { get; set; }
        public string ParamContent { get; set; }
        public string PackContent { get; set; }
        public string OwnerAccount { get; set; }
        public int? SellCount { get; set; }
        public DateTime? GroundingTime { get; set; }
        public string Level { get; set; }
        public int? ExchangeTimes { get; set; }
        public int? KindRuleId { get; set; }
        public DateTime? EndTime { get; set; }
        public string IsLimitTime { get; set; }
        public string PackageProducts { get; set; }
        public int? AvailableCoupon { get; set; }
        public string QrcodeUrl { get; set; }
        public string TagIds { get; set; }
        public string TypePrefix { get; set; }
        public DateTime? StartTime { get; set; }
        public string DeptCode { get; set; }
        public string RemarkConfig { get; set; }
        public int StoreId { get; set; }
        public string SupportNopay { get; set; }
        public string ChatRoomLink { get; set; }
        public int? ShippingTemplate { get; set; }
        public string Keyword { get; set; }
        public string Video { get; set; }
    }
}
