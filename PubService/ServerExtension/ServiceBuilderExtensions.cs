using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubService.Server
{
    public static class ServiceBuilderExtensions
    {
        public static void AddServiceExtensions(this IServiceCollection services)
        {
            
            ////注册HttpContextAccessor服务
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            ////注册HttpReques服务
            services.AddSingleton<IHttpRequestServer,HttpRequestServer>();
        }
    }
}
