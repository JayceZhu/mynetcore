using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class ActivityInfo
    {
        public int Recid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string JoinConfig { get; set; }
        public string ProductConfig { get; set; }
        public string Kind { get; set; }
        public int? Status { get; set; }
        public string OwnerAccount { get; set; }
        public string DeptCode { get; set; }
    }
}
