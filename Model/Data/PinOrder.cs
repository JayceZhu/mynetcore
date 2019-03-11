using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class PinOrder
    {
        public int Recid { get; set; }
        public int? MainId { get; set; }
        public string ProductConfig { get; set; }
        public string MemberAccount { get; set; }
        public DateTime? CreateDate { get; set; }
        public string OrderNo { get; set; }
        public int? Status { get; set; }
    }
}
