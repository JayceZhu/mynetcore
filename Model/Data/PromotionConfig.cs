using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class PromotionConfig
    {
        public int Recid { get; set; }
        public string Priority { get; set; }
        public string Config { get; set; }
        public string Result { get; set; }
        public int? Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
    }
}
