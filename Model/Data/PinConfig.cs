using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class PinConfig
    {
        public int Recid { get; set; }
        public string PingId { get; set; }
        public int? MinCount { get; set; }
        public int? MaxDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
        public string Config { get; set; }
    }
}
