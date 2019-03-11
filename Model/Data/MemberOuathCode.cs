using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class MemberOuathCode
    {
        public int Recid { get; set; }
        public string MemberAccount { get; set; }
        public string OuathCode { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? Status { get; set; }
    }
}
