using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class ActivityLog
    {
        public int Recid { get; set; }
        public int? ActivityId { get; set; }
        public string MemberAccount { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Memo { get; set; }
        public string OwnerAccount { get; set; }
    }
}
