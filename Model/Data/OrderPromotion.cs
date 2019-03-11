using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class OrderPromotion
    {
        public int Recid { get; set; }
        public string OrderNo { get; set; }
        public int? RuleId { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        public string Config { get; set; }
        public string Reulst { get; set; }
        public string MemberAccount { get; set; }
        public DateTime? RecordTime { get; set; }
        public decimal? DiscountFee { get; set; }
        public int? RecordNum { get; set; }
    }
}
