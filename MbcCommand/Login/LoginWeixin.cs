using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace MbcService
{
    public class LoginWeixinCommand : LoginOauthCommand
    {
        protected override string GetAccount(CoreContext coreContext, string openeId)
        {
            return coreContext.MemberInfo.Where(m => m.WxOpenId == openeId).Select(m => m.AccountId).FirstOrDefault();
        }

        protected override LoginOAuthData GetOAuthResult(CoreContext coreContext, IDictionary<string, object> parameter)
        {
            LoginOAuthData data = new LoginOAuthData();
            if (!string.IsNullOrEmpty(parameter["code"] as string))
            {
                WxConfig config = coreContext.WxConfig.Where(w => w.OwnerAccount == "wx" && w.Status == "1").FirstOrDefault();
                string appId = config.AppId;
                string appSecretd = config.AppSecret;
                var weixinAccessToken = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessTokenAsync(appId, appSecretd, parameter["code"] as string).Result;
                //WeixinAccessToken weixinAccessToken = GetAccessToken(appId, appSecretd, parameter["code"] as string);
                if (weixinAccessToken.errcode == 0)
                {
                    data.OpenId = weixinAccessToken.openid;
                    data.UnionId = weixinAccessToken.unionid;

                    var member = coreContext.MemberInfo.Where(m => m.WxUnionid == data.UnionId).Select(m => new { m.AccountId, m.NickName, m.PhotoUrl, m.Sex, m.WxUnionid }).FirstOrDefault();
                    if (member != null && !string.IsNullOrEmpty(member.AccountId))
                    {
                        data.NickName = member.NickName;
                        data.PhotoUrl = member.PhotoUrl;
                        data.UnionId = member.WxUnionid;
                        data.Sex = member.Sex;

                        coreContext.Database.ExecuteSqlCommand($"update member_info set wx_open_id={data.OpenId} where wx_unionid={data.UnionId}");
                    }
                    else
                    {
                        if (parameter["state"] as string == "snsapi_userinfo")
                        {
                            OAuthUserInfo user = OAuthApi.GetUserInfoAsync(weixinAccessToken.access_token, weixinAccessToken.openid).Result;
                            data.NickName = ConvertString(user.nickname);
                            data.PhotoUrl = user.headimgurl;
                            data.Sex = string.Format("{0}", user.sex);
                        }
                    }
                    
                }
            }
            else
            {
                data.ErrorCode = 1;
                data.ErrorMessage = "授权失败";
            }

            return data;
        }

        protected string ConvertString(string data)
        {
            StringBuilder buidler = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                char codePoint = data[i];
                if ((codePoint == 0x0) ||
                (codePoint == 0x9) ||
                (codePoint == 0xA) ||
                (codePoint == 0xD) ||
                ((codePoint >= 0x20) && (codePoint <= 0xD7FF)) ||
                ((codePoint >= 0xE000) && (codePoint <= 0xFFFD)))
                {
                    buidler.Append(codePoint);
                }
                else //奇怪的字符
                {
                    buidler.Append("");
                }
            }

            return buidler.ToString();

        }
    }
}
