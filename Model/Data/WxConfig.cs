using System;
using System.Collections.Generic;

namespace Model.Data
{
    public partial class WxConfig
    {
        public int Recid { get; set; }
        public string ServerDomain { get; set; }
        public string WebsiteName { get; set; }
        public string WxAccount { get; set; }
        public string WxAccountKind { get; set; }
        public string LogoUrl { get; set; }
        public string InterfaceUrl { get; set; }
        public string InterfaceToken { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string Status { get; set; }
        public string OauthEnable { get; set; }
        public string AccessTokenValue { get; set; }
        public DateTime? AccessTokenUpdateTime { get; set; }
        public string JsTicketValue { get; set; }
        public DateTime? JsTicketUpdateTime { get; set; }
        public string OwnerAccount { get; set; }
    }
}
