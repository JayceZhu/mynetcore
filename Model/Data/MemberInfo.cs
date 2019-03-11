using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class MemberInfo
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public string NickName { get; set; }
        public string MemberName { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? Birthday { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Qq { get; set; }
        public string Icq { get; set; }
        public string Msn { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime LastLog { get; set; }
        public int LogTimes { get; set; }
        public string PhotoUrl { get; set; }
        public string Introduction { get; set; }
        public float? AdvancePayment { get; set; }
        public int Score { get; set; }
        public decimal? Cashbox { get; set; }
        public int Coins { get; set; }
        public int Level { get; set; }
        public string Introducer { get; set; }
        public string SellPassword { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string IdentityType { get; set; }
        public string IdentityNo { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string LoginId { get; set; }
        public string WxOpenId { get; set; }
        public string OldWxOpenId { get; set; }
        public string WxUnionid { get; set; }
        public string QqOpenId { get; set; }
        public string SinaOpenId { get; set; }
        public string OwnerAccount { get; set; }
        public string DeptCode { get; set; }
        public string ZlOpenId { get; set; }
        public string CardTag { get; set; }
        public string XcxOpenId { get; set; }
    }
}
