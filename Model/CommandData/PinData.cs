using System;
using System.Collections.Generic;
using System.Text;

namespace Model.CommandData
{
    public class PinData
    {
        /// <summary>
        /// 拼团的Id
        /// </summary>
        public int? MainId { get; set; }

        /// <summary>
        /// 状态9为成功，1拼团中，0未生效，-1失败
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 拼团订单
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 产品配置
        /// </summary>
        public string ProductConfig { get; set; }

        /// <summary>
        /// 最小拼团人数
        /// </summary>
        public int? MinCount { get; set; }

        /// <summary>
        /// 最大有效时间
        /// </summary>
        public int? MaxDate { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

    }
}
