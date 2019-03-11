using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class PaymentLog
    {
        public int Recid { get; set; }
        public int PaymentId { get; set; }
        public string PaymentNo { get; set; }
        public string PaymentAccount { get; set; }
        public string TradeNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string PayStatus { get; set; }
        public DateTime? PayTime { get; set; }
        public decimal? PaymentFee { get; set; }
    }
}
