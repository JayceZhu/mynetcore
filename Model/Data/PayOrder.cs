using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class PayOrder
    {
        public int Recid { get; set; }
        public string OrderNo { get; set; }
        public int PaymentId { get; set; }
        public decimal? PayFee { get; set; }
        public decimal? TotalFee { get; set; }
        public string Memo { get; set; }
        public string MemberAccount { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? PayTime { get; set; }
        public string Kind { get; set; }
        public int? Status { get; set; }
        public decimal? ProductFee { get; set; }
        public decimal? DiscountFee { get; set; }
        public decimal? ShippingFee { get; set; }
    }
}
