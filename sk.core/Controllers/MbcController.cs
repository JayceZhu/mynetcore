using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command;
using MbcService;
using Microsoft.AspNetCore.Mvc;
using Model.CommandData;
using PubService;
using sk.core.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace sk.core.Controllers
{
    [Route("api/Mbc/[action]")]
    [ApiController]
    public class MbcController : ControllerBase
    {
        /// <summary>
        /// 小程序登陆
        /// </summary>
        /// <param name="LoginOAuthParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<object> LoginXcx([FromBody] LoginOAuthParameter LoginOAuthParameter)
        {
            var result = ErrorResult<object>.ParameterError;
            if (LoginOAuthParameter.LoginData.ContainsKey("code") && !string.IsNullOrEmpty(LoginOAuthParameter.LoginData["code"] as string))
            {
                var loginRes = new LoginXcxCommand().Execute(LoginOAuthParameter);
                if (loginRes.ErrorCode == 0 && !string.IsNullOrEmpty(loginRes.Data.Token))
                {
                    result.ErrorCode = 0;
                    result.ErrorMessage = "登陆成功";
                    result.Data = loginRes.Data.Token;
                }
                else if (loginRes.ErrorCode == -4)
                {
                    var singupRes = new SignupAutoCommand().Execute(new SignupAutoParameter { OAuthKind = "xcx", OAuthData = loginRes.Data.OAuthResult });
                    if (singupRes.ErrorCode == 0)
                    {
                        result.ErrorCode = 0;
                        result.ErrorMessage = "登陆成功";
                        result.Data = singupRes.Data.Token;
                    }
                    else
                    {
                        result.ErrorMessage = $"#{singupRes.ErrorMessage}#【如遇到无法登录情况，请稍后再试。如仍然无法登录，请联系客服】";
                    }
                }
                else
                {
                    result.ErrorCode = loginRes.ErrorCode;
                    result.ErrorMessage = $"#{loginRes.ErrorMessage}#【如遇到无法登录情况，请稍后再试。如仍然无法登录，请联系客服】";
                }
            }

            return result;
        }


        /// <summary>
        /// 获取商城授权
        /// </summary>
        /// <param name="MemberCode"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<LoginShopOAuthReulst> LoginShopOuath(string MemberCode)
        {
            return new LoginShopOAuthCommand().Execute(new LoginShopOAuthParamter()
            {
                OAuthAppId = ConfigurationUtil.GetSection("ShopOAuth")["OAuthAppId"],
                OAuthAppSecret = ConfigurationUtil.GetSection("ShopOAuth")["OAuthAppSecret"],
                InterfaceUrl = ConfigurationUtil.GetSection("ShopOAuth")["InterfaceUrl"],
                MemberCode = MemberCode
            });
        }

        /// <summary>
        /// 微信公众号登录
        /// </summary>
        /// <param name="LoginOAuthParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<object> LoginWeiXin([FromBody] LoginOAuthParameter LoginOAuthParameter)
        {
            var result = ErrorResult<object>.ParameterError;
            if (LoginOAuthParameter.LoginData.ContainsKey("code") && !string.IsNullOrEmpty(LoginOAuthParameter.LoginData["code"] as string))
            {
                var loginRes = new LoginWeixinCommand().Execute(LoginOAuthParameter);
                if (loginRes.ErrorCode == 0 && !string.IsNullOrEmpty(loginRes.Data.Token))
                {
                    result.ErrorCode = 0;
                    result.ErrorMessage = "登陆成功";
                    result.Data = loginRes.Data.Token;
                }
                else if (loginRes.ErrorCode == -4)
                {
                    var singupRes = new SignupAutoCommand().Execute(new SignupAutoParameter { OAuthKind = "wx", OAuthData = loginRes.Data.OAuthResult });
                    if (singupRes.ErrorCode == 0)
                    {
                        result.ErrorCode = 0;
                        result.ErrorMessage = "登陆成功";
                        result.Data = singupRes.Data.Token;
                    }
                    else
                    {
                        result.ErrorMessage = $"#{singupRes.ErrorMessage}#【如遇到无法登录情况，请稍后再试。如仍然无法登录，请联系客服】";
                    }
                }
                else
                {
                    result.ErrorCode = loginRes.ErrorCode;
                    result.ErrorMessage = $"#{loginRes.ErrorMessage}#【如遇到无法登录情况，请稍后再试。如仍然无法登录，请联系客服】";
                }
            }

            return result;
        }

        /// <summary>
        /// 获取地址明细
        /// </summary>
        /// <param name="LoadAddressDetailParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<AddressData> LoadAddressDetail([FromBody] LoadAddressDetailParameter LoadAddressDetailParameter)
        {
            return new LoadAddressDetailCommand().Execute(LoadAddressDetailParameter);
        }

        /// <summary>
        /// 获取地址列表
        /// </summary>
        /// <param name="LoadAddressListParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<List<AddressData>> LoadAddressList([FromBody] LoadAddressListParameter LoadAddressListParameter)
        {
            return new LoadAddressListCommand().Execute(LoadAddressListParameter);
        }

        /// <summary>
        /// 保存地址信息
        /// </summary>
        /// <param name="SaveAddressParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<System.Collections.Hashtable> SaveAddress([FromBody] SaveAddressParameter SaveAddressParameter)
        {
            return new SaveAddressCommand().Execute(SaveAddressParameter);
        }
    }
}
