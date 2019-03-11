using Command;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbcService
{
    public class SignupAutoParameter
    {
        public string OAuthKind { get; set; }

        public LoginOAuthData OAuthData { get; set; }
    }
    public class SignupAutoResult
    {
        public string Token { get; set; }
    }
    public class SignupAutoCommand : Command<SignupAutoResult>
    {
        protected override CommandResult<SignupAutoResult> OnExecute(object commandParameter)
        {
            var result = new CommandResult<SignupAutoResult>();
            var param = commandParameter as SignupAutoParameter;
            string openidField = "";
            switch (param.OAuthKind)
            {
                case "wx":
                    openidField = "wx_open_id"; break;
                case "xcx":
                    openidField = "xcx_open_id"; break;
                default:
                    break;
            }
            if (string.IsNullOrEmpty(openidField))
            {
                return ErrorResult<SignupAutoResult>.ParameterError;
            }
            using (CoreContext db = new CoreContext())
            {
                string acc = Guid.NewGuid().ToString("N");

                db.Database.ExecuteSqlCommandAsync($@"INSERT into member_info (account_id,{openidField},wx_unionid,member_name,nick_name,sex,photo_url,log_times,owner_account,coins,score,`status`,`level`,dept_code,add_date,last_log) 
                                              VALUES({acc},{param.OAuthData.OpenId},{param.OAuthData.UnionId},{param.OAuthData.NickName},{param.OAuthData.NickName},:{param.OAuthData.Sex},{param.OAuthData.PhotoUrl}, 
                                              0,'admin', 0, 0, '1', 1,'0001',{DateTime.Now},{DateTime.Now})");
                result.Data.Token = acc;
            }

            return result;
        }
    }
}
