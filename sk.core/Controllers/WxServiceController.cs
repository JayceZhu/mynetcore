using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService;
using PubService;
using sk.core.Filters;
using static PubService.WeiXinUtil;

namespace sk.core.Controllers
{
    [Route("api/WxService/[action]")]
    [ApiController]
    public class WxServiceController : ControllerBase
    {
        /// <summary>
        /// 获取支付签名
        /// </summary>
        /// <param name="GetWxPaymentSignParameter"></param>
        /// <returns></returns>
        [MemberParamterFilter]
        [HttpPost]
        public CommandResult<GetWxPaymentSignResult> GetWxPaymentSign([FromBody] GetWxPaymentSignParameter GetWxPaymentSignParameter)
        {
            return new GetWxPaymentSignCommand(HttpContext).Execute(GetWxPaymentSignParameter);
        }


        /// <summary>
        /// 获取JSDK的签名
        /// </summary>
        /// <param name="GetScriptSignatureParameter"></param>
        /// <returns></returns>
        [HttpPost, HttpGet]
        public CommandResult<ConfigData> GetScriptSignature([FromBody] GetScriptSignatureParameter GetScriptSignatureParameter)
        {
            return new GetScriptSignatureCommand().Execute(GetScriptSignatureParameter);
        }

        /// <summary>
        /// 检查是否已经支付
        /// </summary>
        /// <param name="CheckWeiXinPayParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<int> CheckWeiXinPay([FromBody] CheckWeiXinPayParameter CheckWeiXinPayParameter)
        {
            return new CheckWeiXinPayCommand().Execute(CheckWeiXinPayParameter);
        }
    }
}