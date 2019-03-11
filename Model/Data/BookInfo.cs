using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class BookInfo
    {
        public int Recid { get; set; }
        public string ProductNo { get; set; }
        public string ProductSkuNo { get; set; }
        public int? Counter { get; set; }
        public int? ConfigId { get; set; }
        public string ProductConfig { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string OrderNo { get; set; }
        public string CouponTicket { get; set; }
        public int? Status { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string MemberAccount { get; set; }
        public int? CouponId { get; set; }
        public int? AddressId { get; set; }
        public string ShopOrderNo { get; set; }
    }
}
