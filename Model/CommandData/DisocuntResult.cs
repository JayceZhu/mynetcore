using System;
using System.Collections.Generic;
using System.Text;

namespace Model.CommandData
{
    /// <summary>
    /// 折扣结果
    /// </summary>
    public class DisocuntResult
    {
        public IList<OrderProduct> ProductList { get; set; }

        /// <summary>
        /// 1=满足，0=不满足,9=没有优惠
        /// </summary>
        public int Status { get; set; }

        public string Title { get; set; }

        public string Memo { get; set; }

        public decimal DiscountFee { get; set; }

        public IList<OrderProduct> Gift { get; set; }

        public int Priority { get; set; }

        public string Config { get; set; }

        public string Result { get; set; }

        public int RuleId { get; set; }
    }
}
