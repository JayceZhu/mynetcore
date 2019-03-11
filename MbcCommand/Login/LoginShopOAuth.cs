using Model.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Command;
using Newtonsoft.Json;
using PubService;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace MbcService
{

    public class LoginShopOAuthParamter
    {
        public string OAuthAppId { get; set; }
        public string OAuthAppSecret { get; set; }
        public string InterfaceUrl { get; set; }

        public string MemberCode { get; set; }
    }
    public class LoginShopOAuthReulst
    {
        public string Token { get; set; }
    }
    public class LoginShopOAuthCommand : Command<LoginShopOAuthReulst>
    {
        protected override CommandResult<LoginShopOAuthReulst> OnExecute(object commandParameter)
        {
            var paramter = commandParameter as LoginShopOAuthParamter;
            var result = new CommandResult<LoginShopOAuthReulst>
            {
                Data = new LoginShopOAuthReulst()
            };
            if (paramter == null)
            {
                return ErrorResult<LoginShopOAuthReulst>.ParameterError;
            }

            try
            {
                string responseString = "";
                #region 获取用户信息
                string appid = paramter.OAuthAppId;
                string appsecrect = paramter.OAuthAppSecret;
                string interFaceUri = paramter.InterfaceUrl;
                string Key = "Pub.GetMemberToken";
                var param = new
                {
                    AuthCode = paramter.MemberCode,
                    UserCode = appid
                };
                string data = $"key={ Key}&param={JsonConvert.SerializeObject(param)}&private={ appsecrect}";
                string sign = EncryptUtil.MD5Encrypt(data).ToLower();

                using (var http = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip }))
                {
                    var paramdata = System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(param));

                    Serilog.Log.Logger.Information($"#{Key}input#{interFaceUri}?key={Key}&param={paramdata}&user={appid}&sign={sign}");
                    LogUtil.LogText("LoginShopOuathCommand", "Pub.GetMemberToken", $"#{Key}input#{interFaceUri}?key={Key}&param={paramdata}&user={appid}&sign={sign}");
                    http.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true };

                    responseString = http.GetStringAsync($"{interFaceUri}?key={Key}&param={paramdata}&user={appid}&sign={sign}").Result;

                    LogUtil.LogText("LoginShopOuathCommand", "Pub.GetMemberToken", $"#{Key}output{responseString}");
                }
                #endregion

                //创建用户
                JObject resJobj = JsonConvert.DeserializeObject<JObject>(responseString);
                if (!resJobj["Error"].Value<bool>() && resJobj["Result"]["Token"] != null)
                {
                    string Token = resJobj["Result"]["Token"].Value<string>();
                    if (!string.IsNullOrEmpty(Token))
                    {
                        using (CoreContext context = new CoreContext())
                        {
                            MemberInfo _member = (from m in context.MemberInfo where m.ZlOpenId == Token select m).FirstOrDefault();
                            if (_member == null)
                            {
                                _member = new MemberInfo
                                {
                                    NickName = resJobj["Result"]["Name"].Value<string>(),
                                    MemberName = resJobj["Result"]["Name"].Value<string>(),
                                    PhotoUrl = resJobj["Result"]["Photo"].Value<string>(),
                                    Sex = resJobj["Result"]["Sex"].Value<string>(),
                                    ZlOpenId = Token,
                                    AccountId = Guid.NewGuid().ToString("N"),
                                    OwnerAccount = "admin",
                                    DeptCode = "0001",
                                    AddDate = DateTime.Now,
                                    LastLog = DateTime.Now,
                                    LogTimes = 1,
                                    Status = "1"
                                };
                                context.MemberInfo.Add(_member);

                            }
                            var count = context.Database.ExecuteSqlCommand($"update member_ouath_code set status=-9 where member_account={_member.AccountId}");
                            var token = Guid.NewGuid().ToString("N");
                            context.Add(new MemberOuathCode()
                            {
                                MemberAccount = _member.AccountId,
                                OuathCode = token,
                                CreateTime = DateTime.Now,
                                Status = 1
                            });
                            context.SaveChanges();
                            result.Data.Token = token;
                            //  Log.Information($"#LoginShopOuathCommand#OuathCode#{token}#MemberAccount#{_member.AccountId}");
                            LogUtil.LogText("LoginShopOuathCommand", "LoginShopOuathCommand", $"#OuathCode#{token}#MemberAccount#{_member.AccountId}");
                        }
                    }
                    

                    //var identity = new ClaimsIdentity(MemberAuthorizeFilter.Scheme);
                    //identity.AddClaim(new Claim(ClaimTypes.Sid, _member.AccountId));
                    //HttpContext.SignInAsync(MemberAuthorizeFilter.Scheme, new ClaimsPrincipal(identity));
                    //return Redirect(WebUtility.UrlDecode(redirect));
                }
                else
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = responseString;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"#{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}#{ex.Message}");
                result.ErrorCode = -1;
                result.ErrorMessage = ex.Message;
                return result;

            }

            return result;
        }
    }
}
