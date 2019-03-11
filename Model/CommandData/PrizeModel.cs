using System;
using System.Collections.Generic;
using System.Text;

namespace Model.CommandData
{
    public class PrizeModel
    {
        public string PrizeCode { get; set; }

        public int CouponId { get; set; }

        public string PrizeName { get; set; }

        public string Kind { get; set; }

        public int Sort { get; set; }

        public int Limit { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Ratio { get; set; }


        public int LimitDate { get; set; }

        public int Count { get; set; }
    }
}
