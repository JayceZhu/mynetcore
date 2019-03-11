using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sk.core.Filters
{
    public class CacheFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var redisdb = RedisClient.GetDatabase();
            string controller = context.RouteData.Values["controller"].ToString().ToLower();
            string api = controller + "_" + context.RouteData.Values["action"].ToString().ToLower();
            long cacheTime = (long)redisdb.HashGet("sk.core.cachetime", controller);
            string jsonResult = "";
            string cacheKey = "sk.core.cachedata:" + api;
            //无需缓存
            if (cacheTime != 0)
            {
                foreach (var arg in context.ActionArguments)
                {
                    MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
                    var md5ByteArray = m5.ComputeHash(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(arg.Value)));
                    cacheKey += ":" + BitConverter.ToString(md5ByteArray).Replace("-", "").ToUpper();
                }
                //从缓存中获取数据
                jsonResult = redisdb.StringGet(cacheKey);
                try
                {
                    //直接使用缓存的数据
                    if (!string.IsNullOrEmpty(jsonResult))
                    {

                        context.Result = new ObjectResult(jsonResult);
                        return;
                    }

                    //异步回调
                    await next();

                    var contextResult = next().Result.Result as ObjectResult;
                    redisdb.StringSet(cacheKey, JsonConvert.SerializeObject(contextResult.Value), TimeSpan.FromSeconds(cacheTime));
                }
                catch (Exception ex)
                {
                    LogUtil.Log("CacheFilter", api, ex.Message);
                }
            }
            else
            {
                await base.OnActionExecutionAsync(context, next);
            }


        }

    }
}
