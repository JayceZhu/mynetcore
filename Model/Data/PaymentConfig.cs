using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class PaymentConfig
    {
        public int Id { get; set; }
        public string PaymentName { get; set; }
        public string InterfaceId { get; set; }
        public string SellerEmail { get; set; }
        public string AppId { get; set; }
        public string UserId { get; set; }
        public string PrivateKey { get; set; }
        public string SupportCoins { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public string Status { get; set; }
        public string OwnerAccount { get; set; }
        public string Kind { get; set; }
        public string PayType { get; set; }
        public string PayUrl { get; set; }
        public string QueryOrder { get; set; }
        public string CreateRefund { get; set; }
        public string QueryRefund { get; set; }
        public string SynchroCommand { get; set; }
    }
}
