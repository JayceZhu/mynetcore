using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class PrizeInfo
    {
        public int Recid { get; set; }
        public string PrizeName { get; set; }
        public string PrizeCode { get; set; }
        public string MemberAccount { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Kind { get; set; }
        public int? CouponId { get; set; }
        public string WinnerInfo { get; set; }
        public int? ActId { get; set; }
        public DateTime? UseDate { get; set; }
    }
}
