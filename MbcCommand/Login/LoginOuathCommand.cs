using System;
using System.Collections.Generic;
using System.Text;
using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using Newtonsoft.Json;

namespace MbcService
{
    public class LoginOAuthParameter : LoginBaseParameter
    {
        public IDictionary<string, object> LoginData { get; set; }
    }
    public class LoginOuathResult : LoginBaseResult
    {
        public LoginOAuthData OAuthResult { get; set; }
    }
    public abstract class LoginOauthCommand : LoginBaseCommand<LoginOuathResult>
    {
        protected override CommandResult<LoginOuathResult> Login(CoreContext coreContext, LoginBaseParameter parameter)
        {
            CommandResult<LoginOuathResult> result = new CommandResult<LoginOuathResult>();
            var param = parameter as LoginOAuthParameter;
            // using (CoreContext coreContext = new CoreContext())
            {
                result.Data.OAuthResult = GetOAuthResult(coreContext, param.LoginData);
                if (result.Data.OAuthResult.ErrorCode != 0 || string.IsNullOrEmpty(result.Data.OAuthResult.OpenId))
                {
                    return ErrorResult<LoginOuathResult>.NoFundOpenId;
                }

                //下载图片,把photourl设置成json格式
                if (!string.IsNullOrEmpty(result.Data.OAuthResult.PhotoUrl))
                {
                    SimpleFileInfo fileinfo = new SimpleFileInfo() { FileId = -2 };
                    fileinfo.FilePath = result.Data.OAuthResult.PhotoUrl;
                    result.Data.OAuthResult.PhotoUrl = JsonConvert.SerializeObject(new List<SimpleFileInfo>(){
                    fileinfo
                });
                }

                string acc = GetAccount(coreContext, result.Data.OAuthResult.OpenId);
                if (string.IsNullOrEmpty(acc))
                {
                    return ErrorResult<LoginOuathResult>.NoSignup;
                }
                else
                {
                    result.Data.Token = acc;

                    coreContext.Database.ExecuteSqlCommandAsync($@"update member_info set sex=ifnull(sex,{result.Data.OAuthResult.Sex}),member_name=ifnull(member_name,{result.Data.OAuthResult.NickName})
                    ,nick_name=ifnull(nick_name,{result.Data.OAuthResult.NickName}),photo_url=ifnull(photo_url,{result.Data.OAuthResult.PhotoUrl}) where account_id={acc}");


                }

                return result;
            }

        }

        protected abstract LoginOAuthData GetOAuthResult(CoreContext coreContext, IDictionary<string, object> parameter);
        protected abstract string GetAccount(CoreContext coreContext, string openeId);
    }
}
