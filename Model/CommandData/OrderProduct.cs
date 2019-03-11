using System;
using System.Collections.Generic;
using System.Text;

namespace Model.CommandData
{
    public class OrderProduct : ProductData
    {
        /// <summary>
        /// sku json数据
        /// </summary>
        public string SkuParamCode { get; set; }

        /// <summary>
        /// 对应sku编码
        /// </summary>
        public string ProductSkuNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Counter { get; set; }
    }
}
