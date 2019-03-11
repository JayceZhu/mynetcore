using Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PubService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sk.core.Filters
{
    public class ApiSecurityFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sign = context.HttpContext.Request.Headers["sign"];
            var appsecret = ConfigurationUtil.GetSection("ZlanApi")["APP_SECRECT"] as string;
            var nonceStr = context.HttpContext.Request.Headers["noncestr"];
            var timestamp = context.HttpContext.Request.Headers["timestamp"];
            //根据请求类型拼接参数
            var query = context.HttpContext.Request.Query;
            string data = string.Empty;
            string method = context.HttpContext.Request.Method;
            switch (method)
            {
                case "POST":
                    System.IO.Stream stream = context.HttpContext.Request.Body;
                    string responseJson = string.Empty;
                    StreamReader streamReader = new StreamReader(stream);
                    data = streamReader.ReadToEnd();
                    break;
                case "GET":
                    data = query.OrderBy(k => k.Key).ToString();
                    break;
                default:
                    context.Result = new ObjectResult(ErrorResult<int>.ParameterError);
                    return;
            }
            var appid = context.HttpContext.Request.Headers["appid"];
            if (SingUtil.Sign(appid, appsecret, nonceStr, timestamp, data) != sign)
            {
                context.Result = new ObjectResult(ErrorResult<int>.NoAuthorization);
                return;
            }
            await next();
        }

    }
}
