using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class AddDrawCountLog
    {
        public int Recid { get; set; }
        public string MemberAccount { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Memo { get; set; }
        public int? Counter { get; set; }
        public string Kind { get; set; }
        public int? ActId { get; set; }
    }
}
