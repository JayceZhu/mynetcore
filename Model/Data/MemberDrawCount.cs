using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class MemberDrawCount
    {
        public int Recid { get; set; }
        public string MemberAccount { get; set; }
        public int? ActId { get; set; }
        public int? Counter { get; set; }
        public int? CurrentCount { get; set; }
    }
}
