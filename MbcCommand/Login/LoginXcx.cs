using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MbcService
{
    public class LoginXcxCommand : LoginOauthCommand
    {
        protected override string GetAccount(CoreContext coreContext, string openeId)
        {
            return coreContext.MemberInfo.Where(m => m.XcxOpenId == openeId).Select(m => m.AccountId).FirstOrDefault();
        }

        protected override LoginOAuthData GetOAuthResult(CoreContext coreContext, IDictionary<string, object> parameter)
        {
            LoginOAuthData data = new LoginOAuthData();

            var Code = parameter["code"] as string;
            if (string.IsNullOrEmpty(Code))
            {
                data.ErrorCode = 1;
                data.ErrorMessage = "授权失败";
                return data;
            }
            var Endata = parameter["EncryptedData"] as string;
            var IV = parameter["IV"] as string;
            var Appid = parameter["Appid"] as string;
            WxConfig config = coreContext.WxConfig.Where(w => w.OwnerAccount == "xcx" && w.Status == "1").FirstOrDefault();
            string appId = config.AppId;
            string appSecretd = config.AppSecret;
            JObject keyData = GetKey(appId, appSecretd, Code);
            if (!string.IsNullOrEmpty(keyData["openid"].Value<string>()))
            {
                JObject user = JsonConvert.DeserializeObject<JObject>(new AESCUtil(keyData["session_key"].Value<string>(), IV.Replace(" ", "+")).AESDecrypt(Endata.Replace(" ", "+")));
                data.NickName = ConvertString(user["nickName"].Value<string>());
                data.OpenId = user["openId"].Value<string>();
                data.PhotoUrl = user["avatarUrl"].Value<string>();
                data.UnionId = user["unionId"].Value<string>();
                data.Sex = user["gender"].Value<string>();

                var member = coreContext.MemberInfo.Where(m => m.WxUnionid == data.UnionId).Select(m => new { m.AccountId, m.NickName, m.PhotoUrl, m.Sex, m.WxUnionid }).FirstOrDefault();
                if (member != null && !string.IsNullOrEmpty(member.AccountId))
                {
                    data.NickName = member.NickName;
                    data.PhotoUrl = member.PhotoUrl;
                    data.UnionId = member.WxUnionid;
                    data.Sex = member.Sex;

                    coreContext.Database.ExecuteSqlCommand($"update member_info set xcx_open_id={data.OpenId} where wx_unionid={data.UnionId}");
                }
            }
            else
            {
                data.ErrorCode = keyData["errcode"].Value<int>();
                data.ErrorMessage = keyData["errmsg"].Value<string>();
            }

            return data;
        }

        protected JObject GetKey(string appid, string appsecretd, string code)
        {
            string access_token_url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + appsecretd + "&js_code=" + code + "&grant_type=authorization_code";

            WebClient web = new WebClient();

            Stream stream = web.OpenRead(access_token_url);
            StreamReader reader = new StreamReader(stream);
            string response = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<JObject>(response);
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
