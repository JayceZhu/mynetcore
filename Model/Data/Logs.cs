using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class Logs
    {
        public int Id { get; set; }
        public string Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        public string Logger { get; set; }
        public string ModuleName { get; set; }
    }
}
