using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class ProductInfo
    {
        public int Recid { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Status { get; set; }
        public int? TypeId { get; set; }
        public string TypePrefix { get; set; }
        public int? SaleCount { get; set; }
        public string ImageUrl { get; set; }
        public int? Sort { get; set; }
    }
}
